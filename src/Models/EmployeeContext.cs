using Microsoft.EntityFrameworkCore;

namespace MD.Salary.ConsoleApp.Models
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(Constants.ConnectionStringConsole);
        }
        public DbSet<EmployeeDB> Employees { get; set; }
    }
}