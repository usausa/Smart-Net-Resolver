namespace Example.WebApplication;

using Microsoft.Data.Sqlite;

using Smart.Data.Mapper;

public static class DatabaseInitializer
{
    public static void SetupMasterDatabase(string connectionString)
    {
        using var con = new SqliteConnection(connectionString);
        con.Execute("CREATE TABLE IF NOT EXISTS Item (Id int PRIMARY KEY, Name text, Price int)");
        con.Execute("DELETE FROM Item");
        con.Execute("INSERT INTO Item (Id, Name, Price) VALUES (1, 'Item-1', 100)");
        con.Execute("INSERT INTO Item (Id, Name, Price) VALUES (2, 'Item-2', 200)");
        con.Execute("INSERT INTO Item (Id, Name, Price) VALUES (3, 'Item-3', 300)");
    }

    public static void SetupCharacterDatabase(string connectionString)
    {
        using var con = new SqliteConnection(connectionString);
        con.Execute("CREATE TABLE IF NOT EXISTS Character (Id int PRIMARY KEY, Name text, Level int)");
        con.Execute("DELETE FROM Character");
        con.Execute("INSERT INTO Character (Id, Name, Level) VALUES (1, 'Character-1', 43)");
        con.Execute("INSERT INTO Character (Id, Name, Level) VALUES (2, 'Character-2', 65)");
        con.Execute("INSERT INTO Character (Id, Name, Level) VALUES (3, 'Character-3', 27)");
    }
}
