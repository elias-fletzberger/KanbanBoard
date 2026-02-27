using KanbanBoard.Core.Models;
using Xunit;


namespace KanbanBoard.Tests;

public class BoardTests
{
    [Fact]
    public void New_Board_Should_Start_With_Empty_List()
    {
        var board = new Board();

        Assert.NotNull(board.Cards);
        Assert.Empty(board.Cards);
    }

    [Fact]
    public void Should_Be_Able_To_Add_Card_To_Board()
    {
        var board = new Board();
        var card = new CardItem("Test Aufgabe");

        board.Cards.Add(card);

        Assert.Single(board.Cards);
    }
}
