using KanbanBoard.Core.Models;


namespace KanbanBoard.Core.Interfaces;

public interface IBoardRepository
{
    Board Load();
    void Save(Board board);
}
