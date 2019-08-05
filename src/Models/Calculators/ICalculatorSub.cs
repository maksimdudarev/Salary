using System;
using System.Collections.Generic;

namespace MD.Salary.ConsoleApp.Models
{
    public interface ICalculatorSub
    {
        decimal SubordinateRate { get; set; }
        decimal GetSalary(List<EmployeeFull> subList, DateTime salaryDate);
    }
}

