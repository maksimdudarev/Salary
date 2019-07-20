using System;
using System.Collections.Generic;
using System.Linq;
using MD.Salary.ConsoleApp.Application;

namespace MD.Salary.ConsoleApp.Models
{
    public abstract class Calculator
    {
        public decimal SubordinateRate { get; set; }
        public decimal UnitRate { get; set; } = 100;
        public Calculator() { }
        public Calculator(decimal subordinateRate)
        {
            SubordinateRate = subordinateRate / UnitRate;
        }
        public decimal GetSalaryDirect(List<Employee> subList, DateTime salaryDate)
        {
            return subList.Sum(emp => ConsoleAppProgram.SalaryCache.ContainsKey(emp.ID) ?
                                      ConsoleAppProgram.SalaryCache.GetValue(emp.ID) : emp.GetSalary(salaryDate));
        }
    }
}
