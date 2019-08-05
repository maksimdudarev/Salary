using Microsoft.EntityFrameworkCore;
using MD.Salary.ConsoleApp.Models;

namespace MD.Salary.WebMvc.Models
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options)
            : base(options)
        { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}
