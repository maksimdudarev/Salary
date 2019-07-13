using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data.SQLite;
using MD.Salary.Model;

namespace MD.Salary.Database
{
    class DataRetriever
    {
        public static List<EmployeeDB> GetData(string dataSource, string tableName)
        {
            string connectionString = "Data Source = " + dataSource + "; Version = 3; New = True; Compress = True;";
            string commandText = "SELECT * FROM " + tableName;
            SQLiteConnection connection = CreateConnection(connectionString);
            SQLiteDataReader dataReader = ExecuteReader(connection, commandText);
            List<EmployeeDB> employeeList = GetEmployeeList(dataReader);
            CloseConnection(connection);
            return employeeList;
        }
        private static SQLiteConnection CreateConnection(string connectionString)
        {
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (Exception) { }
            return connection;
        }
        private static void CloseConnection(SQLiteConnection connection)
        {
            connection.Close();
        }
        private static SQLiteDataReader ExecuteReader(SQLiteConnection connection, string commandText)
        {
            SQLiteDataReader dataReader = null;
            SQLiteCommand command = CreateCommand(connection, commandText);
            try
            {
                dataReader = command.ExecuteReader();
            }
            catch (Exception) { } 
            return dataReader;
        }
        private static SQLiteCommand CreateCommand(SQLiteConnection connection, string commandText)
        {
            SQLiteCommand command;
            command = connection.CreateCommand();
            command.CommandText = commandText;
            return command;
        }
        private static List<EmployeeDB> GetEmployeeList(SQLiteDataReader dataReader)
        {
            var employeeList = new List<EmployeeDB> { };
            try
            {
                while (dataReader.Read())
                {
                    EmployeeDB employee = GetEmployee(dataReader);
                    employeeList.Add(employee);
                }
            }
            catch (Exception) { }
            return employeeList;
        }
        private static EmployeeDB GetEmployee(SQLiteDataReader dataReader)
        {
            var employee = new EmployeeDB();
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                object value = dataReader.GetValue(i);
                string propertyName = dataReader.GetName(i);
                value = TransformValue(propertyName, value);
                SetPropertyValue(employee, propertyName, value);
            }
            return employee;
        }
        private static object TransformValue(string propertyName, object value)
        {
            switch (propertyName)
            {
                case "group":
                    value = Enum.Parse(typeof(Group), value.ToString());
                    break;
                case "hiredate":
                    value = DateTimeOffset.FromUnixTimeSeconds((long)value).UtcDateTime;
                    break;
            }
            return value;
        }
        private static void SetPropertyValue(object src, string propertyName, object value)
        {
            var flags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
            src.GetType().GetProperty(propertyName, flags).SetValue(src, value);
        }
    }
}

