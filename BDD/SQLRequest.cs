using System.Data.SQLite;
using System.Globalization;

namespace BDD;

public class SQLRequest
{
    public static SQLiteConnection openSqLiteConnection()
    {
        // Data Source = chemin de la database
        string absolutePath = @"..\..\..\BDD\database.sqlite";
        string connectionString = $"Data Source={absolutePath};Version=3;";
        // Création de la connection
        SQLiteConnection connection = new SQLiteConnection(connectionString);
        try
        {
            // Ouverture de la connection avec la base de données
            connection.Open();
            Console.WriteLine("Connexion réussie à la base de données SQLite!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erreur de connexion: " + ex.Message);
        }
        return connection;
    }
    
    public static void SelectAllLogin_info(SQLiteConnection connection, string query)
    {
        SQLiteCommand command = new SQLiteCommand(query, connection);
        SQLiteDataReader reader = command.ExecuteReader();
        Console.WriteLine("Colonne1: Id, Colonne2: mail, Colonne3: Password");
        while (reader.Read())
        {
            // Traitement des résultats de la requête SELECT
            Console.WriteLine($"Colonne1: {reader["Id"]}, Colonne2: {reader["mail"]}, Colonne3: {reader["Password"]}");
        }
    }

    public static void ExecuteNonQuery(SQLiteConnection connection, string query)
    {
        SQLiteCommand command = new SQLiteCommand(query, connection);
        int rowsAffected = command.ExecuteNonQuery();
        Console.WriteLine($"Nombre de lignes affectées : {rowsAffected}");
    }
}