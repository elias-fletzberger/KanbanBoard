
using System;
using System.Text.Json;
using KanbanBoard.Core.Interfaces;
using KanbanBoard.Core.Models;

namespace KanbanBoard.Infrastructure.Persistence;


/// <summary>
/// JSON implementation of IBoardRepository.
/// Stores the board data from the current application runtime
/// to persistent storage.
/// </summary>
public class JsonBoardRepository : IBoardRepository
{
    //private Board _board = new();
    private static readonly string _folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"KanbanBoard");
    private readonly string _filePath = Path.Combine(_folderPath, "board.json");

    private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    public Board Load()
    {
        if (!File.Exists(_filePath))
        {
            return new Board();
        }

        string cardJson = File.ReadAllText(_filePath);
        Board? board = JsonSerializer.Deserialize<Board>(cardJson);

        return board ?? new Board();
    }

    public void Save(Board board)
    {
        Directory.CreateDirectory(_folderPath);   
        
        string jsonBoard = JsonSerializer.Serialize<Board>(board, _jsonOptions);
        File.WriteAllText(_filePath, jsonBoard);
    }
}