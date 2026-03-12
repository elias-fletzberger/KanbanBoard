
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
        if (string.IsNullOrWhiteSpace(cardJson))
        {  
            return new Board(); 
        }

        try
        {
            Board? board = JsonSerializer.Deserialize<Board>(cardJson);
            return board ?? new Board();
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JSON load error: {ex.Message}");
            return new Board();
        }
        catch (IOException ex)
        {
            Console.WriteLine($"JSON load error: {ex.Message}");
            return new Board();
        }
        
    }

    public void Save(Board board)
    {
        Directory.CreateDirectory(_folderPath);

        try
        {
            string jsonBoard = JsonSerializer.Serialize<Board>(board, _jsonOptions);
            File.WriteAllText(_filePath, jsonBoard);
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Save failed: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"No permission to write file: {ex.Message}");
        }
    }
}