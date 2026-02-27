using System.Collections.ObjectModel;
using KanbanBoard.Core.Models;
using KanbanBoard.Infrastructure.Persistence;


namespace KanbanBoard.App.ViewModels;

public class MainViewModel
{
    private readonly InMemoryBoardRepository _repository;

    public ObservableCollection<CardItem> Cards { get; set; }

    public MainViewModel()
    {
        _repository = new InMemoryBoardRepository();

        var board = _repository.Load();

        Cards = new ObservableCollection<CardItem>(board.Cards);
    }
}
