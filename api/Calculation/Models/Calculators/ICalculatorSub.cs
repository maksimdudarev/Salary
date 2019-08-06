using System;
using System.Collections.Generic;
using MD.Salary.WebApi.Utilities;

namespace MD.Salary.WebApi.Models
{
    public interface ICalculatorSub
    {
        decimal SubordinateRate { get; set; }
        decimal GetSalary(List<EmployeeFull> subList, DateTime salaryDate, MemoizationCache salaryCache);
    }
}

