using Microsoft.EntityFrameworkCore;
using SGMS.Contract;
using SGMS.Data;
using SGMS.Models;
using System.Linq.Expressions;

namespace SGMS.Repository
{
    public class PlayerSelectionRepository : IUnitOfWork<PlayerSelection>
    {
        private readonly ApplicationDbContext _dbContext;
        public PlayerSelectionRepository(ApplicationDbContext dbContext)
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
        public async Task<PlayerSelection> OnItemCreationAsync(PlayerSelection selection)
        {
            try
            {
                await _dbContext.AddAsync(selection);

                return selection;
            }
            catch (Exception)
            {

                throw new Exception("Error: Failed to register selection");
            }
        }
        public async Task<PlayerSelection> OnLoadItemAsync(Guid Id)
        {
            try
            {
                var pSelection = await _dbContext.Selections.Where(m => m.Id == Id).FirstOrDefaultAsync();

                if (pSelection is null)
                {
                    throw new Exception("Error!");
                }

                return pSelection;

            }
            catch (Exception)
            {

                throw new Exception("Error: Unable to load item");
            }
        }
        public async Task<List<PlayerSelection>> OnLoadItemsAsync()
        {
            try
            {
                var players = await _dbContext.Selections.ToListAsync();

                var getActivePlayers = from n in players

                                       select n;

                return getActivePlayers.ToList();

            }
            catch (Exception)
            {

                throw new Exception("Error: players could not be loaded!");
            }
        }
        public async Task<PlayerSelection> OnModifyItemAsync(PlayerSelection selection)
        {
            PlayerSelection results = new PlayerSelection();

            try
            {
                results = await _dbContext.Selections.FirstOrDefaultAsync(x => x.Id == selection.Id);

                if (results != null)
                {
                    results.IsActive = selection.IsActive;

                    results.IsSubstitute = selection.IsSubstitute;

                    results.MatchId = selection.MatchId;

                    results.ModifiedBy = selection.ModifiedBy;

                    results.ModifiedOn = selection.ModifiedOn;

                    results.Number = selection.Number;

                    results.PLayerId = selection.PLayerId;

                    results.Position = selection.Position;

                    results.TeamId = selection.TeamId;

                    await _dbContext.SaveChangesAsync();

                }
            }
            catch (Exception)
            {

                throw new Exception("Error: selection Failed to Modify!");
            }

            return results;
        }
        public async Task<int> OnRemoveItemAsync(Guid Id)
        {
            PlayerSelection selection = new PlayerSelection();

            int record = 0;

            try
            {
                selection = await _dbContext.Selections.FirstOrDefaultAsync(m => m.Id == Id);

                if (selection != null)
                {
                    _dbContext.Remove(selection);

                    record = await _dbContext.SaveChangesAsync();

                    return record;
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
