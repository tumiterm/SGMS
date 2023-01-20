using Microsoft.EntityFrameworkCore;
using SGMS.Contract;
using SGMS.Data;
using SGMS.Models;
using System.Linq.Expressions;

namespace SGMS.Repository
{
    public class TeamRepository : IUnitOfWork<Team>
    {
        private readonly ApplicationDbContext _dbContext;
        public TeamRepository(ApplicationDbContext dbContext)
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
        public async Task<Team> OnItemCreationAsync(Team team)
        {
            try
            {
                await _dbContext.AddAsync(team);

                return team;
            }
            catch (Exception)
            {

                throw new Exception("Error: Failed to register team");
            }
        }
        public async Task<Team> OnLoadItemAsync(Guid TeamId)
        {
            try
            {
                return await _dbContext.Teams.FirstOrDefaultAsync(m => m.TeamId == TeamId);
            }
            catch (Exception)
            {

                throw new Exception("Error: Unable to load item");
            }
        }
        public async Task<List<Team>> OnLoadItemsAsync()
        {
            try
            {
                var teams = await _dbContext.Teams.ToListAsync();

                var getActiveTeams = from n in teams

                                        select n;

                return getActiveTeams.ToList();

            }
            catch (Exception)
            {

                throw new Exception("Error: Teams could not be loaded!");
            }
        }
        public async Task<Team> OnModifyItemAsync(Team team)
        {
            Team results = new Team();

            try
            {
                results = await _dbContext.Teams.FirstOrDefaultAsync(x => x.TeamId == team.TeamId);

                if (results != null)
                {

                    results.Description = team.Description;

                    results.IsActive = team.IsActive;

                    results.TeamLogo = team.TeamLogo;

                    results.TeamLogoFile = team.TeamLogoFile;

                    results.MunicipalityId = team.MunicipalityId;

                    results.DistrictId = team.DistrictId;

                    results.Role = team.Role;

                    results.TeamName = team.TeamName;

                    await _dbContext.SaveChangesAsync();

                }
            }
            catch (Exception)
            {

                throw new Exception("Error: Team Failed to Modify!");
            }

            return results;
        }
        public async Task<int> OnRemoveItemAsync(Guid TeamId)
        {
            Team team = new Team();

            int record = 0;

            try
            {
                team = await _dbContext.Teams.FirstOrDefaultAsync(m => m.TeamId == TeamId);

                if (team != null)
                {
                    _dbContext.Remove(team);

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
