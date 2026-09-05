using System.Globalization;
using System.Text.Json;
using Microsoft.Data.Sqlite;
using KanbanBoard.Core.Models;

namespace KanbanBoard.Infrastructure.Persistence;


/// <summary>
/// SQLite-based implementation for persisting board data
/// in the user's application data directory.
/// </summary>
public class SqliteBoardRepository
{
    private static readonly string _folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "KanbanBoard");
    private readonly string _filePath = Path.Combine(_folderPath, "kanbanboard.db");


    public SqliteBoardRepository()
    {
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        try
        {
            Directory.CreateDirectory(_folderPath);
            string connectionString = $"Data Source={_filePath}";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string sqlCommand = @"
                    CREATE TABLE IF NOT EXISTS cards 
                    (
                        id TEXT PRIMARY KEY,
                        title TEXT NOT NULL,
                        status TEXT NOT NULL,
                        description TEXT,
                        tags TEXT,
                        createdAt TEXT NOT NULL,
                        updatedAt TEXT NOT NULL,
                        dueDate TEXT
                    );";

                using (var command = new SqliteCommand(sqlCommand, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"SQLite initialization error: {ex.Message}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"SQLite initialization error: {ex.Message}");
        }
    }

    public Board Load()
    {
        string connectionString = $"Data Source={_filePath}";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string sqlQuery = @"
                    SELECT
                        id,
                        title,
                        status,
                        description,
                        tags,
                        createdAt,
                        updatedAt,
                        dueDate
                        FROM cards;";

            using (var query = new SqliteCommand(sqlQuery, connection))
            {
                using (var reader = query.ExecuteReader())
                {
                    Board board = new Board();

                    while (reader.Read())
                    {
                        string idString = reader.GetString(0);
                        if (!Guid.TryParse(idString, out Guid id))
                        {
                            Console.WriteLine($"Invalid Guid: {idString}");
                            continue;
                        }

                        string title = reader.GetString(1);

                        string statusString = reader.GetString(2);
                        if(!Enum.TryParse(statusString, out CardStatus status) || !Enum.IsDefined(status))
                        {
                            Console.WriteLine($"Invalid CardStatus: {statusString}");
                            continue;
                        }

                        string description;
                        if (!reader.IsDBNull(3)) description = reader.GetString(3);
                        else description = "";

                        List<string> tags;
                        if (!reader.IsDBNull(4))
                        {
                            string tagsString = reader.GetString(4);
                            try
                            {
                                tags = JsonSerializer.Deserialize<List<string>?>(tagsString) ?? new List<string>();
                            }
                            catch (JsonException ex)
                            {
                                Console.WriteLine($"JSON-tags load error: {ex.Message}");
                                tags = new List<string>();
                            }
                        }
                        else tags = new List<string>();

                        string createdAtString = reader.GetString(5);
                        if (!DateTime.TryParseExact(createdAtString, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime createdAt))
                        {
                            Console.WriteLine($"Invalid CreatedAt-Date: {createdAtString}");
                            continue;
                        }

                        string updatedAtString = reader.GetString(6);
                        if (!DateTime.TryParseExact(updatedAtString, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime updatedAt))
                        {
                            Console.WriteLine($"Invalid UpdatedAt-Date: {updatedAtString}");
                            continue;
                        }

                        DateTime? dueDate = null;
                        if (!reader.IsDBNull(7))
                        {
                            string dueDateString = reader.GetString(7);
                            if (DateTime.TryParseExact(dueDateString, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDueDate))
                            {
                                dueDate = parsedDueDate;
                            }
                            else Console.WriteLine($"Invalid DueDate: {dueDateString}");
                        }


                        CardItem card = new CardItem(
                            id,
                            title,
                            status,
                            description, 
                            tags, 
                            createdAt, 
                            updatedAt, 
                            dueDate);

                        board.Cards.Add(card);
                    }

                    return board;
                }
            }
        }
    }

    public void SaveAll(Board board)
    {
        try
        {
            Directory.CreateDirectory(_folderPath);
            string connectionString = $"Data Source={_filePath}";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    string sqlDeleteCommand = @"
                    DELETE FROM cards;";

                    string sqlInsertCommand = @"
                    INSERT INTO cards
                    (
                        id,
                        title,
                        status,
                        description,
                        tags,
                        createdAt,
                        updatedAt,
                        dueDate
                    )
                    VALUES
                    (
                        @id,
                        @title,
                        @status,
                        @description,
                        @tags,
                        @createdAt,
                        @updatedAt,
                        @dueDate
                    );";

                    using (var command = new SqliteCommand(sqlDeleteCommand, connection))
                    {
                        command.Transaction = transaction;
                        command.ExecuteNonQuery();
                    }

                    using (var command = new SqliteCommand(sqlInsertCommand, connection))
                    {
                        command.Transaction = transaction;

                        var idParameter = command.Parameters.Add("@id", SqliteType.Text);
                        var titleParameter = command.Parameters.Add("@title", SqliteType.Text);
                        var statusParameter = command.Parameters.Add("@status", SqliteType.Text);
                        var descriptionParameter = command.Parameters.Add("@description", SqliteType.Text);
                        var tagsParameter = command.Parameters.Add("@tags", SqliteType.Text);
                        var createdAtParameter = command.Parameters.Add("@createdAt", SqliteType.Text);
                        var updatedAtParameter = command.Parameters.Add("@updatedAt", SqliteType.Text);
                        var dueDateParameter = command.Parameters.Add("@dueDate", SqliteType.Text);

                        foreach (var card in board.Cards)
                        {
                            idParameter.Value = card.Id.ToString();

                            titleParameter.Value = card.Title;

                            statusParameter.Value = card.Status.ToString();

                            if (card.Description is not null) descriptionParameter.Value = card.Description;
                            else descriptionParameter.Value = DBNull.Value;

                            if (card.Tags is not null) tagsParameter.Value = JsonSerializer.Serialize(card.Tags);
                            else tagsParameter.Value = DBNull.Value;

                            createdAtParameter.Value = card.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);

                            updatedAtParameter.Value= card.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);

                            if (card.DueDate.HasValue)
                                dueDateParameter.Value = card.DueDate.Value.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);

                            else dueDateParameter.Value= DBNull.Value;

                            command.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                }
            }
        }
        catch(SqliteException ex)
        {
            Console.WriteLine($"SQLite save error: {ex.Message}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"SQLite save error: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"No permission to write file: {ex.Message}");
        }
    }
}