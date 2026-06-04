using System.ComponentModel;
using System.Runtime.CompilerServices;
using KanbanBoard.App.Theme;
using KanbanBoard.Core.Models;

namespace KanbanBoard.App.ViewModels;

/// <summary>
/// CardEdit view model of the application.
/// Handles card edeting state and exposes values required by the edit window.
/// </summary>
public class CardEditViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));



    private string _tagsText = string.Empty;
    public Array StatusValues => Enum.GetValues(typeof(CardStatus));
    
    public string TagsText
    {
        get
        {
            return _tagsText;
        }
        set
        {
            if (_tagsText == value)
                return;

            _tagsText = value;
            OnPropertyChanged();

            Card.Tags = value
                .Split(',')
                .Select(tag => tag.Trim())
                .Where(tag => !string.IsNullOrWhiteSpace(tag))
                .ToList();
        }
    }


    public CardItem Card { get; }
    public ThemeService Theme { get; }



    public CardEditViewModel(CardItem card, ThemeService theme)
    {
        Card = card;
        _tagsText = string.Join(", ", Card.Tags);
        Theme = theme;
    }
}