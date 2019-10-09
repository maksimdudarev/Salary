using System.Collections.Generic;
using System.Threading.Tasks;
using MD.Salary.WebApi.Core.Models;

namespace MD.Salary.WebApi.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(long id);
        Task<User> GetUserByNameAsync(string name);
        Task<List<User>> UserListAsync();
        Task AddUserAsync(User item);
        Task<Token> GetTokenByValueAsync(string value);
        Task<List<Token>> TokenListAsync();
        Task AddTokenAsync(Token item);
        Task DeleteTokenAsync(Token item);
        bool CheckValidUserKey(string reqkey);
    }
}
