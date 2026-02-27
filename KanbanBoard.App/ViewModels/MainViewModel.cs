using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using KanbanBoard.Core.Models;
using KanbanBoard.Infrastructure.Persistence;


namespace KanbanBoard.App.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public Array StatusValues => Enum.GetValues(typeof(CardStatus));

    private readonly InMemoryBoardRepository _repository;

    public ObservableCollection<CardItem> Cards { get; }

    private CardItem? _selectedCard;
    public CardItem? SelectedCard
    {
        get => _selectedCard;
        set
        {
            if (_selectedCard == value) return;
            _selectedCard = value;
            OnPropertyChanged(); 
        }
    }

    public MainViewModel()
    {
        _repository = new InMemoryBoardRepository();

        var board = _repository.Load();

        if (!board.Cards.Any())
        {
            board.Cards.Add(new CardItem("Erste Testkarte"));
            board.Cards.Add(new CardItem("Zweite Testkarte"));
        }

        Cards = new ObservableCollection<CardItem>(board.Cards);
    }
}
