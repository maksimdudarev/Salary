using Microsoft.EntityFrameworkCore;

namespace MD.Salary.ConsoleApp.Models
{
    public class EmployeeFullContext : DbContext
    {
        public EmployeeFullContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(Constants.ConnectionStringConsole);
        }
        public DbSet<Employee> Employees { get; set; }
    }
}