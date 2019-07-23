using System;
using System.Collections.Generic;
using System.Linq;

namespace MD.Salary.ConsoleApp.Models
{
    class CalculatorSubSalesman : Calculator, ICalculatorSub
    {
        public CalculatorSubSalesman(decimal subordinateRate) : base()
        {
            SubordinateRate = subordinateRate / Constants.PercentRate;
        }
        public decimal SubordinateRate { get; set; }
        public decimal GetSalary(List<Employee> subList, DateTime salaryDate)
        {
            return SubordinateRate * (GetSalaryDirect(subList, salaryDate) + GetSalaryIndirect(subList, salaryDate));
        }
        private decimal GetSalaryIndirect(List<Employee> subList, DateTime salaryDate)
        {
            return subList.Sum(emp => GetSalaryDirect(emp.SubordinateList, salaryDate));
        }
    }

}

