
/// <summary>
/// Main view model of the application.
/// Handles UI interaction, exposes commands for creating and deleting cards
/// and coordinates loading and saving through the repository.
/// </summary>



using System;
using System.Linq;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using KanbanBoard.App.Commands;
using KanbanBoard.Core.Models;
using KanbanBoard.Core.Interfaces;
using KanbanBoard.Infrastructure.Persistence;


namespace KanbanBoard.App.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public Array StatusValues => Enum.GetValues(typeof(CardStatus));


 
    private readonly IBoardRepository _repository;
    public ObservableCollection<CardItem> Cards { get; }
    //private readonly Board board;

    private CardItem? _selectedCard;
    public CardItem? SelectedCard
    {
        get => _selectedCard;
        set
        {
            if (_selectedCard == value) return;
            _selectedCard = value;
            OnPropertyChanged();
            CommandManager.InvalidateRequerySuggested();
        }
    }



    public ICommand CreateCardCommand { get; }
    public ICommand DeleteCardCommand { get; }

    

    public MainViewModel()
    {
        _repository = new InMemoryBoardRepository();

        var board = _repository.Load();

        Cards = new ObservableCollection<CardItem>(board.Cards);

        if (!Cards.Any())
        {
            Cards.Add(new CardItem("Erste Testkarte"));
            Cards.Add(new CardItem("Zweite Testkarte"));
            SaveCurrentBoard();
        }

        CreateCardCommand = new RelayCommand(execute => CreateCard());
        DeleteCardCommand = new RelayCommand(execute => DeleteCard(), canExecute => SelectedCard != null);
    }


    private void CreateCard()
    {
        var card = new CardItem("neue Karte");
        Cards.Add(card);
        SelectedCard = card;
        SaveCurrentBoard();
    }

    private void DeleteCard()
    {
        if (SelectedCard != null)
        {
            Cards.Remove(SelectedCard);
            SelectedCard = null;
            SaveCurrentBoard();
        }
    }

    private void SaveCurrentBoard()
    {
        var board = new Board();
        board.Cards = Cards.ToList();
        _repository.Save(board);
    }
}
