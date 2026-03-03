using System;
using System.Linq;
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
            CommandManager.InvalidateRequerySuggested();
        }
    }


    private readonly InMemoryBoardRepository _repository;
    public ObservableCollection<CardItem> Cards { get; }

    public ICommand CreateCardCommand { get; }
    public ICommand DeleteCardCommand { get; }


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

        CreateCardCommand = new RelayCommand(execute => CreateCard(board));
        DeleteCardCommand = new RelayCommand(execute => DeleteCard(board), canExecute => SelectedCard != null);
    }


    private void CreateCard(Board board)
    {
        var card = new CardItem("neue Karte");
        board.Cards.Add(card);        
        SelectedCard = card;
        _repository.Save(board);
    }

    private void DeleteCard(Board board)
    {
        if (SelectedCard != null)
        {
            board.Cards.Remove(SelectedCard);
            SelectedCard = null;
            _repository.Save(board);
        }
    }
}
