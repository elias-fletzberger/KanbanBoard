
using KanbanBoard.Core.Interfaces;
using KanbanBoard.Core.Models;

namespace KanbanBoard.Infrastructure.Persistence;


/// <summary>
/// In-memory implementation of IBoardRepository.
/// Stores the board data only for the current application runtime
/// without persistent storage.
/// </summary>
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
