
/// <summary>
/// Represents the Kanban board and holds the collection of cards.
/// Serves as the root domain object that is loaded and saved
/// by the repository.
/// </summary>



namespace KanbanBoard.Core.Models;

public class Board
{
    public List<CardItem> Cards { get; set; }
    
    public Board()
    {
        Cards = new List<CardItem>();
    }
}
