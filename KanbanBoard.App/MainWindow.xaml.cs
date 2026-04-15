using KanbanBoard.App.ViewModels;
using KanbanBoard.Core.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KanbanBoard.App;

/// <summary>
/// Interaction logic for MainWindow.xaml to Drag & Drop cards.
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }

    private void ListBox_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            if(sender is ListBox listBox)
            {
                if (listBox.SelectedItem is CardItem card)
                {
                    DragDrop.DoDragDrop(listBox, card, DragDropEffects.Move);
                }
            }
        }
    }

    private void ListBox_Drop(object sender, DragEventArgs e)
    {

    }
}