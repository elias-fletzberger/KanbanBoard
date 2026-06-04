using System.Windows;
using KanbanBoard.App.ViewModels;
using KanbanBoard.App.Theme;
using KanbanBoard.Core.Models;


namespace KanbanBoard.App.Views;

/// <summary>
/// Interaction logic for CardEditWindow.xaml to edit cards
/// </summary>
public partial class CardEditWindow : Window
{
    public CardEditWindow(CardItem card, ThemeService theme)
    {
        InitializeComponent();
        DataContext = new CardEditViewModel(card, theme);
    }
}