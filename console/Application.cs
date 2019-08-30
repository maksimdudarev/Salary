using System;
using System.Collections.Generic;
using MD.Salary.WebApi.Application;
using MD.Salary.WebApi.Core.Models;
using MD.Salary.ConsoleApp.Database;

namespace MD.Salary.ConsoleApp.Application
{
    public static class Application
    {
        static void Main(string[] args)
        {
            DateTime salaryDate = Input.ReadDate();
            List<Employee> items = DatabaseAccess.ReadEmployees();
            var program = new WebApiProgram(items, ((DateTimeOffset)salaryDate).ToUnixTimeSeconds());
            Output.WriteSalary(program.Employees, salaryDate);
        }
    }
}