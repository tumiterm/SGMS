using Microsoft.EntityFrameworkCore;
using SGMS.Contract;
using SGMS.Data;
using SGMS.Models;
using System.Linq.Expressions;

namespace SGMS.Repository
{
    public class ContactRepository : IUnitOfWork<Contact>
    {
        private readonly ApplicationDbContext _dbContext;
        public ContactRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool DoesEntityExist<TEntity>(Expression<Func<TEntity, bool>> predicate = null) where TEntity : class
        {
            IQueryable<TEntity> data = _dbContext.Set<TEntity>();

            return data.Any(predicate);
        }
        public async Task<int> ItemSaveAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
        public async Task<Contact> OnItemCreationAsync(Contact contact)
        {
            try
            {
                await _dbContext.AddAsync(contact);

                return contact;
            }
            catch (Exception)
            {

                throw new Exception("Error: Failed to add contact");
            }
        }
        public async Task<Contact> OnLoadItemAsync(Guid ContactId)
        {
            try
            {
                return await _dbContext.Contacts.FirstOrDefaultAsync(m => m.ContactId == ContactId);
            }
            catch (Exception)
            {

                throw new Exception("Error: Unable to load contact");
            }
        }
        public async Task<List<Contact>> OnLoadItemsAsync()
        {
            try
            {
                var contacts = await _dbContext.Contacts.ToListAsync();

                var getAllContacts = from n in contacts

                                     select n;


                return getAllContacts.ToList();

            }
            catch (Exception)
            {

                throw new Exception("Error: Contacts could not be loaded!");
            }
        }
        public async Task<Contact> OnModifyItemAsync(Contact contact)
        {
            Contact results = new Contact();

            try
            {
                results = await _dbContext.Contacts.FirstOrDefaultAsync(x => x.ContactId == contact.ContactId);

                if (results != null)
                {
                    results.Alternative = contact.Alternative;
                   
                    results.Email = contact.Email;
                   
                    results.Phone = contact.Phone;
                }
            }
            catch (Exception)
            {

                throw new Exception("Error: Contact Failed to Modify!");
            }

            return results;
        }
        public async Task<int> OnRemoveItemAsync(Guid ContactId)
        {
            Contact contact = new Contact();

            int record = 0;

            try
            {
                contact = await _dbContext.Contacts.FirstOrDefaultAsync(m => m.ContactId == ContactId);

                if (contact != null)
                {
                    _dbContext.Remove(contact);

                   record = await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw new Exception("Error: Delete Failed");
            }

            return record;
        }
    }
}
