using Microsoft.EntityFrameworkCore;
using SGMS.Contract;
using SGMS.Data;
using SGMS.Models;
using System.Linq.Expressions;

namespace SGMS.Repository
{
    public class HelpContactsRepository : IUnitOfWork<SysUsers>
    {
        private readonly ApplicationDbContext _dbContext;
        public HelpContactsRepository(ApplicationDbContext dbContext)
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
        public async Task<SysUsers> OnItemCreationAsync(SysUsers users)
        {
            try
            {
                await _dbContext.AddAsync(users);

                return users;
            }
            catch (Exception)
            {

                throw new Exception("Error: Failed to register users");
            }
        }
        public async Task<SysUsers> OnLoadItemAsync(Guid Id)
        {
            try
            {
                var users = await _dbContext.EmergencyUsers.Where(m => m.Id == Id).FirstOrDefaultAsync();

                if (users is null)
                {
                    throw new Exception("Error!");
                }

                return users;

            }
            catch (Exception)
            {

                throw new Exception("Error: Unable to load users");
            }
        }
        public async Task<List<SysUsers>> OnLoadItemsAsync()
        {
            try
            {
                var users = await _dbContext.EmergencyUsers.ToListAsync();

                var getActiveUsers = from n in users

                                           select n;

                return getActiveUsers.ToList();

            }
            catch (Exception)
            {

                throw new Exception("Error: users could not be loaded!");
            }
        }
        public async Task<SysUsers> OnModifyItemAsync(SysUsers users)
        {
            SysUsers results = new SysUsers();

            try
            {
                results = await _dbContext.EmergencyUsers.FirstOrDefaultAsync(x => x.Id == users.Id);

                if (results != null)
                {
                    results.Name = users.Name;

                    results.LastName = users.LastName;

                    results.Phone = users.Phone;

                    results.Role = users.Role;

                }
            }
            catch (Exception)
            {

                throw new Exception("Error: User Failed to Modify!");
            }

            return results;
        }
        public async Task<int> OnRemoveItemAsync(Guid Id)
        {
            SysUsers users = new SysUsers();

            int record = 0;

            try
            {
                users = await _dbContext.EmergencyUsers.FirstOrDefaultAsync(m => m.Id == Id);

                if (users != null)
                {
                    _dbContext.Remove(users);

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
