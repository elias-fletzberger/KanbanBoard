
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
        Board board = new Board();

        if (File.Exists(_filePath))
        {
            string cardJson = File.ReadAllText(_filePath);
            board = JsonSerializer.Deserialize<Board>(cardJson);

            if (board == null)
            {
                return board = new Board();
            }
            else
            {
                return board;
            }
        }
        

        return board;
    }

    public void Save(Board board)
    {
        if (!File.Exists(_folderPath))
        {
            Directory.CreateDirectory(_folderPath);
        }
        
        string jsonBoard = JsonSerializer.Serialize<Board>(board, _jsonOptions);
        File.WriteAllText(_filePath, jsonBoard);
    }
}