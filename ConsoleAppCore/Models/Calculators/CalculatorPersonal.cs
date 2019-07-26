using System;
using static System.Math;

namespace MD.Salary.ConsoleApp.Models
{
    public class CalculatorPersonal : Calculator
    {
        public CalculatorPersonal(ConstantsEmployee rates) : base()
        {
            ExperienceRate = rates.ExperienceRate / Constants.PercentRate;
            LimitRate = rates.LimitRate / Constants.PercentRate;
        }
        private decimal ExperienceRate { get; set; }
        private decimal LimitRate { get; set; }
        private int GetExperience(DateTime salaryDate, DateTime hireDate)
        {
            var exp = salaryDate.Year - hireDate.Year;
            if (hireDate.Date > salaryDate.AddYears(-exp)) exp--;
            return exp;
        }
        public decimal GetSalary(decimal salaryBase, DateTime hireDate, DateTime salaryDate)
        {
            return (Min(ExperienceRate * GetExperience(salaryDate, hireDate), LimitRate) + 1) * salaryBase;
        }
    }
}
