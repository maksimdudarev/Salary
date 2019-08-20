using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MD.Salary.WebApi.Core.Interfaces;
using MD.Salary.WebApi.Core.Models;

namespace MD.Salary.WebApi.Tests
{
    public class EmployeeRepositoryFake : IEmployeeRepository
    {
        private readonly List<Employee> _employees;

        public EmployeeRepositoryFake(List<Employee> employees)
        {
            _employees = employees;
        }

        public async Task<List<Employee>> ListBySearhstringAsync(string searchString)
        {
            var items = from i in _employees select i;
            if (!string.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.Name.Contains(searchString));
            }
            await Task.Delay(1000);
            return items.ToList();
        }

        public async Task<Employee> GetByIdAsync(long id)
        {
            await Task.Delay(1000);
            return _employees.FirstOrDefault(s => s.ID == id);
        }

        public async Task AddAsync(Employee item)
        {
            await Task.Delay(1000);
            item.ID = 5000;
            _employees.Add(item);
            return;
        }

        public Task UpdateAsync(Employee item)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Employee item)
        {
            await Task.Delay(1000);
            _employees.Remove(item);
            return;
        }

    }
}
