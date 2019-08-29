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
            var program = new WebApiProgram();
            List<Employee> employeesDB = DatabaseAccess.ReadEmployees();
            Employees employees = program.GetSalaryFromDB(employeesDB, salaryDate);
            Output.WriteSalary(employees, salaryDate);
        }
    }
}