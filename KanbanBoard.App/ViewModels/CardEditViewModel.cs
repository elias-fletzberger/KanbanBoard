using KanbanBoard.Core.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace KanbanBoard.App.ViewModels;

/// <summary>
/// CardEdit view model of the application.
/// Handles card edeting state and exposes values required by the edit window.
/// </summary>
public class CardEditViewModel : INotifyPropertyChanged
{
    private string _tagsText = string.Empty;
    public Array StatusValues => Enum.GetValues(typeof(CardStatus));

    public CardItem Card { get; }

    public CardEditViewModel(CardItem card)
    {
        Card = card;
        _tagsText = string.Join(", ", Card.Tags);
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


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
}