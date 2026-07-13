using System.Windows.Media;

namespace KanbanBoard.App.Theme;

public static class ThemeColors
{
    public static readonly Brush DarkBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020"));
    public static readonly Brush DarkTextColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#dfdfdf"));

    public static readonly Brush DarkToolBarBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#304545"));
    public static readonly Brush DarkToolBarBorder = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#707070"));

    public static readonly Brush DarkColumnBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#203535"));
    public static readonly Brush DarkColumnBorder = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#555555"));

    public static readonly Brush DarkComboBoxBackground = DarkColumnBackground;
    public static readonly Brush DarkComboBoxBorder = DarkTextColor;
    public static readonly Brush DarkComboBoxHoverBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#405555"));

    public static readonly Brush DarkDatePickerBackground = DarkColumnBackground;
    public static readonly Brush DarkDatePickerBorder = DarkColumnBorder;
    public static readonly Brush DarkDatePickerHoverBackground = DarkComboBoxHoverBackground;

    public static readonly Brush DarkCalendarBackground = DarkColumnBackground;
    public static readonly Brush DarkCalendarBorder = DarkColumnBorder;




    public static readonly Brush LightBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f7f7f7"));
    public static readonly Brush LightTextColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#505050"));

    public static readonly Brush LightToolBarBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fcfcfc"));
    public static readonly Brush LightToolBarBorder = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e0e0e0"));

    public static readonly Brush LightColumnBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fcfcfc"));
    public static readonly Brush LightColumnBorder = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e0e0e0"));

    public static readonly Brush LightComboBoxBackground = LightColumnBackground;
    public static readonly Brush LightComboBoxBorder = LightTextColor;
    public static readonly Brush LightComboBoxHoverBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f0f0f0"));

    public static readonly Brush LightDatePickerBackground = LightColumnBackground;
    public static readonly Brush LightDatePickerBorder = LightColumnBorder;
    public static readonly Brush LightDatePickerHoverBackground = LightComboBoxHoverBackground;

    public static readonly Brush LightCalendarBackground = LightColumnBackground;
    public static readonly Brush LightCalendarBorder = LightColumnBorder;
}