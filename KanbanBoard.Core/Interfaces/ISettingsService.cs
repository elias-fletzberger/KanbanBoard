
using KanbanBoard.Core.Models;

namespace KanbanBoard.Core.Interfaces;

/// <summary>
/// Defines methods for loading and saving application settings.
/// </summary>
public interface ISettingsService
{
    AppSettings Load();
    void Save(AppSettings settings);
}
