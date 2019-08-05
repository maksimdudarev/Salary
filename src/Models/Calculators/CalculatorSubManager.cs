using System;
using System.Collections.Generic;

namespace MD.Salary.ConsoleApp.Models
{
    class CalculatorSubManager : Calculator, ICalculatorSub
    {
        public CalculatorSubManager(ConstantsEmployee rates) : base()
        {
            SubordinateRate = rates.SubordinateRate / Constants.PercentRate;
        }
        public decimal SubordinateRate { get; set; }
        public decimal GetSalary(List<EmployeeFull> subList, DateTime salaryDate)
        {
            return SubordinateRate * GetSalaryDirect(subList, salaryDate);
        }
    }

}

