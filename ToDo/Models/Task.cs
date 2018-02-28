namespace ToDo.Models
{
    public class Task
    {
        public int TaskId { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }

        public int StatusId { get; set; }
        public Status Status { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}

