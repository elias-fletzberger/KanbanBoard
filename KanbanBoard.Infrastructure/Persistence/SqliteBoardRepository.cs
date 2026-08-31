using Microsoft.Data.Sqlite;


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
                        Description TEXT,
                        Status TEXT NOT NULL,
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
}