using System;

namespace Console.Example.EF
{
    public class Task
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsComplete { get; set; }
        public virtual User AssignedTo { get; set; }

        public override string ToString()
        {
            return "Task [id=" + TaskId + ", title=" + Title + ", dueDate=" + DueDate.ToString() + 
                ", IsComplete=" + IsComplete + "]";
        }
    }
}
