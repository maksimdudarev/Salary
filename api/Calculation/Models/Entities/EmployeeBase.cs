using System;
using MD.Salary.WebApi.Utilities;
using MD.Salary.WebApi.Core.Models;

namespace MD.Salary.WebApi.Models
{
    public abstract class EmployeeBase
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public DateTime HireDate { get; set; }
        public Group Group { get; set; }
        public decimal SalaryBase { get; set; }
        public long SuperiorID { get; set; }
        public CalculatorPersonal SalaryPersonal { get; set; }
        public EmployeeBase(Employee employeeDB)
        {
            ID = employeeDB.UserId;
            Name = employeeDB.Name;
            HireDate = DateTimeOffset.FromUnixTimeSeconds(employeeDB.HireDate).UtcDateTime;
            Enum.TryParse(employeeDB.Group, out Group group);
            Group = group;
            SalaryBase = employeeDB.SalaryBase;
            SuperiorID = employeeDB.SuperiorID;
            var calculator = new Factory().GetCalculator(Group);
            SalaryPersonal = calculator.Personal;
        }
        public decimal GetSalaryPersonal(DateTime salaryDate)
        {
            decimal salary = SalaryPersonal.GetSalary(SalaryBase, HireDate, salaryDate);
            return salary;
        }
    }
}
