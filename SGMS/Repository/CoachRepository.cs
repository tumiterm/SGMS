using Microsoft.EntityFrameworkCore;
using SGMS.Contract;
using SGMS.Data;
using SGMS.Models;
using System.Linq.Expressions;

namespace SGMS.Repository
{
    public class CoachRepository : IUnitOfWork<Coach>
    {
        private readonly ApplicationDbContext _dbContext;
        public CoachRepository(ApplicationDbContext dbContext)
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
        public async Task<Coach> OnItemCreationAsync(Coach coach)
        {
            try
            {
                await _dbContext.AddAsync(coach);

                return coach;
            }
            catch (Exception)
            {

                throw new Exception("Error: Failed to register coach");
            }
        }
        public async Task<Coach> OnLoadItemAsync(Guid CoachId)
        {
            try
            {
                var coach =  await _dbContext.Coaches.Where(m => m.CoachId == CoachId).FirstOrDefaultAsync();

                if(coach is null)
                {
                    throw new Exception("Error!");
                }

                return coach;
                //  return await _dbContext.Employees.FirstOrDefaultAsync(m => m.EmployeeId == EmployeeId);

            }
            catch (Exception)
            {

                throw new Exception("Error: Unable to load item");
            }
        }
        public async Task<List<Coach>> OnLoadItemsAsync()
        {
            try
            {
                var coaches = await _dbContext.Coaches.ToListAsync();

                var getActiveCoaches = from n in coaches

                                     select n;

                return getActiveCoaches.ToList();

            }
            catch (Exception)
            {

                throw new Exception("Error: Coaches could not be loaded!");
            }
        }
        public async Task<Coach> OnModifyItemAsync(Coach coach)
        {
            Coach results = new Coach();

            try
            {
                results = await _dbContext.Coaches.FirstOrDefaultAsync(x => x.CoachId == coach.CoachId);

                if (results != null)
                {
                    results.Contact = new Contact();

                    results.IDNumber = coach.IDNumber;

                    results.IsActive = coach.IsActive;

                    results.LastName = coach.LastName;

                    results.Name = coach.Name;

                    results.RSAIDCopy = coach.RSAIDCopy;

                    results.Title = coach.Title;

                }
            }
            catch (Exception)
            {

                throw new Exception("Error: Coach Failed to Modify!");
            }

            return results;
        }
        public async Task<int> OnRemoveItemAsync(Guid CoachId)
        {
            Coach coach = new Coach();

            int record = 0;

            try
            {
                coach = await _dbContext.Coaches.FirstOrDefaultAsync(m => m.CoachId == CoachId);

                if (coach != null)
                {
                    _dbContext.Remove(coach);

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
