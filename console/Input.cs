using System;

namespace MD.Salary.ConsoleApp.Application
{
    public static class Input
    {
        public static DateTime ReadDate()
        {
            string inputDate = "17/1/7"; // timestamp = 1168981200
            Console.WriteLine($"Input date (for ex., {inputDate}): ");
            inputDate = Console.ReadLine();
            if (!DateTime.TryParse(inputDate, out DateTime salaryDate)) salaryDate = DateTime.Today;
            return salaryDate;
        }
    }
}