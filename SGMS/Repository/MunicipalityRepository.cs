using Microsoft.EntityFrameworkCore;
using SGMS.Contract;
using SGMS.Data;
using SGMS.Models;
using System.Linq.Expressions;

namespace SGMS.Repository
{
    public class MunicipalityRepository : IUnitOfWork<Municipality>
    {
        private readonly ApplicationDbContext _dbContext;
        public MunicipalityRepository(ApplicationDbContext dbContext)
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
        public async Task<Municipality> OnItemCreationAsync(Municipality municipality)
        {
            try
            {
                await _dbContext.AddAsync(municipality);

                return municipality;
            }
            catch (Exception)
            {

                throw new Exception("Error: Failed to register municipality");
            }
        }
        public async Task<Municipality> OnLoadItemAsync(Guid MunicipalityId)
        {
            try
            {
                var municipality = await _dbContext.Municipalities.Where(m => m.MunicipalityId == MunicipalityId).FirstOrDefaultAsync();

                if (municipality is null)
                {
                    throw new Exception("Error!");
                }

                return municipality;

            }
            catch (Exception)
            {

                throw new Exception("Error: Unable to load municipality");
            }
        }
        public async Task<List<Municipality>> OnLoadItemsAsync()
        {
            try
            {
                var municipalities = await _dbContext.Municipalities.ToListAsync();

                var getMunicipalities = from n in municipalities

                                   select n;

                return getMunicipalities.ToList();

            }
            catch (Exception)
            {

                throw new Exception("Error: Municipalities could not be loaded!");
            }
        }
        public async Task<Municipality> OnModifyItemAsync(Municipality municipality)
        {
            Municipality results = new Municipality();

            try
            {
                results = await _dbContext.Municipalities.FirstOrDefaultAsync(x => x.MunicipalityId == municipality.MunicipalityId);

                if (results != null)
                {

                    results.MunicipalityName = municipality.MunicipalityName;

                    results.DistrictId = municipality.DistrictId;

                    await _dbContext.SaveChangesAsync();

                }
            }
            catch (Exception)
            {

                throw new Exception("Error: Municipslity Failed to Modify!");
            }

            return results;
        }
        public async Task<int> OnRemoveItemAsync(Guid MunicipalityId)
        {
            Municipality municipality = new Municipality();

            int record = 0;

            try
            {
                municipality = await _dbContext.Municipalities.FirstOrDefaultAsync(m => m.MunicipalityId == MunicipalityId);

                if (municipality != null)
                {
                    _dbContext.Remove(municipality);

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
