using Microsoft.EntityFrameworkCore;
using SGMS.Contract;
using SGMS.Data;
using SGMS.Models;
using System.Linq.Expressions;

namespace SGMS.Repository
{
    public class PlayerRepository : IUnitOfWork<Player>
    {
        private readonly ApplicationDbContext _dbContext;
        public PlayerRepository(ApplicationDbContext dbContext)
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
        public async Task<Player> OnItemCreationAsync(Player player)
        {
            try
            {
                await _dbContext.AddAsync(player);

                return player;
            }
            catch (Exception)
            {

                throw new Exception("Error: Failed to register player");
            }
        }
        public async Task<Player> OnLoadItemAsync(Guid PlayerId)
        {
            try
            {
                return await _dbContext.Players.FirstOrDefaultAsync(m => m.PlayerId == PlayerId);
            }
            catch (Exception)
            {

                throw new Exception("Error: Unable to load player");
            }
        }
        public async Task<List<Player>> OnLoadItemsAsync()
        {
            try
            {
                var players = await _dbContext.Players.ToListAsync();

                var getActivePlayers = from n in players

                                       where n.IsActive == true

                                       select n;

                return getActivePlayers.ToList();

            }
            catch (Exception)
            {

                throw new Exception("Error: Players could not be loaded!");
            }
        }
        public async Task<Player> OnModifyItemAsync(Player player)
        {
            Player results = new Player();

            try
            {
                results = await _dbContext.Players.FirstOrDefaultAsync(x => x.PlayerId == player.PlayerId);

                if (results != null)
                {
                    results.DOB = player.DOB;

                    results.IDNumber = player.IDNumber;

                    results.TeamId = player.TeamId;

                    results.JerseyNumber = player.JerseyNumber;

                    results.IsActive = player.IsActive;

                    results.PlayerLastName = player.PlayerLastName;

                    results.RSAIDCopy = player.RSAIDCopy;

                    results.Position = player.Position;

                    results.Photo = player.Photo;

                    results.Phone = player.Phone;

                    results.Email = player.Email;

                    results.AlternativePhone = player.AlternativePhone;

                    results.StreetName = player.StreetName;

                    results.City = player.City;

                    results.Province = player.Province;

                    results.PostalCode = player.PostalCode;

                    _dbContext.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw new Exception("Error: Player Failed to Modify!");
            }

            return results;
        }
        public async Task<int> OnRemoveItemAsync(Guid PlayerId)
        {
            Player player = new Player();

            int record = 0;

            try
            {
                player = await _dbContext.Players.FirstOrDefaultAsync(m => m.PlayerId == PlayerId);

                if (player != null)
                {
                    _dbContext.Remove(player);

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
