using System;
using System.Collections.Generic;
using MD.Salary.WebApi.Core.Models;
using MD.Salary.WebApi.Utilities;

namespace MD.Salary.WebApi.Models
{
    public class EmployeeManager : EmployeeFull
    {
        //public List<EmployeeFull> SubordinateList { get; set; }
        //public ICalculatorSub SalarySub { get; set; }
        public EmployeeManager(Employee employeeDB) : base(employeeDB)
        {
        }
        public override decimal GetSalary(DateTime salaryDate, MemoizationCache salaryCache)
        {
            decimal salary = GetSalaryPersonal(salaryDate) + GetSalarySub(salaryDate, salaryCache);
            salaryCache.Add(ID, salary);
            return salary;
        }
        public override decimal GetSalarySub(DateTime salaryDate, MemoizationCache salaryCache)
        {
            decimal salary = SalarySub.GetSalary(SubordinateList, salaryDate, salaryCache);
            return salary;
        }
    }
}
