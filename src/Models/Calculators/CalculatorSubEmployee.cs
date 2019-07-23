using System;
using System.Collections.Generic;

namespace MD.Salary.ConsoleApp.Models
{
    class CalculatorSubEmployee : Calculator, ICalculatorSub
    {
        public CalculatorSubEmployee(decimal subordinateRate) : base()
        {
            SubordinateRate = subordinateRate / Constants.PercentRate;
        }
        public decimal SubordinateRate { get; set; }
        public decimal GetSalary(List<Employee> subList, DateTime salaryDate)
        {
            return 0;
        }
    }

}

