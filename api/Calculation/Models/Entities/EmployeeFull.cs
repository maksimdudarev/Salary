using System;
using System.Collections.Generic;
using System.Linq;
using MD.Salary.WebApi.Utilities;
using MD.Salary.WebApi.Core.Models;

namespace MD.Salary.WebApi.Models
{
    public class EmployeeFull
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public DateTime HireDate { get; set; }
        public decimal SalaryBase { get; set; }
        public long SuperiorID { get; set; }
        public List<EmployeeFull> SubordinateList { get; set; }
        public ICalculatorSub SalarySub { get; set; }
        public CalculatorPersonal SalaryPersonal { get; set; }
        public EmployeeFull(Employee employeeDB)
        {
            ID = employeeDB.UserId;
            Name = employeeDB.Name;
            HireDate = DateTimeOffset.FromUnixTimeSeconds(employeeDB.HireDate).UtcDateTime;
            SalaryBase = employeeDB.SalaryBase;
            SuperiorID = employeeDB.SuperiorID;
            var calculator = new Factory(employeeDB).GetCalculator();
            SalaryPersonal = calculator.Personal;
            SalarySub = calculator.Sub;
        }
        public void CalculateSubordinate(List<EmployeeFull> employeeList)
        {
            List<long> subordinateID = employeeList.Where(emp => emp.SuperiorID == ID).Select(emp => emp.ID).ToList();
            SubordinateList = employeeList.Where(emp => subordinateID.Contains(emp.ID)).ToList();
        }
        public virtual decimal GetSalary(DateTime salaryDate, MemoizationCache salaryCache)
        {
            decimal salary = GetSalaryPersonal(salaryDate) + GetSalarySub(salaryDate, salaryCache);
            salaryCache.Add(ID, salary);
            return salary;
        }
        public decimal GetSalaryPersonal(DateTime salaryDate)
        {
            decimal salary = SalaryPersonal.GetSalary(SalaryBase, HireDate, salaryDate);
            return salary;
        }
        public virtual decimal GetSalarySub(DateTime salaryDate, MemoizationCache salaryCache)
        {
            decimal salary = SalarySub.GetSalary(SubordinateList, salaryDate, salaryCache);
            return salary;
        }
    }
}
