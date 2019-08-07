using System;
using System.Collections.Generic;
using System.Linq;
using MD.Salary.WebApi.Utilities;

namespace MD.Salary.WebApi.Models
{
    public abstract class Calculator
    {
        public decimal GetSalaryDirect(List<EmployeeFull> subList, DateTime salaryDate, MemoizationCache salaryCache)
        {
            return subList.Sum(emp => salaryCache.ContainsKey(emp.ID) ?
                                      salaryCache.GetValue(emp.ID) : emp.GetSalary(salaryDate, salaryCache));
        }
    }
}
