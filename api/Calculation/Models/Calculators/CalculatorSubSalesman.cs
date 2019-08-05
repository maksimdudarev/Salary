using System;
using System.Collections.Generic;
using System.Linq;

namespace MD.Salary.WebApi.Models
{
    class CalculatorSubSalesman : Calculator, ICalculatorSub
    {
        public CalculatorSubSalesman(ConstantsEmployee rates) : base()
        {
            SubordinateRate = rates.SubordinateRate / Constants.PercentRate;
        }
        public decimal SubordinateRate { get; set; }
        public decimal GetSalary(List<EmployeeFull> subList, DateTime salaryDate)
        {
            return SubordinateRate * (GetSalaryDirect(subList, salaryDate) + GetSalaryIndirect(subList, salaryDate));
        }
        private decimal GetSalaryIndirect(List<EmployeeFull> subList, DateTime salaryDate)
        {
            return subList.Sum(emp => GetSalaryDirect(emp.SubordinateList, salaryDate));
        }
    }

}

