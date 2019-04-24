using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Console.Example.EF.DataModel.Tables
{
    [Table("TASK")]
    public class Task
    {
        [Key]
        public int ID { get; set; }
        public string TITLE { get; set; }
        public DateTime DUEDATE { get; set; }
        public bool IS_COMPLETE { get; set; }
        public virtual User ASSIGNED_TO { get; set; }

        public override string ToString()
        {
            return 
                "TASK [ID=" + ID + 
                "; TITLE=" + TITLE + 
                "; DUEDATE=" + DUEDATE.ToString() +
                "; IS_COMPLETE=" + IS_COMPLETE + "]";
        }
    }
}
