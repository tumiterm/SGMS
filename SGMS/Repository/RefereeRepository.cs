using Microsoft.EntityFrameworkCore;
using SGMS.Contract;
using SGMS.Data;
using SGMS.Models;
using System.Linq.Expressions;

namespace SGMS.Repository
{
    public class RefereeRepository : IUnitOfWork<Referee>
    {
        private readonly ApplicationDbContext _dbContext;
        public RefereeRepository(ApplicationDbContext dbContext)
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
        public async Task<Referee> OnItemCreationAsync(Referee referee)
        {
            try
            {
                await _dbContext.AddAsync(referee);

                return referee;
            }
            catch (Exception)
            {

                throw new Exception("Error: Failed to register referee");
            }
        }
        public async Task<Referee> OnLoadItemAsync(Guid RefereeId)
        {
            try
            {
                var referee = await _dbContext.Referees.Where(m => m.RefereeId == RefereeId).FirstOrDefaultAsync();

                if ( referee is null)
                {
                    throw new Exception("Error!");
                }

                return referee;
                //  return await _dbContext.Employees.FirstOrDefaultAsync(m => m.EmployeeId == EmployeeId);

            }
            catch (Exception)
            {

                throw new Exception("Error: Unable to load item");
            }
        }
        public async Task<List<Referee>> OnLoadItemsAsync()
        {
            try
            {
                var referees = await _dbContext.Referees.ToListAsync();

                var getActiveReferees = from n in referees

                                       select n;

                return getActiveReferees.ToList();

            }
            catch (Exception)
            {

                throw new Exception("Error: Referees could not be loaded!");
            }
        }
        public async Task<Referee> OnModifyItemAsync(Referee referee)
        {
            Referee results = new Referee();

            try
            {
                results = await _dbContext.Referees.FirstOrDefaultAsync(x => x.RefereeId == referee.RefereeId);

                if (results != null)
                {
                    results.Contact = new Contact();

                    results.IsActive = referee.IsActive;

                    results.LastName = referee.LastName;

                    results.Name = referee.Name;

                    results.Title = referee.Title;

                    results.Photo = referee.Photo;
                     
                    results.RefereeLevel = referee.RefereeLevel;

                    results.RefereeType = referee.RefereeType;

                    results.HasJoinedLFA = referee.HasJoinedLFA;

                    results.RefereeLevel = referee.RefereeLevel;

                }
            }
            catch (Exception)
            {

                throw new Exception("Error: Referee Failed to Modify!");
            }

            return results;
        }
        public async Task<int> OnRemoveItemAsync(Guid RefereeId)
        {
            Referee referee = new Referee();

            int record = 0;

            try
            {
                referee = await _dbContext.Referees.FirstOrDefaultAsync(m => m.RefereeId == RefereeId);

                if (referee != null)
                {
                    _dbContext.Remove(referee);

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
