using System.Data.Entity;

namespace Console.Example.EF
{
    public class ClassEFSampleContext : DbContext
    {
        public ClassEFSampleContext(string connectionString)
        {
            //Database.SetInitializer<ClassEFSampleContext>(new DropCreateDatabaseAlways<ClassEFSampleContext>());
            Database.SetInitializer(new CreateDatabaseIfNotExists<ClassEFSampleContext>());
            Database.Connection.ConnectionString = connectionString;
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Task> Tasks { get; set; }
    }
}
