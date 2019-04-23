using System.Data.SqlClient;

namespace Console.Example.EF
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var builder = CreateSqlConnection();
            SqlConnect(builder);

            System.Console.WriteLine();
            System.Console.WriteLine("Press any key to finish...");
            System.Console.ReadKey(true);
        }

        private static SqlConnectionStringBuilder CreateSqlConnection()
        {
            System.Console.WriteLine("Create new SqlConnectionStringBuilder.");
            var builder = new SqlConnectionStringBuilder() {
                Authentication = SqlAuthenticationMethod.ActiveDirectoryIntegrated,
                TrustServerCertificate = true,
                DataSource = "tcp:localhost",
                InitialCatalog = "master",
                ConnectTimeout = 30,
            };
            System.Console.WriteLine($"  - Authentication: {builder.Authentication}.");
            System.Console.WriteLine($"  - TrustServerCertificate: {builder.TrustServerCertificate}.");
            System.Console.WriteLine($"  - DataSource: {builder.DataSource}.");
            System.Console.WriteLine($"  - InitialCatalog: {builder.InitialCatalog}.");
            System.Console.WriteLine($"  - ConnectTimeout: {builder.ConnectTimeout}.");
            return builder;
        }

        private static void SqlConnect(SqlConnectionStringBuilder builder)
        {
            System.Console.WriteLine("Try Sql connect.");
            try
            {
                using (var con = new SqlConnection(builder.ConnectionString))
                {
                    con.Open();
                    System.Console.WriteLine("  - Sql connect is opened.");
                    var db = "SAMPLE";
                    var table = "EMPLOYEES";
                    DropDb(con, db);
                    DropTable(con, db, table);
                    if (CreateDb(con, db) && CreateTable(con, db, table))
                    {
                        InsertData(con, db, table);
                        SelectData(con, db, table);
                    }
                }
            }
            catch (SqlException exception)
            {
                System.Console.WriteLine($"  - error: {exception.Message}!");
            }
        }

        private static bool DropDb(SqlConnection con, string db)
        {
            try
            {
                System.Console.WriteLine($"Try dropping database '{db}'.");
                var query = $@"
DROP DATABASE IF EXISTS [{db}]
                    ".TrimStart('\r', ' ', '\n').TrimEnd('\r', ' ', '\n');
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.ExecuteNonQuery();
                    System.Console.WriteLine("  - droping database is finished.");
                }
                return true;
            }
            catch (System.Exception exception)
            {
                System.Console.WriteLine($"  - error: {exception.Message}!");
            }
            return false;
        }

        private static bool CreateDb(SqlConnection con, string db)
        {
            try
            {
                System.Console.WriteLine($"Try creating database '{db}'.");
                var query = $@"
IF NOT EXISTS (SELECT NAME FROM SYS.DATABASES WHERE NAME = N'{db}')
    CREATE DATABASE [{db}]
                    ".TrimStart('\r', ' ', '\n').TrimEnd('\r', ' ', '\n');
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.ExecuteNonQuery();
                    System.Console.WriteLine("  - create database is finished.");
                }
                return true;
            }
            catch (System.Exception exception)
            {
                System.Console.WriteLine($"  - error: {exception.Message}!");
            }
            return false;
        }

        private static bool DropTable(SqlConnection con, string db, string table)
        {
            try
            {
                System.Console.WriteLine($"Try droping table '{table}'.");
                var query = $@"
USE [{db}];
IF EXISTS (SELECT NAME FROM SYS.TABLES WHERE NAME = N'{table}')
DROP TABLE [{table}]
                ".TrimStart('\r', ' ', '\n').TrimEnd('\r', ' ', '\n');
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.ExecuteNonQuery();
                    System.Console.WriteLine("  - droping table is finished.");
                }
                return true;
            }
            catch (System.Exception exception)
            {
                System.Console.WriteLine($"  - error: {exception.Message}!");
            }
            return false;
        }

        private static bool CreateTable(SqlConnection con, string db, string table)
        {
            try
            {
                System.Console.WriteLine($"Try creating table '{table}'.");
                var query = $@"
USE [{db}];
IF NOT EXISTS (SELECT NAME FROM SYS.TABLES WHERE NAME = N'{table}')
CREATE TABLE {table} (
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    NAME NVARCHAR(50), 
    LOCATION NVARCHAR(50) 
); 
                ".TrimStart('\r', ' ', '\n').TrimEnd('\r', ' ', '\n');
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.ExecuteNonQuery();
                    System.Console.WriteLine("  - creating table is finished.");
                }
                return true;
            }
            catch (System.Exception exception)
            {
                System.Console.WriteLine($"  - error: {exception.Message}!");
            }
            return false;
        }

        private static bool InsertData(SqlConnection con, string db, string table)
        {
            try
            {
                System.Console.WriteLine($"Try inserting data into table '{table}'.");
                var query = $@"
USE [{db}];
INSERT INTO {table} (NAME, LOCATION) VALUES 
    (N'Jared', N'Australia'), 
    (N'Nikita', N'India'), 
    (N'Tom', N'Germany'); 
                ".TrimStart('\r', ' ', '\n').TrimEnd('\r', ' ', '\n');
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.ExecuteNonQuery();
                    System.Console.WriteLine("  - inserting rows into table is finished.");
                }
                return true;
            }
            catch (System.Exception exception)
            {
                System.Console.WriteLine($"  - error: {exception.Message}!");
            }
            return false;
        }

        private static bool SelectData(SqlConnection con, string db, string table)
        {
            try
            {
                System.Console.WriteLine($"Try reading data from table '{table}'.");
                var query = $@"
USE [{db}];
SELECT ID, NAME, LOCATION FROM {table} 
                ".TrimStart('\r', ' ', '\n').TrimEnd('\r', ' ', '\n');
                using (var cmd = new SqlCommand(query, con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                System.Console.WriteLine(
                                    $"{reader.GetInt32(0)} {reader.GetString(1)} {reader.GetString(2)}");
                            }
                        }
                    }

                    System.Console.WriteLine("  - reading rows from table is finished.");
                }
                return true;
            }
            catch (System.Exception exception)
            {
                System.Console.WriteLine($"  - error: {exception.Message}!");
            }
            return false;
        }
    }
}
