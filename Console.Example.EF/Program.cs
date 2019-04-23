using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                //UserID = Environment.UserName,
                DataSource = "tcp:localhost",
                InitialCatalog = "master",
                ConnectTimeout = 30,
                TrustServerCertificate = true,
            };
            System.Console.WriteLine($"  builder.Authentication: {builder.Authentication}.");
            System.Console.WriteLine($"  builder.DataSource: {builder.DataSource}.");
            System.Console.WriteLine($"  builder.InitialCatalog: {builder.InitialCatalog}.");
            return builder;
        }

        private static void SqlConnect(SqlConnectionStringBuilder builder)
        {
            System.Console.WriteLine("Sql try connect.");
            try
            {
                using (var con = new SqlConnection(builder.ConnectionString))
                {
                    System.Console.WriteLine("  Sql is connected.");
                    con.Open();
                    System.Console.WriteLine("  Sql connect is opened.");


                }

            }
            catch (SqlException exception)
            {
                System.Console.WriteLine($"  Sql error: {exception.Message}!");
            }
        }

        
    }
}
