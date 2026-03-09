using KanbanBoard.App.ViewModels;
using System.Windows;

namespace KanbanBoard.App;


public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}