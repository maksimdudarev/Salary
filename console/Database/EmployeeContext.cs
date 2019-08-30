using Microsoft.EntityFrameworkCore;
using MD.Salary.WebApi.Models;
using MD.Salary.WebApi.Core.Models;

namespace MD.Salary.ConsoleApp.Database
{
    public class EmployeeContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(Constants.ConnectionStringConsole);
        }
        public DbSet<Employee> Employees { get; set; }
    }
}