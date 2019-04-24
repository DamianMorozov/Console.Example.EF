using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Console.Example.EF.DataModel.Tables
{
    [Table("USER")]
    public class User
    {
        [Key]
        public int ID { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public virtual IList<Task> Tasks { get; set; }

        public string GetFullName()
        {
            return FIRST_NAME + " " + LAST_NAME;
        }

        public override string ToString()
        {
            return "USER [ID=" + ID + "; NAME=" + GetFullName() + "]";
        }
    }
}
