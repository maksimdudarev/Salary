using System;
using System.Collections.Generic;
using System.Linq;

namespace MD.Salary.ConsoleApp.Models
{
    public interface ICalculatorSub
    {
        decimal GetSalary(List<Employee> subList, DateTime salaryDate);
    }
    class CalculatorSubEmployee : Calculator, ICalculatorSub
    {
        public CalculatorSubEmployee(decimal subordinateRate) : base(subordinateRate) { }
        public decimal GetSalary(List<Employee> subList, DateTime salaryDate)
        {
            return 0;
        }
    }
    class CalculatorSubManager : Calculator, ICalculatorSub
    {
        public CalculatorSubManager(decimal subordinateRate) : base(subordinateRate) { }
        public decimal GetSalary(List<Employee> subList, DateTime salaryDate)
        {
            return SubordinateRate * GetSalaryDirect(subList, salaryDate);
        }
    }
    class CalculatorSubSalesman : Calculator, ICalculatorSub
    {
        public CalculatorSubSalesman(decimal subordinateRate) : base(subordinateRate) { }
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

