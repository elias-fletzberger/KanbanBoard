
namespace KanbanBoard.Core.Models;

public class CardItem
{
    public CardItem(string title)
    {
        Id = Guid.NewGuid();
        Title = title;
        Status = CardStatus.ToDo;
        CreatedAt = DateTime.Now;
        UpdatedAt = CreatedAt;
        Tags = new List<string>();
    }

    public Guid Id { get; }
    public string Title { get; set; }
    public CardStatus Status {  get; set; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt {  get; set; }
    public DateTime DueDate { get; set; }
    public List<string> Tags {  get; set; }
}
