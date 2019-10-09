using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MD.Salary.WebApi.Core.Interfaces;
using MD.Salary.WebApi.Core.Models;

namespace MD.Salary.WebApi.Infrastructure
{
    public class EFUserRepository : IUserRepository
    {
        private readonly EmployeeContext _dbContext;

        public EFUserRepository(EmployeeContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<User> GetUserByIdAsync(long id)
        {
            return _dbContext.Users.FirstOrDefaultAsync(s => s.ID == id);
        }

        public Task<User> GetUserByNameAsync(string name)
        {
            return _dbContext.Users.FirstOrDefaultAsync(s => s.Name == name);
        }

        public Task<List<User>> UserListAsync()
        {
            return _dbContext.Users.ToListAsync();
        }

        public Task AddUserAsync(User item)
        {
            _dbContext.Users.Add(item);
            return _dbContext.SaveChangesAsync();
        }

        public Task<Token> GetTokenByValueAsync(string value)
        {
            return _dbContext.Tokens.FirstOrDefaultAsync(s => s.Value == value);
        }

        public Task<List<Token>> TokenListAsync()
        {
            return _dbContext.Tokens.ToListAsync();
        }

        public Task AddTokenAsync(Token item)
        {
            _dbContext.Tokens.Add(item);
            return _dbContext.SaveChangesAsync();
        }

        public Task DeleteTokenAsync(Token item)
        {
            _dbContext.Tokens.Remove(item);
            return _dbContext.SaveChangesAsync();
        }

        public bool CheckValidUserKey(string reqkey)
        {
            var userkeyList = new List<string>
            {
                "28236d8ec201df516d0f6472d516d72d",
                "38236d8ec201df516d0f6472d516d72c",
                "48236d8ec201df516d0f6472d516d72b"
            };

            if (userkeyList.Contains(reqkey))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
