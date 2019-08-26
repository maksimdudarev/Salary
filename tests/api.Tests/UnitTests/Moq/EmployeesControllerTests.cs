using Moq;
using MD.Salary.WebApi.Core.Interfaces;

namespace MD.Salary.WebApi.Tests.UnitTests.Moq
{
    public class EmployeesControllerTests
    {
        public Mock<IEmployeeRepository> _repository;

        public EmployeesControllerTests()
        {
            _repository = new Mock<IEmployeeRepository>();
        }
    }
}
