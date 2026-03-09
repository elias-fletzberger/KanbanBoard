
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KanbanBoard.Core.Models;


/// <summary>
/// Represents a single task card on the Kanban board.
/// Contains the domain data such as title, status, description,
/// tags and timestamps.
/// </summary>
public class CardItem : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


    private string _title;
    private CardStatus _status;
    private string? _description;
    private List<string> _tags;
    private DateTime _updatedAt;
    private DateTime _dueDate;
    


    public Guid Id { get; }
    public string Title
    {
        get => _title;
        set
        {
            if (_title == value)
                return;

            _title = value;

            UpdatedAt = DateTime.Now;
            OnPropertyChanged();
            OnPropertyChanged(nameof(UpdatedAt));
        }
    }
    public CardStatus Status
    {
        get => _status;
        set
        {
            if (_status == value)
                return;

            _status = value;

            UpdatedAt = DateTime.Now;
            OnPropertyChanged();
            OnPropertyChanged(nameof(UpdatedAt));
        }
    }
    public string? Description
    {
        get => _description;
        set
        {
            if (_description == value)
                return;

            _description = value;

            UpdatedAt = DateTime.Now;
            OnPropertyChanged();
            OnPropertyChanged(nameof(UpdatedAt));
        }
    }
    public List<string> Tags
    {
        get => _tags;
        set
        {
            if (_tags == value)
                return;

            _tags = value;

            UpdatedAt = DateTime.Now;
            OnPropertyChanged();
            OnPropertyChanged(nameof(UpdatedAt));
        }
    }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt
    {
        get => _updatedAt;
        set => _updatedAt = value;
    }
    public DateTime DueDate
    {
        get => _dueDate;
        set
        {
            if (_dueDate == value)
                return;

            _dueDate = value;

            UpdatedAt = DateTime.Now;
            OnPropertyChanged();
            OnPropertyChanged(nameof(UpdatedAt));
        }
    }


    public CardItem(string title)
    {
        Id = Guid.NewGuid();
        Title = title;
        Status = CardStatus.ToDo;
        Tags = new List<string>();
        CreatedAt = DateTime.Now;
        UpdatedAt = CreatedAt;
        DueDate = CreatedAt;
    }
}
