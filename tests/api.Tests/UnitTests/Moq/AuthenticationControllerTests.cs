using MD.Salary.WebApi.Controllers;
using MD.Salary.WebApi.Core.Interfaces;
using Moq;

namespace MD.Salary.WebApi.Tests.UnitTests.Moq
{
    public class AuthenticationControllerTests
    {
        public Mock<IEmployeeRepository> _repository;
        public AuthenticationController _controller;

        public AuthenticationControllerTests()
        {
            _repository = new Mock<IEmployeeRepository>();
            _controller = new AuthenticationController(_repository.Object);
        }
    }
}
