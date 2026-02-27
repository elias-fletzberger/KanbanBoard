
namespace KanbanBoard.Core.Models;

public class Board
{
    public List<CardItem> Cards { get; set; }
    
    public Board()
    {
        Cards = new List<CardItem>();
    }
}
