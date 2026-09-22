using KanbanBoard.Core.Models;

namespace KanbanBoard.Core.Interfaces;


/// <summary>
/// Defines the contract for loading, adding, updating, deleting,
/// and bulk-saving board data.
/// Allows the application to remain independent from the
/// concrete storage implementation.
/// </summary>
public interface IBoardRepository
{
    Board Load();
    void SaveAll(Board board);
    void Add(CardItem card);
    void Update(CardItem card);
    void Delete(Guid id);
}