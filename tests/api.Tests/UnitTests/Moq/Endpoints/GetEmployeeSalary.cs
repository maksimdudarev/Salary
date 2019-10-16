using MD.Salary.WebApi.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;
using static MD.Salary.WebApi.Tests.UnitTests.Asyncs;

namespace MD.Salary.WebApi.Tests.UnitTests.Moq
{
    public class GetEmployeeSalary : EmployeesControllerTests
    {
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

        public GetEmployeeSalary()
        {
            _repository.Setup(repo => repo.EmployeeListBySearhstringAsync("")).ReturnsAsync(GetTestEmployees());
        }

        [Fact]
        public void ReturnsNotFoundResult()
        {
            // Act
            var actResult = GetAsyncActionResult(() => _controller.GetEmployeeSalary(2000, 1564952400));

            // Assert
            Assert.IsType<NotFoundResult>(actResult);
        }

        [Fact]
        public void ReturnsOkObjectResult()
        {
            // Act
            var actResult = GetAsyncActionResult(() => _controller.GetEmployeeSalary(1001, 1564952400));

            // Assert
            Assert.IsType<OkObjectResult>(actResult);
        }
    }
}
