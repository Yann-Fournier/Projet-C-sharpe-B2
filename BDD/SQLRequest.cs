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
    
    // SELECT User ----------------------------------------------------------------------------------------------------------------------
    public static String SelectUser(SQLiteConnection connection, string query)
    {
        String response = "Id, Name, Login_info, Address, Photo, Commands, Cart, Invoices, Prefer_payment, Rating\n";
        SQLiteCommand command = new SQLiteCommand(query, connection);
        SQLiteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            // Traitement des résultats de la requête SELECT
            String s = $"{reader["Id"]}, {reader["Name"]}, {reader["Login_info"]}, {reader["Address"]}, {reader["Photo"]}, {reader["Commands"]}, {reader["Cart"]}, {reader["Invoices"]}, {reader["Prefer_payment"]}, {reader["Rating"]}\n";
            response = response + s;
        }
        return response;
    }
    
    public static String SelectUserById(SQLiteConnection connection, string query)
    {
        String response = "";
        SQLiteCommand command = new SQLiteCommand(query, connection);
        SQLiteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            // Traitement des résultats de la requête SELECT
            String s = $"Id: {reader["Id"]}, Name: {reader["Name"]}, Login_info: {reader["Login_info"]}, Address: {reader["Address"]}, Photo: {reader["Photo"]}, Commands: {reader["Commands"]}, Cart: {reader["Cart"]}, Invoices: {reader["Invoices"]}, Prefer_payment: {reader["Prefer_payment"]}, Rating: {reader["Rating"]}\n";
            response = response + s;
        }
        return response;
    }
    
    public static String SelectUserByName(SQLiteConnection connection, string query)
    {
        String response = "";
        SQLiteCommand command = new SQLiteCommand(query, connection);
        SQLiteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            // Traitement des résultats de la requête SELECT
            String s = $"Id: {reader["Id"]}, Name: {reader["Name"]}, Login_info: {reader["Login_info"]}, Address: {reader["Address"]}, Photo: {reader["Photo"]}, Commands: {reader["Commands"]}, Cart: {reader["Cart"]}, Invoices: {reader["Invoices"]}, Prefer_payment: {reader["Prefer_payment"]}, Rating: {reader["Rating"]}\n";
            response = response + s;
        }
        return response;
    }
    
    // SELECT Items -----------------------------------------------------------------------------------------------------------------
    public static String SelectItems(SQLiteConnection connection, string query)
    {
        String response = "Id, Name, Price, Description, Photo, Category, Seller, Rating\n";
        SQLiteCommand command = new SQLiteCommand(query, connection);
        SQLiteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            // Traitement des résultats de la requête SELECT
            String s = $"{reader["Id"]}, {reader["Name"]}, {reader["Price"]}, {reader["Description"]}, {reader["Photo"]}, {reader["Category"]}, {reader["Seller"]}, {reader["Rating"]}\n";
            response = response + s;
        }
        return response;
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

// try
// {
//     // Exemple de requête SELECT ALL
//     SQLRequest.SelectAllLogin_info(connection, "SELECT * FROM Login_info");
// }
// catch (Exception ex)
// {
//     Console.WriteLine("Wrong request: " + ex.Message);
// }

// try
// {
//     // Fermer la connection avec la base de données
//     connection.Close();
// }
// catch (Exception e)
// {
//     Console.WriteLine("Impossible to close connection: " + e.Message);
//     throw;
// }