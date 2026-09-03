using System.Text.Json;
using System.Globalization;
using Microsoft.Data.Sqlite;
using KanbanBoard.Core.Models;
using System.Reflection.Metadata.Ecma335;


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
                    CREATE TABLE IF NOT EXISTS Cards 
                    (
                        Id TEXT PRIMARY KEY,
                        Title TEXT NOT NULL,
                        Status TEXT NOT NULL,
                        Description TEXT,
                        Tags TEXT,
                        CreatedAt TEXT NOT NULL,
                        UpdatedAt TEXT NOT NULL,
                        DueDate TEXT
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
                        Id,
                        Title,
                        Status,
                        Description,
                        Tags,
                        CreatedAt,
                        UpdatedAt,
                        DueDate
                        FROM Cards;";

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
}