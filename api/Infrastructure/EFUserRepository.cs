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

        public Task<User> GetByIdAsync(long id)
        {
            return _dbContext.Users.FirstOrDefaultAsync(s => s.ID == id);
        }

        public Task<User> GetByNameAsync(string name)
        {
            return _dbContext.Users.FirstOrDefaultAsync(s => s.Name == name);
        }

        public Task AddAsync(User item)
        {
            _dbContext.Users.Add(item);
            return _dbContext.SaveChangesAsync();
        }
    }
}
