using System;
using System.Collections.Generic;
using System.Linq;
using MD.Salary.WebApi.Application;

namespace MD.Salary.WebApi.Models
{
    public abstract class Calculator
    {
        public decimal GetSalaryDirect(List<EmployeeFull> subList, DateTime salaryDate)
        {
            return subList.Sum(emp => WebApiProgram.SalaryCache.ContainsKey(emp.ID) ?
                                      WebApiProgram.SalaryCache.GetValue(emp.ID) : emp.GetSalary(salaryDate));
        }
    }
}
