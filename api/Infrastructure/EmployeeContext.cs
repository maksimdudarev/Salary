using Microsoft.EntityFrameworkCore;
using MD.Salary.WebApi.Core.Models;
using ContactsApi.Models;

namespace MD.Salary.WebApi.Infrastructure
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Contact> Contacts { get; set; }
    }
}