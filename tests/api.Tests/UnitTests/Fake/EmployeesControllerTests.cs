using MD.Salary.WebApi.Controllers;
using MD.Salary.WebApi.Core.Interfaces;
using MD.Salary.WebApi.Core.Models;
using System.Collections.Generic;

namespace MD.Salary.WebApi.Tests.UnitTests.Fake
{
    public class EmployeesControllerTests
    {
        public EmployeesController _controller;
        public IEmployeeRepository _service;

        public List<Employee> GetTestEmployees()
        {
            var items = new List<Employee>
            {
                new Employee() { ID = 1001, Name = "Orange Juice", Group="Orange Tree", SalaryBase = 5.00M },
                new Employee() { ID = 1002, Name = "Diary Milk", Group="Cow", SalaryBase = 4.00M },
                new Employee() { ID = 1003, Name = "Frozen Pizza", Group="Uncle Mickey", SalaryBase = 12.00M }
            };
            return items;
        }

        public EmployeesControllerTests()
        {
            _service = new EmployeeRepositoryFake(GetTestEmployees());
            _controller = new EmployeesController(_service);
        }
    }
}
