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
        public Group Group { get; set; }
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
            Enum.TryParse(employeeDB.Group, out Group group);
            Group = group;
            SalaryBase = employeeDB.SalaryBase;
            SuperiorID = employeeDB.SuperiorID;
            var calculator = new Factory().GetCalculator(Group);
            SalaryPersonal = calculator.Personal;
            SalarySub = calculator.Sub;
        }
        public void CalculateSubordinate(List<EmployeeFull> employeeList)
        {
            List<long> subordinateID = employeeList.Where(emp => emp.SuperiorID == ID).Select(emp => emp.ID).ToList();
            SubordinateList = employeeList.Where(emp => subordinateID.Contains(emp.ID)).ToList();
        }
        public decimal GetSalary(DateTime salaryDate, MemoizationCache salaryCache)
        {
            decimal salary = SalaryPersonal.GetSalary(SalaryBase, HireDate, salaryDate) + SalarySub.GetSalary(SubordinateList, salaryDate, salaryCache);
            salaryCache.Add(ID, salary);
            return salary;
        }
    }
}
