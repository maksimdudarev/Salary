using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using static MD.Salary.WebApi.Tests.UnitTests.Asyncs;
using static MD.Salary.WebApi.Tests.UnitTests.Constants;

namespace MD.Salary.WebApi.Tests.UnitTests.Moq
{
    public class GetEmployeeSalary : EmployeesControllerTests
    {
        public GetEmployeeSalary()
        {
            _repository.Setup(repo => repo.ListBySearhstringAsync("")).ReturnsAsync(GetTestEmployees());
        }


        [Fact]
        public void ReturnsNotFoundResult()
        {
            // Act
            var actResult = GetAsyncActionResult(() => _controller.GetEmployeeSalary(NotExistingId, SalaryDate));

            // Assert
            Assert.IsType<NotFoundResult>(actResult);
        }

        [Fact]
        public void ReturnsOkObjectResult()
        {
            // Act
            var actResult = GetAsyncActionResult(() => _controller.GetEmployeeSalary(ExistingId, SalaryDate));

            // Assert
            Assert.IsType<OkObjectResult>(actResult);
        }
    }
}
