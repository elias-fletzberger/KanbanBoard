using KanbanBoard.App.Theme;
using KanbanBoard.App.ViewModels;
using KanbanBoard.Core.Interfaces;
using KanbanBoard.Core.Models;
using KanbanBoard.Infrastructure.Persistence;
using System.Windows;

namespace KanbanBoard.App;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ISettingsService settingsService = new JsonSettingsService();
        var settings = settingsService.Load();

        ThemeService theme = new ThemeService();
        theme.IsDarkmodeActive = settings.ColorMode == ColorMode.Dark;

        MainViewModel viewModel = new MainViewModel(theme, settings, settingsService);

        MainWindow window = new MainWindow();
        window.DataContext = viewModel;
        window.Show();
    }
}