using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MD.Salary.WebApi.Core.Interfaces;
using MD.Salary.WebApi.Core.Models;

namespace MD.Salary.WebApi.Infrastructure.EFRepositories
{
    public class EFEmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeContext _dbContext;

        public EFEmployeeRepository(EmployeeContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Employee> GetByIdAsync(long id)
        {
            return _dbContext.Employees.FirstOrDefaultAsync(s => s.ID == id);
        }

        public Task<List<Employee>> ListBySearhstringAsync(string searchString)
        {
            var items = from i in _dbContext.Employees select i;
            if (!string.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.Name.Contains(searchString));
            }
            return items.ToListAsync();
        }

        public Task AddAsync(Employee item)
        {
            _dbContext.Employees.Add(item);
            return _dbContext.SaveChangesAsync();
        }

        public Task UpdateAsync(Employee item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
            return _dbContext.SaveChangesAsync();
        }

        public Task DeleteAsync(Employee item)
        {
            _dbContext.Employees.Remove(item);
            return _dbContext.SaveChangesAsync();
        }
    }
}
