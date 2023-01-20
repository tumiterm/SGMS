using Microsoft.EntityFrameworkCore;
using SGMS.Contract;
using SGMS.Data;
using SGMS.Models;
using System.Linq.Expressions;

namespace SGMS.Repository
{
    public class UserRepository : IUnitOfWork<User>
    {
        private readonly ApplicationDbContext _dbContext;
        public UserRepository(ApplicationDbContext dbContext)
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
        public async Task<User> OnItemCreationAsync(User user)
        {
            try
            {
                await _dbContext.AddAsync(user);

                return user;
            }
            catch (Exception)
            {

                throw new Exception("Error: Failed to register user");
            }
        }
        public async Task<User> OnLoadItemAsync(Guid Id)
        {
            try
            {
                var user = await _dbContext.Users.Where(m => m.Id == Id).FirstOrDefaultAsync();

                if (user is null)
                {
                    throw new Exception("Error!");
                }

                return user;

            }
            catch (Exception)
            {

                throw new Exception("Error: Unable to load user");
            }
        }
        public async Task<List<User>> OnLoadItemsAsync()
        {
            try
            {
                var users = await _dbContext.Users.ToListAsync();

                var getActiveUsers = from n in users

                                           select n;

                return getActiveUsers.ToList();

            }
            catch (Exception)
            {

                throw new Exception("Error: Users could not be loaded!");
            }
        }
        public async Task<User> OnModifyItemAsync(User user)
        {
            User results = new User();

            try
            {
                results = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == user.Id);

                if (results != null)
                {

                    results.Name = user.Name;

                    results.LastName = user.LastName;

                    results.IsActive = user.IsActive;

                    results.IsEmailVerified = user.IsEmailVerified;

                    results.LastLoginDate = user.LastLoginDate;

                    results.Password = user.Password;

                    results.Role = user.Role;

                    results.Username = user.Username;

                    _dbContext.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw new Exception("Error: User Failed to Modify!");
            }

            return results;
        }
        public async Task<int> OnRemoveItemAsync(Guid Id)
        {
            User user = new User();

            int record = 0;

            try
            {
                user = await _dbContext.Users.FirstOrDefaultAsync(m => m.Id == Id);

                if (user != null)
                {
                    _dbContext.Remove(user);

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
