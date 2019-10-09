using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MD.Salary.WebApi.Core.Interfaces;
using MD.Salary.WebApi.Core.Models;

namespace MD.Salary.WebApi.Infrastructure.EFRepositories
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

        public bool CheckValidUserKey(string value)
        {
            var item = _dbContext.Tokens.FirstOrDefaultAsync(s => s.Value == value);
            if (item == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
