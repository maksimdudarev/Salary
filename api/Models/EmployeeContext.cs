﻿using MD.Salary.ConsoleApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MD.Salary.WebApi.Models
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options)
            : base(options)
        {
        }

        public DbSet<EmployeeDB> Employees { get; set; }
    }
}