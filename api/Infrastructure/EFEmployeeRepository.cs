using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MD.Salary.WebApi.Core.Interfaces;
using MD.Salary.WebApi.Models;

namespace MD.Salary.WebApi.Infrastructure
{
    public class EFEmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeContext _dbContext;

        public EFEmployeeRepository(EmployeeContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Employee> GetByIdAsync(int id)
        {
            return _dbContext.Employees
                //.Include(s => s.Ideas)
                .FirstOrDefaultAsync(s => s.ID == id);
        }

        public Task<List<Employee>> ListAsync()
        {
            return _dbContext.Employees
                //.Include(s => s.Ideas)
                .OrderByDescending(s => s.HireDate)
                .ToListAsync();
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
    }
}
