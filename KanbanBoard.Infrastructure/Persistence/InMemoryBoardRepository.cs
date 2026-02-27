using KanbanBoard.Core.Interfaces;
using KanbanBoard.Core.Models;


namespace KanbanBoard.Infrastructure.Persistence;

public class InMemoryBoardRepository : IBoardRepository
{
    private Board _board = new();

    public Board Load()
    {
        return _board; 
    }

    public void Save(Board board)
    {
        _board = board;
    }
}
