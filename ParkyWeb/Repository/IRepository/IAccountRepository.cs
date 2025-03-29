using ParkyWeb.Models;

namespace ParkyWeb.Repository.IRepository
{
    public interface IAccountRepository : IRepository<User>
    {

        Task<User> LoginAsync(string url,User objectToCreate);
        Task<bool> RegisterAsync(string url,User objectToCreate);



    }
}
