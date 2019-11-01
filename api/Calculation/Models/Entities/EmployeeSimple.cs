using System;
using MD.Salary.WebApi.Core.Models;
using MD.Salary.WebApi.Utilities;

namespace MD.Salary.WebApi.Models
{
    public class EmployeeSimple : EmployeeFull
    {
        public EmployeeSimple(Employee employeeDB) : base(employeeDB)
        {
        }
        public override decimal GetSalary(DateTime salaryDate, MemoizationCache salaryCache)
        {
            decimal salary = GetSalaryPersonal(salaryDate);
            salaryCache.Add(ID, salary);
            return salary;
        }
    }
}
