using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using KanbanBoard.Core.Interfaces;
using KanbanBoard.Core.Models;

namespace KanbanBoard.Infrastructure.Persistence;

/// <summary>
/// Loads and saves application settings as JSON files.
/// </summary>
public class JsonSettingsService : ISettingsService
{
    private static readonly string _folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "KanbanBoard");
    private readonly string _filePath = Path.Combine(_folderPath, "settings.json");

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    public AppSettings Load()
    {
        if (!File.Exists(_filePath))
        {
            return new AppSettings();
        }

        try
        {
            string settingsJson = File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(settingsJson))
            {
                return new AppSettings();
            }

            AppSettings? settings = JsonSerializer.Deserialize<AppSettings>(settingsJson, _jsonOptions);
            if (settings == null)
            {
                return new AppSettings();
            }

            if (!Enum.IsDefined(settings.ColorMode))
            {
                return new AppSettings();
            }

            return settings;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JSON load error: {ex.Message}");
            return new AppSettings();
        }
        catch (IOException ex)
        {
            Console.WriteLine($"JSON load error: {ex.Message}");
            return new AppSettings();
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"No permission to read file: {ex.Message}");
            return new AppSettings();
        }

    }

    public void Save(AppSettings settings)
    {
        try
        {
            Directory.CreateDirectory(_folderPath);

            string jsonSettings = JsonSerializer.Serialize<AppSettings>(settings, _jsonOptions);
            File.WriteAllText(_filePath, jsonSettings);
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
