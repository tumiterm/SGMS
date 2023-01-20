using Microsoft.EntityFrameworkCore;
using SGMS.Contract;
using SGMS.Data;
using SGMS.Models;
using System.Linq.Expressions;

namespace SGMS.Repository
{
    public class SponsorRepository : IUnitOfWork<Sponsor>
    {
        private readonly ApplicationDbContext _dbContext;
        public SponsorRepository(ApplicationDbContext dbContext)
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
        public async Task<Sponsor> OnItemCreationAsync(Sponsor sponsor)
        {
            try
            {
                await _dbContext.AddAsync(sponsor);

                return sponsor;
            }
            catch (Exception)
            {

                throw new Exception("Error: Failed to register item");
            }
        }
        public async Task<Sponsor> OnLoadItemAsync(Guid SponsorId)
        {
            try
            {
                return await _dbContext.Sponsors.FirstOrDefaultAsync(m => m.SponsorId == SponsorId);
            }
            catch (Exception)
            {

                throw new Exception("Error: Unable to load item");
            }
        }
        public async Task<List<Sponsor>> OnLoadItemsAsync()
        {
            try
            {
                var sponsors = await _dbContext.Sponsors.ToListAsync();

                var getActiveSponsors = from n in sponsors

                                           select n;

                return getActiveSponsors.ToList();

            }
            catch (Exception)
            {

                throw new Exception("Error: Items could not be loaded!");
            }
        }
        public async Task<Sponsor> OnModifyItemAsync(Sponsor sponsor)
        {
            Sponsor results = new Sponsor();

            try
            {
                results = await _dbContext.Sponsors.FirstOrDefaultAsync(x => x.SponsorId == sponsor.SponsorId);

                if (results != null)
                {
                    results.Email = sponsor.Email;

                    results.LastName = sponsor.LastName;

                    results.Name = sponsor.Name;

                    results.Phone = sponsor.Phone;

                    results.Title = sponsor.Title;

                }
            }
            catch (Exception)
            {

                throw new Exception("Error: Entity Failed to Modify!");
            }

            return results;
        }
        public async Task<int> OnRemoveItemAsync(Guid Id)
        {
            Sponsor sponsor = new Sponsor();

            int record = 0;

            try
            {
                sponsor = await _dbContext.Sponsors.FirstOrDefaultAsync(m => m.TournamentId == Id);

                if (sponsor != null)
                {
                    _dbContext.Remove(sponsor);

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
