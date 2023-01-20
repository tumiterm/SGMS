using Microsoft.EntityFrameworkCore;
using SGMS.Contract;
using SGMS.Data;
using SGMS.Models;
using System.Linq.Expressions;

namespace SGMS.Repository
{
    public class DistrictRepository : IUnitOfWork<District>
    {
        private readonly ApplicationDbContext _dbContext;
        public DistrictRepository(ApplicationDbContext dbContext)
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
        public async Task<District> OnItemCreationAsync(District district)
        {
            try
            {
                await _dbContext.AddAsync(district);

                return district;
            }
            catch (Exception)
            {

                throw new Exception("Error: Failed to register district");
            }
        }
        public async Task<District> OnLoadItemAsync(Guid DistrictId)
        {
            try
            {
                var district = await _dbContext.Districts.Where(m => m.DistrictId == DistrictId).FirstOrDefaultAsync();

                if (district is null)
                {
                    throw new Exception("Error!");
                }

                return district;

            }
            catch (Exception)
            {

                throw new Exception("Error: Unable to load district");
            }
        }
        public async Task<List<District>> OnLoadItemsAsync()
        {
            try
            {
                var districts = await _dbContext.Districts.ToListAsync();

                var getDistricts = from n in districts

                                   select n;

                return getDistricts.ToList();

            }
            catch (Exception)
            {

                throw new Exception("Error: districts could not be loaded!");
            }
        }
        public async Task<District> OnModifyItemAsync(District district)
        {
            District results = new District();

            try
            {
                results = await _dbContext.Districts.FirstOrDefaultAsync(x => x.DistrictId == district.DistrictId);

                if (results != null)
                {

                    results.DistrictName = district.DistrictName;

                    await _dbContext.SaveChangesAsync();

                }
            }
            catch (Exception)
            {

                throw new Exception("Error: District Failed to Modify!");
            }

            return results;
        }
        public async Task<int> OnRemoveItemAsync(Guid DistrictId)
        {
            District district = new District();

            int record = 0;

            try
            {
                district = await _dbContext.Districts.FirstOrDefaultAsync(m => m.DistrictId == DistrictId);

                if (district != null)
                {
                    _dbContext.Remove(district);

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
