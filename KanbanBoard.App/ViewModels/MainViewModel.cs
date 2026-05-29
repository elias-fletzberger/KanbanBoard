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
    private string _tagsText;
    private bool _isSortDescending = true;
    private CardSortMode _selectedSortMode = CardSortMode.CreatedAt;
    private bool _isDarkmodeActive = false;

    public Array StatusValues => Enum.GetValues(typeof(CardStatus));
    public Array SortModes => Enum.GetValues(typeof(CardSortMode));


    public ObservableCollection<CardItem> Cards { get; }
    private IEnumerable<CardItem> SortCards(IEnumerable<CardItem> cards)
    {
        IEnumerable<CardItem> sortedCards;

        switch (SelectedSortMode)
        {
            case CardSortMode.CreatedAt:
                sortedCards = cards.OrderBy(card => card.CreatedAt);
                break;

            case CardSortMode.UpdatedAt:
                sortedCards = cards.OrderBy(card => card.UpdatedAt);
                break;

            case CardSortMode.DueDate:
                sortedCards = cards.OrderBy(card => card.DueDate);
                break;

            default:
                sortedCards = cards;
                break;
        }

        if (IsSortDescending)
        {
            sortedCards = sortedCards.Reverse();
        }

        return sortedCards;
    }
    public IEnumerable<CardItem> ToDoCards
    {
        get
        {
            return SortCards(Cards.Where(card => card.Status == CardStatus.ToDo));
        }

    }
    public IEnumerable<CardItem> DoingCards
    {
        get 
        {
            return SortCards(Cards.Where(card => card.Status == CardStatus.Doing));
        }
    }
    public IEnumerable<CardItem> DoneCards
    {
        get
        {
            return SortCards(Cards.Where(card => card.Status == CardStatus.Done));
        }
    }
    
    


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
                _tagsText = string.Join(", ", _selectedCard.Tags);
            }
            else
            {
                _tagsText = string.Empty;
            }
            OnPropertyChanged(nameof(TagsText));

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
    public ICommand ChangeSortDirectionCommand { get; }
    public ICommand ChangeDarkmodeCommand { get; }


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

            if (_selectedCard is not null)
            {
                SelectedCard.Tags = value
                    .Split(',')
                    .Select(tag => tag.Trim())
                    .Where(tag => !string.IsNullOrWhiteSpace(tag))
                    .ToList();
            }
        }
    }

    public bool IsSortDescending
    {
        get => _isSortDescending;
        set
        {
            if (_isSortDescending == value) return;
            _isSortDescending = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(SortDirectionIcon));
        }
    }

    public string SortDirectionIcon
    {
        get
        {
            if (IsSortDescending)
            {
                return "/icons/caret-down-fill.png";
            }
            else
            {
                return "/icons/caret-up-fill.png";
            }
        }
    }

    public CardSortMode SelectedSortMode
    {
        get => _selectedSortMode;
        set
        {
            if(_selectedSortMode  == value) 
                return;

            _selectedSortMode = value;
            
            OnPropertyChanged();
            RefreshBoardColumns();
        }
    }

    public bool IsDarkmodeActive
    {
        get => _isDarkmodeActive;
        set
        {
            if (_isDarkmodeActive == value) return;
            _isDarkmodeActive = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(DarkmodeIcon));
        }
    }

    public string DarkmodeIcon
    {
        get
        {
            if (IsDarkmodeActive)
            {
                return "/icons/sun.png";
            }
            else
            {
                return "/icons/moon-stars.png";
            }
        }
    }


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
        ChangeSortDirectionCommand = new RelayCommand(execute => ChangeSortDirection());
        ChangeDarkmodeCommand = new RelayCommand(execute => ChangeDarkmode());

        _autoSaveTimer = new DispatcherTimer();
        _autoSaveTimer.Interval = TimeSpan.FromSeconds(1);
        _autoSaveTimer.Tick += AutoSaveTimer_Tick;
    }



    private void CreateCard()
    {
        var card = new CardItem();
        Cards.Add(card);
        RefreshBoardColumns();
        SelectedCard = card;
        SaveCurrentBoard();
    }

    private void DeleteCard()
    {
        if (SelectedCard != null)
        {
            Cards.Remove(SelectedCard);
            RefreshBoardColumns();
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
        RefreshBoardColumns();
        ScheduleAutoSave();        
    }

    private void RefreshBoardColumns()
    {
        OnPropertyChanged(nameof(ToDoCards));
        OnPropertyChanged(nameof(DoingCards));
        OnPropertyChanged(nameof(DoneCards));
    }

    private void ChangeSortDirection()
    {
        IsSortDescending = !IsSortDescending;
        RefreshBoardColumns();
    }

    private void ChangeDarkmode()
    {
        IsDarkmodeActive = !IsDarkmodeActive;
    }
}