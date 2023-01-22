namespace ToDoMysqlApi.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string? Content { get; set; }
        public bool IsComplete { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}