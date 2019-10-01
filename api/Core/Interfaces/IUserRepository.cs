using System.Threading.Tasks;
using MD.Salary.WebApi.Core.Models;

namespace MD.Salary.WebApi.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(long id);
        Task<User> GetByNameAsync(string name);
        Task AddAsync(User item);
    }
}
