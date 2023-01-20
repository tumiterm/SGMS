using Microsoft.EntityFrameworkCore;
using SGMS.Contract;
using SGMS.Data;
using SGMS.Models;
using System.Linq.Expressions;

namespace SGMS.Repository
{
    public class TournamentRepository : IUnitOfWork<Tournament>
    {
        private readonly ApplicationDbContext _dbContext;
        public TournamentRepository(ApplicationDbContext dbContext)
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
        public async Task<Tournament> OnItemCreationAsync(Tournament tournament)
        {
            try
            {
                await _dbContext.AddAsync(tournament);

                return tournament;
            }
            catch (Exception)
            {

                throw new Exception("Error: Failed to register item");
            }
        }
        public async Task<Tournament> OnLoadItemAsync(Guid TournamentId)
        {
            try
            {
                return await _dbContext.Tournaments.FirstOrDefaultAsync(m => m.TournamentId == TournamentId);
            }
            catch (Exception)
            {

                throw new Exception("Error: Unable to load item");
            }
        }
        public async Task<List<Tournament>> OnLoadItemsAsync()
        {
            try
            {
                var tournaments = await _dbContext.Tournaments.ToListAsync();

                var getActiveTournaments = from n in tournaments

                                           where n.IsActive == true

                                      select n;

                return getActiveTournaments.ToList();

            }
            catch (Exception)
            {

                throw new Exception("Error: Items could not be loaded!");
            }
        }
        public async Task<Tournament> OnModifyItemAsync(Tournament tournament)
        {
            Tournament results = new Tournament();

            try
            {
                results = await _dbContext.Tournaments.FirstOrDefaultAsync(x => x.TournamentId == tournament.TournamentId);

                if(results != null)
                {
                    results.AffiliationFee = tournament.AffiliationFee;

                    results.CutOffDate = tournament.CutOffDate;

                    results.Cycle = tournament.Cycle;

                    results.From = tournament.From;

                    results.HasPrice = tournament.HasPrice;

                    results.Price1 = tournament.Price1;

                    results.Price2 = tournament.Price2;

                    results.Till = tournament.Till;

                    results.HasAffiliationFee = tournament.HasAffiliationFee;

                    results.HasSponsor = tournament.HasSponsor;

                    results.IsActive = tournament.IsActive;

                    results.TournamentName = tournament.TournamentName;

                    results.TournamentStatus = tournament.TournamentStatus;

                    results.Type = tournament.Type;

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
            Tournament item = new Tournament();

            int record = 0;

            try
            {
                 item = await _dbContext.Tournaments.FirstOrDefaultAsync(m => m.TournamentId == Id);

                if (item != null)
                {
                    _dbContext.Remove(item);

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
