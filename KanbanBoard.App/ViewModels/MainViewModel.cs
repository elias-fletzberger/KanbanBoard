
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Threading;
using KanbanBoard.App.Commands;
using KanbanBoard.Core.Interfaces;
using KanbanBoard.Core.Models;
using KanbanBoard.Infrastructure.Persistence;

namespace KanbanBoard.App.ViewModels;


/// <summary>
/// Main view model of the application.
/// Handles UI interaction, exposes commands for creating and deleting cards
/// and coordinates loading and saving through the repository.
/// </summary>
public class MainViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


 
    private readonly IBoardRepository _repository;

    private readonly DispatcherTimer _autoSaveTimer;

    private CardItem? _selectedCard;
    


    public Array StatusValues => Enum.GetValues(typeof(CardStatus));

    public ObservableCollection<CardItem> Cards { get; }

    public CardItem? SelectedCard
    {
        get => _selectedCard;
        set
        {
            if (_selectedCard == value) return;

            if (_selectedCard is not null)
            {
                _selectedCard.PropertyChanged -= SelectedCard_PropertyChanged;
            }

            _selectedCard = value;
            
            if (_selectedCard is not null)
            {
                _selectedCard.PropertyChanged += SelectedCard_PropertyChanged;
            }

            OnPropertyChanged();
            CommandManager.InvalidateRequerySuggested();
        }
    }



    public ICommand CreateCardCommand { get; }
    public ICommand DeleteCardCommand { get; }

    

    public MainViewModel()
    {
        _repository = new JsonBoardRepository();
        
        var board = _repository.Load();

        Cards = new ObservableCollection<CardItem>(board.Cards);

        if (!Cards.Any())
        {
            CardItem card;
            card = new CardItem();
            card.Title = "Erste Testkarte";
            Cards.Add(card);
            card = new CardItem();
            card.Title = "Zweite Testkarte";
            Cards.Add(card);
            SaveCurrentBoard();
        }
                

        CreateCardCommand = new RelayCommand(execute => CreateCard());
        DeleteCardCommand = new RelayCommand(execute => DeleteCard(), canExecute => SelectedCard != null);

        _autoSaveTimer = new DispatcherTimer();
        _autoSaveTimer.Interval = TimeSpan.FromSeconds(1);
        _autoSaveTimer.Tick += AutoSaveTimer_Tick;
    }



    private void CreateCard()
    {
        var card = new CardItem();
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

    private void ScheduleAutoSave()
    {
        _autoSaveTimer.Stop();
        _autoSaveTimer.Start();
    }

    private void AutoSaveTimer_Tick(object? sender, EventArgs e)
    {
        _autoSaveTimer.Stop();
        SaveCurrentBoard();        
    }

    private void SelectedCard_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        ScheduleAutoSave();        
    }
}
