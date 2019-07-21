using Microsoft.EntityFrameworkCore;

namespace MD.Salary.ConsoleApp.Models
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext()
        {
        }
        public EmployeeContext(DbContextOptions<EmployeeContext> options)
            : base(options)
        {
        }
        public DbSet<EmployeeDB> Employees { get; set; }
    }
}