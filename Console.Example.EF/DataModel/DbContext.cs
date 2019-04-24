using System;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Console.Example.EF.DataModel
{
    public class DbContext : System.Data.Entity.DbContext
    {
        public DbContext(string connectionString)
        {
            //Database.SetInitializer<DbContext>(new DropCreateDatabaseAlways<DbContext>());
            Database.SetInitializer<DbContext>(new CreateDatabaseIfNotExists<DbContext>());
            Database.Connection.ConnectionString = connectionString;
        }

        public DbSet<Tables.User> User { get; set; }
        public DbSet<Tables.Task> Task { get; set; }
        public DbSet<Tables.Iris> Iris{ get; set; }

        public void TableIrisClear()
        {
            var query = from item in Iris select item;
            foreach (var item in query)
            {
                Iris.Remove(item);
            }
            System.Console.WriteLine("Clear 'IRIS' table complete.");
            SaveChanges();
        }


        public int TableFillIris(string fileName, char separator)
        {
            var sr = new StreamReader(fileName);
            while (!sr.EndOfStream)
            {
                var record = sr.ReadLine().Split(separator);
                if (record.Length == 5)
                {
                    var iris = new Tables.Iris
                    {
                        SEPAL_LENGTH = float.Parse(record[0], CultureInfo.InvariantCulture.NumberFormat),
                        SEPAL_WIDTH = float.Parse(record[1], CultureInfo.InvariantCulture.NumberFormat),
                        PETAL_LENGTH = float.Parse(record[2], CultureInfo.InvariantCulture.NumberFormat),
                        PETAL_WIDTH = float.Parse(record[3], CultureInfo.InvariantCulture.NumberFormat),
                        LABEL = record[4],
                    };
                    Iris.Add(iris);
                }
            }
            System.Console.WriteLine("Fill 'IRIS' table complete.");
            return SaveChanges();
        }
    }
}
