using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using KanbanBoard.App.Icons;

namespace KanbanBoard.App.Theme;


public class ThemeService : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));



    private bool _isDarkmodeActive = false;


    public bool IsDarkmodeActive
    {
        get => _isDarkmodeActive;
        set
        {
            if (_isDarkmodeActive == value) return;
            _isDarkmodeActive = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(DarkmodeIcon));
            OnPropertyChanged(nameof(WindowBackground));
            OnPropertyChanged(nameof(TextColor));
            OnPropertyChanged(nameof(ToolbarBackground));
            OnPropertyChanged(nameof(ToolbarBorder));
            OnPropertyChanged(nameof(ColumnBackground));
            OnPropertyChanged(nameof(ColumnBorder));
        }
    }



    public Geometry DarkmodeIcon
    {
        get
        {
            return IsDarkmodeActive
                ? AppIcons.Sun
                : AppIcons.MoonStars;
        }
    }

    public Brush WindowBackground
    {
        get
        {
            return IsDarkmodeActive ? ThemeColors.DarkBackground : ThemeColors.LightBackground;
        }
    }
    public Brush TextColor
    {
        get
        {
            return IsDarkmodeActive ? ThemeColors.DarkTextColor : ThemeColors.LightTextColor;
        }
    }
    public Brush ToolbarBackground
    {
        get
        {
            return IsDarkmodeActive ? ThemeColors.DarkToolbarBackground : ThemeColors.LightToolbarBackground;
        }
    }
    public Brush ToolbarBorder
    {
        get
        {
            return IsDarkmodeActive ? ThemeColors.DarkToolbarBorder : ThemeColors.LightToolbarBorder;
        }
    }
    public Brush ColumnBackground
    {
        get
        {
            return IsDarkmodeActive ? ThemeColors.DarkColumnBackground : ThemeColors.LightColumnBackground;
        }
    }
    public Brush ColumnBorder
    {
        get
        {
            return IsDarkmodeActive ? ThemeColors.DarkColumnBorder : ThemeColors.LightColumnBorder;
        }
    }


    public void ToggleTheme()
    {
        IsDarkmodeActive = !IsDarkmodeActive;
    }
}
