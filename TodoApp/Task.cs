namespace TodoApp
{
    public class Task
    {
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? DueDate { get; set; }

        public Task(string description, DateTime? dueDate = null)
        {
            Description = description;
            IsCompleted = false;
            CreatedDate = DateTime.Now;
            CompletedDate = null;
            DueDate = dueDate;
        }

        public void MarkCompleted(bool completed)
        {
            IsCompleted = completed;
            CompletedDate = completed ? DateTime.Now : null;
        }
    }
}