using System;
using System.Collections.Generic;
using System.Linq;

namespace MD.Salary.ConsoleApp.Models
{
    public interface ICalculatorSub
    {
        decimal GetSalary(List<Employee> subList, DateTime salaryDate);
    }
    class ICalculatorSubEmployee : Calculator, ICalculatorSub
    {
        public ICalculatorSubEmployee(decimal subordinateRate) : base(subordinateRate) { }
        public decimal GetSalary(List<Employee> subList, DateTime salaryDate)
        {
            return 0;
        }
    }
    class ICalculatorSubManager : Calculator, ICalculatorSub
    {
        public ICalculatorSubManager(decimal subordinateRate) : base(subordinateRate) { }
        public decimal GetSalary(List<Employee> subList, DateTime salaryDate)
        {
            return SubordinateRate * GetSalaryDirect(subList, salaryDate);
        }
    }
    class ICalculatorSubSalesman : Calculator, ICalculatorSub
    {
        public ICalculatorSubSalesman(decimal subordinateRate) : base(subordinateRate) { }
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

