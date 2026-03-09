
using KanbanBoard.Core.Models;
using Xunit;

namespace KanbanBoard.Tests;


public class CardItemTests
{
    [Fact]
    public void Constructor_Should_Set_Default_Values()
    {
        var title = "Test Aufgabe";

        var card = new CardItem(title);

        Assert.NotEqual(Guid.Empty, card.Id);
        Assert.Equal(title, card.Title);
        Assert.Equal(CardStatus.ToDo, card.Status);
        Assert.NotNull(card.Tags);
        Assert.True(card.CreatedAt <= DateTime.Now);
    }
}