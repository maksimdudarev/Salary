using System;
using System.Collections.Generic;
using MD.Salary.WebApi.Application;
using MD.Salary.WebApi.Core.Models;
using MD.Salary.WebApi.Models;

namespace MD.Salary.ConsoleApp.Application
{
    public static class ConsoleAppProgram
    {
        static void Main(string[] args)
        {
            var program = new WebApiProgram();
            List<EmployeeFull> employeeList;
            using (var db = new Models.EmployeeContext())
            {
                var employeeListDB = new List<Employee>();
                foreach (var employeeDB in db.Employees) employeeListDB.Add(employeeDB);
                employeeList = program.GetEmployeeListFromDB(employeeListDB);
            }
            DateTime salaryDate = InputDate();
            employeeList = program.CalculateSalary(employeeList, salaryDate);
            foreach (var employee in employeeList) WriteSalary(employee, program);
            Console.WriteLine($"\nTotal = {program.GetSalaryTotal()} Date = {salaryDate}");
            Console.ReadLine();
        }
        public static DateTime InputDate()
        {
            string inputDate = "17/1/7"; // timestamp = 1168981200
            Console.WriteLine($"Input date (for ex., {inputDate}): ");
            inputDate = Console.ReadLine();
            if (!DateTime.TryParse(inputDate, out DateTime salaryDate)) salaryDate = DateTime.Today;
            return salaryDate;
        }
        public static void WriteSalary(EmployeeFull employee, WebApiProgram program)
        {
            Console.WriteLine($"{employee.ID} {employee.Name} {employee.Group} " +
                $"{employee.HireDate.ToString("dd MMMM yyyy")} salary = {program.GetSalary(employee)}");
        }
    }
}
