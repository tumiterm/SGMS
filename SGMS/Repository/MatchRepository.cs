using Microsoft.EntityFrameworkCore;
using SGMS.Contract;
using SGMS.Data;
using SGMS.Models;
using System.Linq.Expressions;

namespace SGMS.Repository
{
    public class MatchRepository : IUnitOfWork<Match>
    {
        private readonly ApplicationDbContext _dbContext;
        public MatchRepository(ApplicationDbContext dbContext)
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
        public async Task<Match> OnItemCreationAsync(Match match)
        {
            try
            {
                await _dbContext.AddAsync(match);

                return match;
            }
            catch (Exception)
            {

                throw new Exception("Error: Failed to register match");
            }
        }
        public async Task<Match> OnLoadItemAsync(Guid MatchId)
        {
            try
            {
                var match = await _dbContext.Matches.Where(m => m.MatchId == MatchId).FirstOrDefaultAsync();

                if (match is null)
                {
                    throw new Exception("Error!");
                }

                return match;
                //  return await _dbContext.Employees.FirstOrDefaultAsync(m => m.EmployeeId == EmployeeId);

            }
            catch (Exception)
            {

                throw new Exception("Error: Unable to load match");
            }
        }
        public async Task<List<Match>> OnLoadItemsAsync()
        {
            try
            {
                var matches = await _dbContext.Matches.ToListAsync();

                var getActiveMatches = from n in matches

                                           select n;

                return getActiveMatches.ToList();

            }
            catch (Exception)
            {

                throw new Exception("Error: Marches could not be loaded!");
            }
        }
        public async Task<Match> OnModifyItemAsync(Match match)
        {
            Match results = new Match();

            try
            {
                results = await _dbContext.Matches.FirstOrDefaultAsync(x => x.MatchId == match.MatchId);

                if (results != null)
                {

                    results.FirstTeamId = match.FirstTeamId;

                    results.GameDay = match.GameDay;

                    results.IsActive = match.IsActive;

                    results.PlayerId = match.PlayerId;

                    results.RefereeId = match.RefereeId;

                    results.Score1 = match.Score1;

                    results.Score2 = match.Score2;

                    results.SecondTeamId = match.SecondTeamId;

                    results.Status = match.Status;

                    await _dbContext.SaveChangesAsync();

                }
            }
            catch (Exception)
            {

                throw new Exception("Error: Match Failed to Modify!");
            }

            return results;
        }
        public async Task<int> OnRemoveItemAsync(Guid MatchId)
        {
            Match match = new Match();

            int record = 0;

            try
            {
                match = await _dbContext.Matches.FirstOrDefaultAsync(m => m.MatchId == MatchId);

                if (match != null)
                {
                    _dbContext.Remove(match);

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
