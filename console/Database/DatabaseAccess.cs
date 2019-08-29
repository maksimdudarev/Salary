using System.Collections.Generic;
using MD.Salary.WebApi.Core.Models;

namespace MD.Salary.ConsoleApp.Database
{
    public static class DatabaseAccess
    {
        public static List<Employee> ReadEmployees()
        {
            var employees = new List<Employee>();
            using (var db = new EmployeeContext()) foreach (var employee in db.Employees) employees.Add(employee);
            return employees;
        }
    }
}