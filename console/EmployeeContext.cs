﻿using Microsoft.EntityFrameworkCore;
using MD.Salary.WebApi.Models;

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
        public DbSet<Employee> Employees { get; set; }
    }
}