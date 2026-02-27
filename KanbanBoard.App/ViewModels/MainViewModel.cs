using System;
using System.Collections.ObjectModel;
using KanbanBoard.Core.Models;
using KanbanBoard.Infrastructure.Persistence;


namespace KanbanBoard.App.ViewModels;

public class MainViewModel
{
    private readonly InMemoryBoardRepository _repository;

    public Array StatusValues => Enum.GetValues(typeof(CardStatus));

    public ObservableCollection<CardItem> Cards { get; set; }

    public CardItem? SelectedCard { get; set; }

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
