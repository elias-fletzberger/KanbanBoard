using System.Windows.Media;

namespace KanbanBoard.App.Theme;

public static class ThemeColors
{
    public static readonly Brush DarkBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020"));
    public static readonly Brush DarkTextColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#dfdfdf"));
    public static readonly Brush DarkToolbarBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#304545"));
    public static readonly Brush DarkToolbarBorder = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#707070"));
    public static readonly Brush DarkColumnBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#203535"));
    public static readonly Brush DarkColumnBorder = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#555555"));

    public static readonly Brush LightBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f7f7f7"));
    public static readonly Brush LightTextColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#505050"));
    public static readonly Brush LightToolbarBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fcfcfc"));
    public static readonly Brush LightToolbarBorder = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e0e0e0"));
    public static readonly Brush LightColumnBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fcfcfc"));
    public static readonly Brush LightColumnBorder = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e0e0e0"));
}
