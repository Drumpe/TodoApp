namespace TodoApp
{
    public class TodoItem
    {
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }

        public TodoItem(string description)
        {
            Description = description;
            IsCompleted = false;
            CreatedDate = DateTime.Now;
            CompletedDate = null;
        }

        public void MarkCompleted(bool completed)
        {
            IsCompleted = completed;
            CompletedDate = completed ? DateTime.Now : null;
        }
    }
}