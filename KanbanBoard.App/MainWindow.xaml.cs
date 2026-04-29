using KanbanBoard.App.ViewModels;
using KanbanBoard.App.DragAndDrop;
using KanbanBoard.Core.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace KanbanBoard.App;

/// <summary>
/// Interaction logic for MainWindow.xaml to Drag & Drop cards.
/// </summary>
public partial class MainWindow : Window
{
    private DragPreviewAdorner? _dragPreview;

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
                    if (listBox.ItemContainerGenerator.ContainerFromItem(card) is ListBoxItem listBoxItem)
                    {
                        listBoxItem.Opacity = 0.5;

                        AdornerLayer? adornerLayer = AdornerLayer.GetAdornerLayer(RootGrid);
                        _dragPreview = new DragPreviewAdorner(RootGrid, card);

                        if (adornerLayer is not null)
                        {
                            adornerLayer.Add(_dragPreview);                          
                        }
                        try
                        {
                            DragDrop.DoDragDrop(listBox, card, DragDropEffects.Move);
                        }
                        finally
                        {
                            if (adornerLayer is not null && _dragPreview is not null)
                            {
                                adornerLayer.Remove(_dragPreview);
                            }

                            _dragPreview = null;
                            listBoxItem.Opacity = 1.0;
                        }
                    }
                }
            }
        }
    }

    private void ListBox_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(typeof(CardItem)))
        {
            if (sender is ListBox listBox)
            {
                var card = e.Data.GetData(typeof(CardItem)) as CardItem;
                var column = Grid.GetColumn(listBox);

                if (card is not null)
                {
                    var newStatus = card.Status;

                    switch (column)
                    {
                        case 0:
                            newStatus = CardStatus.ToDo;
                            break;
                        case 1:
                            newStatus = CardStatus.Doing;
                            break;
                        case 2:
                            newStatus = CardStatus.Done;
                            break;
                        default:
                            break;
                    }

                    if(card.Status!= newStatus)
                    {
                        card.Status = newStatus;
                    }
                }
            }
        }
    }

    private void RootGrid_DragOver(object sender, DragEventArgs e)
    {
        Point pos = e.GetPosition(RootGrid);
        _dragPreview?.UpdatePosition(pos.X, pos.Y);
              
    }
}