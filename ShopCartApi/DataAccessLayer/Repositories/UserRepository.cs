using ShopCartApi.DataContext;
using ShopCartApi.Models;

namespace ShopCartApi.DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private DataContextEF _dataContextEF;
        public UserRepository(IConfiguration config)
        {
            _dataContextEF = new DataContextEF(config);
        
        }
        public User?  getSingleUser(int id)
        {
            return _dataContextEF.Users?.Where(user => user.UserId == id).FirstOrDefault();
        }

        public IEnumerable<User> getUsers()
        {
            return _dataContextEF.Users == null?[]: _dataContextEF.Users.ToList();
        }

        public bool saveChanges()
        {
            return _dataContextEF.SaveChanges() > 0;
        }

        public void Add<T>(T entityToAdd)
        {
            if(entityToAdd != null)
            {
                _dataContextEF.Add(entityToAdd);

            }
        }

        public void Remove<T>(T entityToRemove)
        {
            if (entityToRemove != null)
            {
                _dataContextEF.Remove(entityToRemove);

            }
        }
    }
}
