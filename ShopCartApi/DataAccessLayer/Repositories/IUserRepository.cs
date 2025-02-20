using ShopCartApi.Models;

namespace ShopCartApi.DataAccessLayer.Repositories
{
    public interface IUserRepository
    {

        void Add<T>(T entityToAdd);

        void Remove<T>(T entityToRemove);
        IEnumerable<User> getUsers();
        User? getSingleUser(int id);

        bool saveChanges();



    }
}
