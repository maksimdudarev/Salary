using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using static MD.Salary.WebApi.Tests.UnitTests.Asyncs;
using static MD.Salary.WebApi.Tests.UnitTests.Constants;

namespace MD.Salary.WebApi.Tests.UnitTests.Moq
{
    public class IndexSalary : EmployeesControllerTests
    {
        public IndexSalary()
        {
            _repository.Setup(repo => repo.ListBySearhstringAsync("")).ReturnsAsync(GetTestEmployees());
        }

        [Fact]
        public void ReturnsOkObjectResult()
        {
            // Act
            var actResult = GetAsyncActionResult(() => _controller.IndexSalary(SalaryDate));

            // Assert
            Assert.IsType<OkObjectResult>(actResult);
        }
    }
}
