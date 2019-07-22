using Microsoft.EntityFrameworkCore;
using MD.Salary.ConsoleApp.Models;

namespace MD.Salary.WebApi.Models
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options)
            : base(options)
        {
        }

        public DbSet<EmployeeDB> EmployeeItems { get; set; }
    }
}