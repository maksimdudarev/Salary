using Microsoft.EntityFrameworkCore;
using MD.Salary.WebApi.Core.Models;

namespace MD.Salary.WebApi.Infrastructure
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
    }
}