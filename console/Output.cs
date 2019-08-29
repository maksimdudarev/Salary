using System;
using MD.Salary.WebApi.Application;
using MD.Salary.WebApi.Models;

namespace MD.Salary.ConsoleApp.Application
{
    public static class Output
    {
        public static void WriteSalary(Employees employees, DateTime salaryDate)
        {
            foreach (var employee in employees.Items) WriteEmployeeSalary(employees, employee);
            Console.WriteLine($"\nTotal = {employees.GetSalaryTotal()} Date = {salaryDate}");
            Console.ReadLine();
        }
        private static void WriteEmployeeSalary(Employees employees, EmployeeFull employee)
        {
            Console.WriteLine($"{employee.ID} {employee.Name} {employee.Group} " +
                $"{employee.HireDate.ToString("dd MMMM yyyy")} salary = {employees.GetSalaryEmployee(employee.ID)}");
        }

    }
}