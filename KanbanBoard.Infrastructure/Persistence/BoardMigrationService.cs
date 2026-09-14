using KanbanBoard.Core.Interfaces;
using KanbanBoard.Core.Models;

namespace KanbanBoard.Infrastructure.Persistence;


/// <summary>
/// Migrates existing board data from JSON storage to SQLite when needed.
/// </summary>
public class BoardMigrationService
{
    private readonly IBoardRepository _sqliteRepository;
    private readonly IBoardRepository _jsonRepository;
    

    public BoardMigrationService(IBoardRepository sqliteRepository, IBoardRepository jsonRepository)
    {
        _sqliteRepository = sqliteRepository;
        _jsonRepository = jsonRepository;
    }


    public void MigrateJsonToSqlite()
    {
        Board sqliteboard = _sqliteRepository.Load();

        if (sqliteboard.Cards.Any()) return;

      
                
        Board jsonBoard = _jsonRepository.Load();

        if (!jsonBoard.Cards.Any()) return;

        _sqliteRepository.Save(jsonBoard);
    }
}