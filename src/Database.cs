using System;
using System.Data.SQLite;
using System.Reflection;
using MD.Salary.Model;

namespace MD.Salary.Database
{
    class DBEmployee
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public DateTime HireDate { get; set; }
        public Groups Group { get; set; }
        public decimal SalaryBase { get; set; }
        public long SuperiorID { get; set; }
    }
    class DBConnection
    {
        public static SQLiteConnection CreateConnection(string connectionString)
        {
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            try { connection.Open(); } catch (Exception ex) { }
            return connection;
        }
        public static void CloseConnection(SQLiteConnection connection)
        {
            connection.Close();
        }
        private static SQLiteCommand CreateCommand(SQLiteConnection connection, string commandText)
        {
            SQLiteCommand command;
            command = connection.CreateCommand();
            command.CommandText = commandText;
            return command;
        }
        public static void ExecuteCommand(SQLiteConnection connection, string commandText)
        {
            CreateCommand(connection, commandText).ExecuteNonQuery();
        }
        public static void ReadData(SQLiteConnection connection, string commandText)
        {
            SQLiteDataReader dataReader;
            dataReader = CreateCommand(connection, commandText).ExecuteReader();
            while (dataReader.Read())
            {
                DBEmployee employee = new DBEmployee();
                
                string myreader = "";
                for (int i = 0; i < dataReader.FieldCount; i++) myreader += ReadValue(dataReader, employee, i);
                Console.WriteLine(myreader);
            }
        }
        private static string ReadValue(SQLiteDataReader dataReader, DBEmployee employee, int column)
        {
            object value = dataReader.GetValue(column);
            string propertyName = dataReader.GetName(column);
            switch (propertyName)
            {
                case "group":
                    value = Enum.Parse(typeof(Groups), value.ToString());
                    break;
                case "hiredate":
                    value = DateTimeOffset.FromUnixTimeSeconds((long)value).UtcDateTime;
                    break;
            }
            SetPropValue(employee, propertyName, value);
            return value + " ";
        }
        private static void SetPropValue(object src, string propName, object value)
        {
            PropertyInfo property = src.GetType().GetProperty(propName,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            property.SetValue(src, value);
        }
    }
}

