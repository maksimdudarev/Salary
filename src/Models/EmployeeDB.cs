﻿using System.ComponentModel.DataAnnotations.Schema;

namespace MD.Salary.ConsoleApp.Models
{
    public class EmployeeDB
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public long HireDate { get; set; }
        public string Group { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal SalaryBase { get; set; }
        public long SuperiorID { get; set; }
    }
}
