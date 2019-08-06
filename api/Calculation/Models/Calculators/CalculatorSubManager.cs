using System;
using System.Collections.Generic;
using MD.Salary.WebApi.Utilities;

namespace MD.Salary.WebApi.Models
{
    class CalculatorSubManager : Calculator, ICalculatorSub
    {
        public CalculatorSubManager(ConstantsEmployee rates) : base()
        {
            SubordinateRate = rates.SubordinateRate / Constants.PercentRate;
        }
        public decimal SubordinateRate { get; set; }
        public decimal GetSalary(List<EmployeeFull> subList, DateTime salaryDate, MemoizationCache salaryCache)
        {
            return SubordinateRate * GetSalaryDirect(subList, salaryDate, salaryCache);
        }
    }

}

