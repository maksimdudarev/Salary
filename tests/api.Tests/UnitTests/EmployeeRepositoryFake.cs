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

        public async Task<List<Employee>> ListAsync()
        {
            await Task.Delay(1000);
            return _employees;
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

        public Employee Add(Employee newItem)
        {
            //newItem.ID = Guid.NewGuid();
            _employees.Add(newItem);
            return newItem;
        }

        public Task AddAsync(Employee item)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Employee item)
        {
            throw new NotImplementedException();
        }

        public void Remove(long id)
        {
            var existing = _employees.First(s => s.ID == id);
            _employees.Remove(existing);
        }

        public Task DeleteAsync(Employee item)
        {
            throw new NotImplementedException();
        }
    }
}
