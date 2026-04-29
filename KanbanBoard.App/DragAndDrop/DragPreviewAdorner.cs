using KanbanBoard.Core.Models;
using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Media;

namespace KanbanBoard.App.DragAndDrop;


public class DragPreviewAdorner : Adorner
{
    //private readonly UIElement _adornedElement;
    private readonly CardItem _card;
    private readonly Border _preview;
    
    protected override int VisualChildrenCount => 1;
    protected override Visual GetVisualChild(int index)
    {
        if (index == 0)
        {
            return _preview;
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
    }
    protected override Size MeasureOverride(Size constraint)
    {
        _preview.Measure(constraint);
        return _preview.DesiredSize;
    }
    protected override Size ArrangeOverride(Size finalSize)
    {
        Rect previewRect = new Rect(10, 10, _preview.DesiredSize.Width, _preview.DesiredSize.Height);
        _preview.Arrange(previewRect);
        return finalSize;
    }

    public DragPreviewAdorner(UIElement adornedElement, CardItem card) : base(adornedElement)
    {
        //_adornedElement = adornedElement;
        _card = card;

        _preview = new Border();
        switch (card.Status)
        {
            case CardStatus.ToDo:
                _preview.Background = Brushes.LightPink;
                break;

            case CardStatus.Doing:
                _preview.Background = Brushes.LightSkyBlue;
                break;

            case CardStatus.Done:
                _preview.Background = Brushes.LightSeaGreen;
                break;
        }
        _preview.BorderBrush = Brushes.LightGray;
        _preview.BorderThickness = new Thickness(1);
        _preview.CornerRadius = new CornerRadius(4);
        _preview.Padding = new Thickness(8);
        _preview.Opacity = 0.5;
        _preview.Child = new TextBlock
        {
            Text = _card.Title,
            FontWeight = FontWeights.Bold,
            FontSize = 13,
            TextWrapping = TextWrapping.Wrap,
            MaxHeight = 40,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0,0,0,4)
        };
    }
}
