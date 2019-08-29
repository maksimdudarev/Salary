using System;
using System.Collections.Generic;
using MD.Salary.WebApi.Application;
using MD.Salary.WebApi.Models;

namespace MD.Salary.ConsoleApp.Application
{
    public static class Output
    {
        public static void WriteSalary(List<EmployeeFull> employees, WebApiProgram program, DateTime salaryDate)
        {
            foreach (var employee in employees) WriteEmployeeSalary(employee, program);
            Console.WriteLine($"\nTotal = {program.GetSalaryTotal()} Date = {salaryDate}");
            Console.ReadLine();
        }
        private static void WriteEmployeeSalary(EmployeeFull employee, WebApiProgram program)
        {
            Console.WriteLine($"{employee.ID} {employee.Name} {employee.Group} " +
                $"{employee.HireDate.ToString("dd MMMM yyyy")} salary = {program.GetSalary(employee)}");
        }

    }
}