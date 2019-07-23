namespace MD.Salary.ConsoleApp.Models
{
    public enum Group { Employee, Manager, Salesman }
    public static class Constants
    {
        public static ConstantsEmployee Employee { get; } = new ConstantsEmployee(3, 30, 0);
        public static ConstantsEmployee Manager { get; } = new ConstantsEmployee(5, 40, 0.5m);
        public static ConstantsEmployee Salesman { get; } = new ConstantsEmployee(1, 35, 0.3m);
        public static decimal PercentRate { get; } = 100;
        public const string TypeNameDecimalColumn = "decimal(18, 2)";
        private static string SalaryDbPath { get; } = "..\\db\\Salary.db";
        public static string ConnectionStringConsole { get; } = GetConnectionString("..\\..\\" + SalaryDbPath);
        public static string ConnectionStringApi { get; } = GetConnectionString(SalaryDbPath);
        static string GetConnectionString(string path)
        {
            return "Data Source = " + path;
        }
    }
}
