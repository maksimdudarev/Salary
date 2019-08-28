using System.Collections.Generic;
using System.Threading.Tasks;
using MD.Salary.WebApi.Core.Models;

namespace MD.Salary.WebApi.Core.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetByIdAsync(long id);
        Task<List<Employee>> ListBySearhstringAsync(string searchString = "");
        Task AddAsync(Employee item);
        Task UpdateAsync(Employee item);
        Task DeleteAsync(Employee item);
    }
}
