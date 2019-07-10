using System;
using System.Data.SQLite;
using System.Reflection;
using MD.Salary.Model;

namespace MD.Salary.Database
{
    class DBEmployee
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime HireDate { get; set; }
        public Groups Group { get; set; }
        public decimal SalaryBase { get; set; }
        public int SuperiorID { get; set; }
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
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    object value = null;
                    switch (dataReader.GetFieldType(i).Name)
                    {
                        case "String":
                            value = dataReader.GetString(i);
                            break;
                        case "Int64":
                            value = dataReader.GetInt64(i);
                            break;
                        case "Decimal":
                            value = dataReader.GetDecimal(i);
                            break;
                    }
                    object prop = GetPropValue(employee, dataReader.GetName(i), value);
                    myreader += value + " " + dataReader.GetName(i) + " " + dataReader.GetFieldType(i).Name + "\n";
                }
                Console.WriteLine(myreader);
            }
        }
        public static object GetPropValue(object src, string propName, object value)
        {
            PropertyInfo property = src.GetType().GetProperty(propName,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propName != "group") { property.SetValue(src, value); }
            return property.GetValue(src, null);
        }
    }
}

