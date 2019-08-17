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

        public EmployeeRepositoryFake()
        {
            _employees = new List<Employee>()
            {
                new Employee() { ID = 1001, Name = "Orange Juice", Group="Orange Tree", SalaryBase = 5.00M },
                new Employee() { ID = 1002, Name = "Diary Milk", Group="Cow", SalaryBase = 4.00M },
                new Employee() { ID = 1003, Name = "Frozen Pizza", Group="Uncle Mickey", SalaryBase = 12.00M }
            };
        }

        public List<Employee> GetAllItems()
        {
            return _employees;
        }

        public Employee Add(Employee newItem)
        {
            //newItem.ID = Guid.NewGuid();
            _employees.Add(newItem);
            return newItem;
        }

        public Employee GetById(long id)
        {
            return _employees.Where(a => a.ID == id)
                .FirstOrDefault();
        }

        public void Remove(long id)
        {
            var existing = _employees.First(a => a.ID == id);
            _employees.Remove(existing);
        }

        public Task<Employee> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Employee>> ListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Employee>> ListBySearhstringAsync(string searchString)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(Employee item)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Employee item)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Employee item)
        {
            throw new NotImplementedException();
        }
    }
}
