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
                if (context.Employees.Any() & context.Users.Any() & context.Tokens.Any())
                {
                    return;   // DB has been seeded
                }

                context.Tokens.AddRange(
                    new Token
                    { 
                        UserID = 1,
                        Value = "ZIarF0ni+9lUOdL1AudRYMxR58cqJzxEcgbdJpzzWbg="
                    }
                );

                context.Users.AddRange(
                    new User
                    {
                        Name = "superuser@gmail.com",
                        Password = "$7$F6....0....KoO3uekRvftkTrewf8WQlUAQcuoKj8lhJOC5ejmhXq8$U5DbQolT7i2XBQGdooj9yzIpkA3XbcuMB/2Jis5Xvx5"
                        //superuser
                    },

                    new User
                    {
                        Name = "johnsmith@gmail.com",
                        Password = "$7$F6....0....PNA7h/KEfA6m.AxZWyxruDiwd0Go8k1SwM4di8qmUZC$DzpaqAfFIobjBnRh65jImotb.Fwcm.i9xucLnmC9oe."
                        //johnsmith0
                    },

                    new User
                    {
                        Name = "billgates@gmail.com",
                        Password = "$7$F6....0....vImk8.oLVGxPKipe7k6HBIo9raTbbgrXO.o6ix4fQH7$vpN/2YU8CKCe39aHSk5PqUWKA0cKQLZR8Sl0n1SWPKB"
                        //billgates1
                    },

                    new User
                    {
                        Name = "donaldtrump@gmail.com",
                        Password = "$7$F6....0....C40wZ7UAIhDofJi05U396RkXEWZ8/ngXjqEC267Rjw8$AT631EExMySUosY2ov3iT2Azc/LENMF964oXrdeGca5"
                        //donaldtrump12345
                    }
                );

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
