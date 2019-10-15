using MD.Salary.WebApi.Core.Models;
using MD.Salary.WebApi.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace MD.Salary.WebApi.Demo
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new EmployeeContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<EmployeeContext>>()))
            {
                // Look for any items.
                if (context.Employees.Any())
                {
                    return;   // DB has been seeded
                }

                context.Employees.AddRange(
                    new Employee
                    {
                        Name = "Smith",
                        HireDate = 1115251200,
                        Group = "Employee",
                        SalaryBase = 15M,
                        SuperiorID = 2
                    },

                    new Employee
                    {
                        Name = "Gates",
                        HireDate = 1183766400,
                        Group = "Manager",
                        SalaryBase = 30M,
                        SuperiorID = 3
                    },

                    new Employee
                    {
                        Name = "Trump",
                        HireDate = 1386720000,
                        Group = "Salesman",
                        SalaryBase = 35M,
                        SuperiorID = 5
                    },

                    new Employee
                    {
                        Name = "Parker",
                        HireDate = 1018224000,
                        Group = "Employee",
                        SalaryBase = 20M,
                        SuperiorID = 3
                    },

                    new Employee
                    {
                        Name = "Morgan",
                        HireDate = 1444867200,
                        Group = "Manager",
                        SalaryBase = 40M,
                        SuperiorID = 0
                    },

                    new Employee
                    {
                        Name = "Hughes",
                        HireDate = 168652800,
                        Group = "Salesman",
                        SalaryBase = 25M,
                        SuperiorID = 2
                    },

                    new Employee
                    {
                        Name = "McFly",
                        HireDate = 1359590400,
                        Group = "Employee",
                        SalaryBase = 10M,
                        SuperiorID = 6
                    },

                    new Employee
                    {
                        Name = "Willis",
                        HireDate = 978307200,
                        Group = "Manager",
                        SalaryBase = 45M,
                        SuperiorID = 3
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
