using System;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using KanbanBoard.App.Commands;
using KanbanBoard.Core.Models;
using KanbanBoard.Infrastructure.Persistence;


namespace KanbanBoard.App.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public Array StatusValues => Enum.GetValues(typeof(CardStatus));


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


    private readonly InMemoryBoardRepository _repository;
    public ObservableCollection<CardItem> Cards { get; }

    public ICommand CreateCardCommand { get; }


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

        CreateCardCommand = new RelayCommand(_ => CreateCard());
    }


    private void CreateCard()
    {
        var card = new CardItem("neue Karte");
        Cards.Add(card);
        SelectedCard = card;
    }
}
