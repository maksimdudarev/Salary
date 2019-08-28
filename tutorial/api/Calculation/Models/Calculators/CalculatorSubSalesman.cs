using System;
using System.Collections.Generic;
using System.Linq;
using MD.Salary.WebApi.Utilities;

namespace MD.Salary.WebApi.Models
{
    class CalculatorSubSalesman : Calculator, ICalculatorSub
    {
        public CalculatorSubSalesman(ConstantsEmployee rates) : base()
        {
            SubordinateRate = rates.SubordinateRate / Constants.PercentRate;
        }
        public decimal SubordinateRate { get; set; }
        public decimal GetSalary(List<EmployeeFull> subList, DateTime salaryDate, MemoizationCache salaryCache)
        {
            return SubordinateRate * (GetSalaryDirect(subList, salaryDate, salaryCache) + GetSalaryIndirect(subList, salaryDate, salaryCache));
        }
        private decimal GetSalaryIndirect(List<EmployeeFull> subList, DateTime salaryDate, MemoizationCache salaryCache)
        {
            return subList.Sum(emp => GetSalaryDirect(emp.SubordinateList, salaryDate, salaryCache));
        }
    }

}

