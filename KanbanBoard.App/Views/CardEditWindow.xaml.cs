using KanbanBoard.Core.Models;
using KanbanBoard.App.ViewModels;
using System.Windows;


namespace KanbanBoard.App.Views;

/// <summary>
/// Interaction logic for CardEditWindow.xaml to edit cards
/// </summary>
public partial class CardEditWindow : Window
{
    public CardEditWindow(CardItem card)
    {
        InitializeComponent();
        DataContext = new CardEditViewModel(card);
    }
}