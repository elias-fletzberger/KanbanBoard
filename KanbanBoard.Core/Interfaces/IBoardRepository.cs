
/// <summary>
/// Defines the contract for loading and saving a board.
/// Allows the application to remain independent from the
/// concrete storage implementation.
/// </summary>



using KanbanBoard.Core.Models;


namespace KanbanBoard.Core.Interfaces;

public interface IBoardRepository
{
    Board Load();
    void Save(Board board);
}
