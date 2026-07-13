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
            OnPropertyChanged(nameof(ToolBarBackground));
            OnPropertyChanged(nameof(ToolBarBorder));
            OnPropertyChanged(nameof(ColumnBackground));
            OnPropertyChanged(nameof(ColumnBorder));
            OnPropertyChanged(nameof(ComboBoxBackground));
            OnPropertyChanged(nameof(ComboBoxBorder));
            OnPropertyChanged(nameof(ComboBoxHoverBackground));
            OnPropertyChanged(nameof(DatePickerBackground));
            OnPropertyChanged(nameof(DatePickerBorder));
            OnPropertyChanged(nameof(DatePickerHoverBackground));
            OnPropertyChanged(nameof(CalendarBackground));
            OnPropertyChanged(nameof(CalendarBorder));
        }
    }



    public Geometry DarkmodeIcon =>
        IsDarkmodeActive
            ? AppIcons.Sun
            : AppIcons.MoonStars;
    public Brush WindowBackground =>
        IsDarkmodeActive
            ? ThemeColors.DarkBackground
            : ThemeColors.LightBackground;
    public Brush TextColor =>
        IsDarkmodeActive
            ? ThemeColors.DarkTextColor
            : ThemeColors.LightTextColor;
    public Brush ToolBarBackground =>
        IsDarkmodeActive
            ? ThemeColors.DarkToolBarBackground
            : ThemeColors.LightToolBarBackground;
    public Brush ToolBarBorder =>
        IsDarkmodeActive
            ? ThemeColors.DarkToolBarBorder
            : ThemeColors.LightToolBarBorder;
    public Brush ColumnBackground =>
        IsDarkmodeActive
            ? ThemeColors.DarkColumnBackground
            : ThemeColors.LightColumnBackground;
    public Brush ColumnBorder =>
        IsDarkmodeActive
            ? ThemeColors.DarkColumnBorder
            : ThemeColors.LightColumnBorder;
    public Brush ComboBoxBackground =>
        IsDarkmodeActive
            ? ThemeColors.DarkComboBoxBackground
            : ThemeColors.LightComboBoxBackground;
    public Brush ComboBoxBorder =>
        IsDarkmodeActive
            ? ThemeColors.DarkComboBoxBorder
            : ThemeColors.LightComboBoxBorder;
    public Brush ComboBoxHoverBackground =>
        IsDarkmodeActive
            ? ThemeColors.DarkComboBoxHoverBackground
            : ThemeColors.LightComboBoxHoverBackground;
    public Brush DatePickerBackground =>
        IsDarkmodeActive
            ? ThemeColors.DarkDatePickerBackground
            : ThemeColors.LightDatePickerBackground;
    public Brush DatePickerBorder =>
        IsDarkmodeActive
            ? ThemeColors.DarkDatePickerBorder 
            : ThemeColors.LightDatePickerBorder;
    public Brush DatePickerHoverBackground =>
        IsDarkmodeActive
            ? ThemeColors.DarkDatePickerHoverBackground
            : ThemeColors.LightDatePickerHoverBackground;
    public Brush CalendarBackground =>
        IsDarkmodeActive
            ? ThemeColors.DarkCalendarBackground
            : ThemeColors.LightCalendarBackground;
    public Brush CalendarBorder =>
        IsDarkmodeActive
            ? ThemeColors.DarkCalendarBorder
            : ThemeColors.LightCalendarBorder;

    public void ToggleTheme() => IsDarkmodeActive = !IsDarkmodeActive;
}
