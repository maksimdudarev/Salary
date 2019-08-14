using System.Collections.Generic;
using System.Threading.Tasks;
using MD.Salary.WebApi.Models;

namespace MD.Salary.WebApi.Core.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetByIdAsync(int id);
        Task<List<Employee>> ListAsync();
        Task AddAsync(Employee item);
        Task UpdateAsync(Employee item);
    }
}
