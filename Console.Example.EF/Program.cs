using System;
using System.Data.SqlClient;
using System.Linq;
using Console.Example.EF.DataModel;
using Console.Example.EF.DataModel.Tables;

namespace Console.Example.EF
{
    internal class Program
    {
        private static void Main()
        {
            var builder = CreateSqlConnection("localhost", "master");
            var db = "SAMPLE";
            var table = "EMPLOYEES";
            switch (SetSwitchConection())
            {
                case 1:
                    CreateSqlConnection(builder, db, table);
                    break;
                case 2:
                    CreateEfConnection(builder, db);
                    break;
            }
            System.Console.WriteLine();

            System.Console.WriteLine("Press any key to finish...");
            System.Console.ReadKey(true);
        }

        private static SqlConnectionStringBuilder CreateSqlConnection(string dataSource, string initialCatalog)
        {
            System.Console.WriteLine("--------------------------------------------------------------------------------");
            System.Console.WriteLine("SqlConnectionStringBuilder:");
            var builder = new SqlConnectionStringBuilder() {
                Authentication = SqlAuthenticationMethod.ActiveDirectoryIntegrated,
                TrustServerCertificate = true,
                DataSource = dataSource,
                InitialCatalog = initialCatalog,
                ConnectTimeout = 30,
            };
            System.Console.WriteLine($"  - Authentication:         {builder.Authentication}.");
            System.Console.WriteLine($"  - TrustServerCertificate: {builder.TrustServerCertificate}.");
            System.Console.WriteLine($"  - DataSource:             {builder.DataSource}.");
            System.Console.WriteLine($"  - InitialCatalog:         {builder.InitialCatalog}.");
            System.Console.WriteLine($"  - ConnectTimeout:         {builder.ConnectTimeout}.");
            return builder;
        }

        private static int SetSwitchConection()
        {
            System.Console.WriteLine("--------------------------------------------------------------------------------");
            System.Console.WriteLine("0. Exit.");
            System.Console.WriteLine("1. Create SQL connection.");
            System.Console.WriteLine("2. Create EF connection.");
            System.Console.WriteLine("--------------------------------------------------------------------------------");
            var message = "Input choice: ";
            int value = 0;
            bool check = false;
            while (!check)
            {
                try
                {
                    System.Console.Write(message);
                    value = Convert.ToInt32(System.Console.ReadLine());
                    if (value >= 0 && value <= 2)
                        check = true;
                }
                catch
                {
                    //
                }
            }
            return value;
        }

        #region SqlConnection

        private static void CreateSqlConnection(SqlConnectionStringBuilder builder, string db, string table)
        {
            System.Console.WriteLine("--------------------------------------------------------------------------------");
            System.Console.WriteLine("Try create SQL connection.");
            try
            {
                using (var con = new SqlConnection(builder.ConnectionString))
                {
                    con.Open();
                    System.Console.WriteLine("  - Sql connect is opened.");
                    DropDb(con, db);
                    DropTable(con, db, table);
                    if (CreateDb(con, db))
                    {
                        if (CreateTable(con, db, table))
                        {
                            InsertData(con, db, table);
                            SelectData(con, db, table);
                        }
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
            catch (Exception exception)
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
            catch (Exception exception)
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
            catch (Exception exception)
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
            catch (Exception exception)
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
            catch (Exception exception)
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
            catch (Exception exception)
            {
                System.Console.WriteLine($"  - error: {exception.Message}!");
            }
            return false;
        }

        #endregion

        #region Entity Framework

        private static void CreateEfConnection(SqlConnectionStringBuilder builder, string db)
        {
            System.Console.WriteLine("--------------------------------------------------------------------------------");
            System.Console.WriteLine("Try create EF connection.");
            try
            {
                using (var con = new SqlConnection(builder.ConnectionString))
                {
                    con.Open();
                    System.Console.WriteLine("  - Sql connect is opened.");
                    builder.InitialCatalog = db;
                    using (var context = new DbContext(builder.ConnectionString))
                    {
                        System.Console.WriteLine("  - Created database schema from C# classes.");

                        SetSwitchEf(context);
                    }
                }
            }
            catch (SqlException exception)
            {
                System.Console.WriteLine($"  - error: {exception.Message}!");
            }
        }

        private static void SetSwitchEf(DbContext context)
        {
            System.Console.WriteLine("--------------------------------------------------------------------------------");
            System.Console.WriteLine("0. Exit.");
            System.Console.WriteLine("1. Test.");
            System.Console.WriteLine("2. Clear tables.");
            System.Console.WriteLine("3. Fill 'IRIS' table.");
            System.Console.WriteLine("4. ML. Predict flower type.");
            System.Console.WriteLine("--------------------------------------------------------------------------------");
            var message = "Input choice: ";
            int value = 0;
            bool exit = false;
            while (!exit)
            {
                try
                {
                    System.Console.Write(message);
                    value = Convert.ToInt32(System.Console.ReadLine());
                    if (value == 0)
                    {
                        exit = true;
                        break;
                    }
                    switch (value)
                    {
                        #region Test
                        case 1:
                            EfDeleteTasks(context);
                            EfDeleteUsers(context);
                            context.TableIrisClear();

                            // Create User
                            var user = EfCreateUser(context);
                            // Create Task
                            var task = EfCreateTask(context);
                            var task2 = EfCreateTask(context);

                            // Assign task to user
                            EfAssignTaskToUser(context, task, user);

                            // Find incomplete tasks assigned to user 'Anna'
                            System.Console.WriteLine("Incomplete tasks assigned to 'Anna':");
                            var query = from t in context.Task
                                        where t.IS_COMPLETE == false && t.ASSIGNED_TO.FIRST_NAME.Equals("Anna")
                                        select t;
                            foreach (var t in query)
                            {
                                System.Console.WriteLine(t.ToString());
                            }

                            // Update demo: change the 'dueDate' of a task
                            var taskToUpdate = context.Task.First(); // get the first task
                            System.Console.WriteLine("Updating task: " + taskToUpdate.ToString());
                            taskToUpdate.DUEDATE = DateTime.Parse("2019-02-02");
                            context.SaveChanges();
                            System.Console.WriteLine("dueDate changed: " + taskToUpdate.ToString());

                            EfShowTasks(context);
                            EfShowUsers(context);
                            break;
                        #endregion
                        #region Delete tables
                        case 2:
                            EfDeleteTasks(context);
                            EfDeleteUsers(context);
                            context.TableIrisClear();
                            break;
                        #endregion
                        #region Fill 'IRIS' table
                        case 3:
                            var res = context.TableFillIris("iris-data.txt", ',');
                            System.Console.WriteLine($"TableFillIris: {res}");
                            break;  
                            #endregion
                    }
                }
                catch (Exception exception)
                {
                    System.Console.WriteLine(exception.Message);
                }
            }
        }

        private static void EfDeleteUsers(DbContext context, IQueryable<User> query = null, int id = default)
        {
            if (query == null)
                query = from u in context.User
                        select u;
            else
                query = from u in context.User
                        where u.ID == id
                        select u;
            System.Console.WriteLine("Deleting users");
            foreach (var u in query)
            {
                context.User.Remove(u);
                System.Console.WriteLine("Deleting user: " + u.ToString());
            }
            context.SaveChanges();
        }

        private static void EfDeleteTasks(DbContext context, IQueryable<Task> query = null, DateTime dt = default)
        {
            if (query == null)
                query = from t in context.Task
                        select t;
            else
                query = from t in context.Task
                        where t.DUEDATE < dt
                        select t;
            System.Console.WriteLine("Deleting tasks");
            foreach (var t in query)
            {
                context.Task.Remove(t);
                System.Console.WriteLine("Deleting task: " + t.ToString());
            }
            context.SaveChanges();
        }

        private static void EfShowUsers(DbContext context)
        {
            System.Console.WriteLine("Table Users:");
            var query = from u in context.User select u;
            if (query.ToList().Count == 0)
            {
                System.Console.WriteLine("[None]");
            }
            else
            {
                foreach (var u in query)
                {
                    System.Console.WriteLine(u.ToString());
                }
            }
        }

        private static void EfShowTasks(DbContext context)
        {
            System.Console.WriteLine("Table Task:");
            var query = from t in context.Task select t;
            if (query.ToList().Count == 0)
            {
                System.Console.WriteLine("[None]");
            }
            else
            {
                foreach (var t in query)
                {
                    System.Console.WriteLine(t.ToString());
                }
            }
        }

        private static User EfCreateUser(DbContext context)
        {
            var user = new User
            {
                FIRST_NAME = "Anna",
                LAST_NAME = "Shrestinian"
            };
            context.User.Add(user);
            context.SaveChanges();
            System.Console.WriteLine("Created User: " + user.ToString());
            return user;
        }

        private static Task EfCreateTask(DbContext context)
        {
            var task = new Task()
            {
                TITLE = "Ship Helsinki",
                IS_COMPLETE = false,
                DUEDATE = DateTime.Parse("2019-01-01")
            };
            context.Task.Add(task);
            context.SaveChanges();
            System.Console.WriteLine("Created Task: " + task.ToString());
            return task;
        }

        private static void EfAssignTaskToUser(DbContext context, Task task, User user)
        {
            task.ASSIGNED_TO = user;
            context.SaveChanges();
            System.Console.WriteLine("Assigned Task: '" + task.TITLE + "' to user '" + user.GetFullName() + "'");
        }

        #endregion
    }
}
