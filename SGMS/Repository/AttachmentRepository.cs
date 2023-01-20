using Microsoft.EntityFrameworkCore;
using SGMS.Contract;
using SGMS.Data;
using SGMS.Models;
using System.Linq.Expressions;

namespace SGMS.Repository
{
    public class AttachmentRepository : IUnitOfWork<Attachment>
    {
        private readonly ApplicationDbContext _dbContext;
        public AttachmentRepository(ApplicationDbContext dbContext)
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
        public async Task<Attachment> OnItemCreationAsync(Attachment attachment)
        {
            try
            {
                await _dbContext.AddAsync(attachment);

                return attachment;
            }
            catch (Exception)
            {

                throw new Exception("Error: Failed to register attachment");
            }
        }
        public async Task<Attachment> OnLoadItemAsync(Guid AttachmentId)
        {
            try
            {
                var attachment = await _dbContext.Attachments.Where(m => m.AttachmentId == AttachmentId).FirstOrDefaultAsync();

                if (attachment is null)
                {
                    throw new Exception("Error!");
                }

                return attachment;

            }
            catch (Exception)
            {

                throw new Exception("Error: Unable to load item");
            }
        }
        public async Task<List<Attachment>> OnLoadItemsAsync()
        {
            try
            {
                var attachments = await _dbContext.Attachments.ToListAsync();

                var getActiveAttachments = from n in attachments

                                           select n;

                return getActiveAttachments.ToList();

            }
            catch (Exception)
            {

                throw new Exception("Error: Attachments could not be loaded!");
            }
        }
        public async Task<Attachment> OnModifyItemAsync(Attachment attachment)
        {
            Attachment results = new Attachment();

            try
            {
                results = await _dbContext.Attachments.FirstOrDefaultAsync(x => x.AttachmentId == attachment.AttachmentId);

                if (results != null)
                {

                    results.AssociativeKey = attachment.AssociativeKey;

                    results.File = attachment.File;

                    results.Status = attachment.Status;

                    results.QualificationName = attachment.QualificationName;

                   await _dbContext.SaveChangesAsync();

                }
            }
            catch (Exception)
            {

                throw new Exception("Error: Attachment Failed to Modify!");
            }

            return results;
        }
        public async Task<int> OnRemoveItemAsync(Guid AttachmentId)
        {
            Attachment attachment = new Attachment();
            int record = 0;

            try
            {
                attachment = await _dbContext.Attachments.FirstOrDefaultAsync(m => m.AttachmentId == AttachmentId);

                if (attachment != null)
                {
                    _dbContext.Remove(attachment);

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
