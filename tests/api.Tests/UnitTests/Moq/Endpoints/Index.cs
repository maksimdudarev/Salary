﻿using MD.Salary.WebApi.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;
using static MD.Salary.WebApi.Tests.UnitTests.Asyncs;

namespace MD.Salary.WebApi.Tests.UnitTests.Moq
{
    public class Index : EmployeesControllerTests
    {
        public List<Employee> GetTestEmployees()
        {
            var items = new List<Employee>
            {
                new Employee() { UserId =  1001, Name = "Orange Juice", Group="Orange Tree", SalaryBase = 5.00M },
                new Employee() { UserId =  1002, Name = "Diary Milk", Group="Cow", SalaryBase = 4.00M },
                new Employee() { UserId =  1003, Name = "Frozen Pizza", Group="Uncle Mickey", SalaryBase = 12.00M }
            };
            return items;
        }

        public Index()
        {
            _repository.Setup(repo => repo.EmployeeListBySearhstringAsync("")).ReturnsAsync(GetTestEmployees());
        }

        [Fact]
        public void ReturnsOkObjectResult()
        {
            // Act
            var actResult = GetAsyncActionResult(() => _controller.Index(""));

            // Assert
            Assert.IsType<OkObjectResult>(actResult);
        }
    }
}
