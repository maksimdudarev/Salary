using System;
using MD.Salary.ConsoleApp.Models;

namespace MD.Salary.WebApi.Models
{
    public class EmployeeItem
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public DateTime HireDate { get; set; }
        public Group Group { get; set; }
        public decimal SalaryBase { get; set; }
        public long SuperiorID { get; set; }
    }
}
