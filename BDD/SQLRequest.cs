﻿using System.Data.SQLite;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography;
using System.Text;

namespace BDD;

public class SQLRequest
{
    // Reset Database --------------------------------------------------------------------------------------------------------------------------
    public static void CreateDatabaseFile()
    {
        string scriptFilePath = @"..\\..\\..\\BDD\\script.sql";
        string databaseFilePath = @"..\\..\\..\\BDD\\database.sqlite";

        if (File.Exists(scriptFilePath) && File.Exists(databaseFilePath))
        {
            string script = File.ReadAllText(scriptFilePath);

            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={databaseFilePath};Version=3;"))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(script, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            Console.WriteLine("Base de données créée avec succès.");
        }
        else
        {
            Console.WriteLine("Le fichier SQL n'existe pas.");
        }
    }
    
    // Connection à la base de données ------------------------------------------------------------------------------------------------------
    public static SQLiteConnection OpenSqLiteConnection()
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
    
    // Get Auth Token -------------------------------------------------------------------------------------------------
    public static string GetToken(SQLiteConnection connection, string query)
    {
        String response = "";
        SQLiteCommand command = new SQLiteCommand(query, connection);
        SQLiteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            // Traitement des résultats de la requête SELECT
            String s = $"{reader["Token"]}\n";
            response = response + s;
        }
        return response;
    }
    
    public static string GetIdFromToken(SQLiteConnection connection, string query)
    {
        string response = "";
        SQLiteCommand command = new SQLiteCommand(query, connection);
        SQLiteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            // Traitement des résultats de la requête SELECT
            string s = $"{reader["Id"]}";
            response = response + s;
        }
        return response;
    }
        
    // SELECT User ----------------------------------------------------------------------------------------------------------------------
    public static string SelectUserInfo(SQLiteConnection connection, string query)
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
    
    // SELECT Items -----------------------------------------------------------------------------------------------------------------
    public static string SelectItems(SQLiteConnection connection, string query, int perso)
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

        if (perso == 0 && response == "Id, Name, Price, Description, Photo, Category, Seller, Rating\n")
        {
            return "They are no items corresponding to your research.";
        }
        if (perso == 1 && response == "Id, Name, Price, Description, Photo, Category, Seller, Rating\n")
        {
            return "You sell no items.";
        }
        return response;
    }
    
    // SELECT Commands -----------------------------------------------------------------------------------------------------------------
    public static string SelectCommands(SQLiteConnection connection, string query)
    {
        String response = "Id of your previous command:\n";
        SQLiteCommand command = new SQLiteCommand(query, connection);
        SQLiteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            // Traitement des résultats de la requête SELECT
            String s = $"{reader["Command"]}\n";
            response = response + s;
        }
        if (response == "Id of your previous command:\n\n")
        {
            return "You don't have command.";
        }
        return response;
    }
    
    // SELECT Cart -----------------------------------------------------------------------------------------------------------------
    public static string SelectCart(SQLiteConnection connection, string query)
    {
        String response = "Id of the items in your cart:\n";
        SQLiteCommand command = new SQLiteCommand(query, connection);
        SQLiteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            // Traitement des résultats de la requête SELECT
            String s = $"{reader["Items"]}\n";
            response = response + s;
        }
        if (response == "Id of the items in your cart:\n\n")
        {
            return "Your cart is empty.";
        }
        return response;
    }
    
    // SELECT Invoices -----------------------------------------------------------------------------------------------------------------
    public static string SelectInvoice(SQLiteConnection connection, string query)
    {
        String response = "Id of your previous invoices:\n";
        SQLiteCommand command = new SQLiteCommand(query, connection);
        SQLiteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            // Traitement des résultats de la requête SELECT
            String s = $"{reader["Invoice"]}\n";
            response = response + s;
        }

        if (response == "Id of your previous invoices:\n\n")
        {
            return "You don't have invoice.";
        }
        return response;
    }
    
    // SELECT Category -----------------------------------------------------------------------------------------------------------------
    public static string SelectCategory(SQLiteConnection connection, string query)
    {
        String response = "Id, Name";
        SQLiteCommand command = new SQLiteCommand(query, connection);
        SQLiteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            // Traitement des résultats de la requête SELECT
            String s = $"{reader["Id"]}, {reader["Name"]}\n";
            response = response + s;
        }
        return response;
    }
    
    // SELECT Address -----------------------------------------------------------------------------------------------------------------
    public static string SelectAddress(SQLiteConnection connection, string query)
    {
        String response = "Street, City, CP, State, Country";
        SQLiteCommand command = new SQLiteCommand(query, connection);
        SQLiteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            // Traitement des résultats de la requête SELECT
            response = $"Street: {reader["Street"]},\nCity: {reader["City"]},\nCP: {reader["CP"]},\nState: {reader["State"]},\nCountry: {reader["Country"]}\n";
        }
        return response;
    }
    
    // Insert User -----------------------------------------------------------------------------------------------------------------
    public static String InsertUser(SQLiteConnection connection, NameValueCollection parameters)
    {
        // Récupération des Id avec un COUNT() 
        int countUser = GetMaxId(connection, "User", "Id") + 1;
        int countPhoto = GetMaxId(connection, "Photo", "Id") + 1;
        int countRating = GetMaxId(connection, "Rating", "Id") + 1 ;
        
        // Querys
        string queryUser = "INSERT INTO User (Id, Name, Login_info, Address, Photo,Commands, Cart, Invoices, Prefer_payment, Rating) VALUES (@Val1, @Val2, @Val3, @Val4, @Val5, @Val6, @Val7, @Val8, @Val9, @Val10)";
        string queryLoginInfo = "INSERT INTO Login_info (Id, mail, Password) VALUES (@Val1, @Val2, @Val3)";
        string queryAddress = "INSERT INTO Address (Id, Street, City, CP, State, Country) VALUES (@Val1, @Val2, @Val3, @Val4, @Val5, @Val6)";
        string queryPhoto = "INSERT INTO Photo (Id, Link) VALUES (@Val1, @Val2)";
        string queryCommands = "INSERT INTO Commands (Id) VALUES (@Val1)";
        string queryCart = "INSERT INTO Cart (Id) VALUES (@Val1)";
        string queryInvoices = "INSERT INTO Invoices (Id) VALUES (@Val1)";
        string queryPreferPayement = "INSERT INTO Prefer_payment (Id, Payment) VALUES (@Val1, @Val2)";
        string queryRating = "INSERT INTO Rating (Id, Rating, Comment) VALUES (@Val1, @Val2, @Val3)";
        string queryAuth = "INSERT INTO Auth (Id, Token) VALUES (@Val1, @Val2)";
        
        // Execution des Querys
        using (SQLiteCommand command = new SQLiteCommand(queryLoginInfo, connection))
        {
            // Ajout des paramètres avec leurs valeurs
            command.Parameters.AddWithValue("@Val1", countUser);
            command.Parameters.AddWithValue("@Val2", parameters["email"]);
            command.Parameters.AddWithValue("@Val3", HashPwd(parameters["password"]));
            int rowsAffected = command.ExecuteNonQuery(); // Exécution de la commande SQL
        }
        using (SQLiteCommand command = new SQLiteCommand(queryAddress, connection))
        {
            // Ajout des paramètres avec leurs valeurs
            command.Parameters.AddWithValue("@Val1", countUser);
            command.Parameters.AddWithValue("@Val2", "");
            command.Parameters.AddWithValue("@Val3", "");
            command.Parameters.AddWithValue("@Val4", 2);
            command.Parameters.AddWithValue("@Val5", "");
            command.Parameters.AddWithValue("@Val6", "");
            int rowsAffected = command.ExecuteNonQuery(); // Exécution de la commande SQL
        }
        
        using (SQLiteCommand command = new SQLiteCommand(queryPhoto, connection))
        {
            // Ajout des paramètres avec leurs valeurs
            command.Parameters.AddWithValue("@Val1", countPhoto);
            command.Parameters.AddWithValue("@Val2", "");
            int rowsAffected = command.ExecuteNonQuery(); // Exécution de la commande SQL
        }
        
        using (SQLiteCommand command = new SQLiteCommand(queryCommands, connection))
        {
            // Ajout des paramètres avec leurs valeurs
            command.Parameters.AddWithValue("@Val1", countUser);
            // command.Parameters.AddWithValue("@Val2", 0);
            int rowsAffected = command.ExecuteNonQuery(); // Exécution de la commande SQL
        }
        
        using (SQLiteCommand command = new SQLiteCommand(queryCart, connection))
        {
            // Ajout des paramètres avec leurs valeurs
            command.Parameters.AddWithValue("@Val1", countUser);
            // command.Parameters.AddWithValue("@Val2", 0);
            int rowsAffected = command.ExecuteNonQuery(); // Exécution de la commande SQL
        }
        
        using (SQLiteCommand command = new SQLiteCommand(queryInvoices, connection))
        {
            // Ajout des paramètres avec leurs valeurs
            command.Parameters.AddWithValue("@Val1", countUser);
            // command.Parameters.AddWithValue("@Val2", 0);
            int rowsAffected = command.ExecuteNonQuery(); // Exécution de la commande SQL
        }
        
        using (SQLiteCommand command = new SQLiteCommand(queryPreferPayement, connection))
        {
            // Ajout des paramètres avec leurs valeurs
            command.Parameters.AddWithValue("@Val1", countUser);
            command.Parameters.AddWithValue("@Val2", 0);
            int rowsAffected = command.ExecuteNonQuery(); // Exécution de la commande SQL
        }
        
        using (SQLiteCommand command = new SQLiteCommand(queryRating, connection))
        {
            // Ajout des paramètres avec leurs valeurs
            command.Parameters.AddWithValue("@Val1", countRating);
            command.Parameters.AddWithValue("@Val2", 3);
            command.Parameters.AddWithValue("@Val3", 0);
            int rowsAffected = command.ExecuteNonQuery(); // Exécution de la commande SQL
        }
        
        using (SQLiteCommand command = new SQLiteCommand(queryUser, connection))
        {
            // Ajout des paramètres avec leurs valeurs
            command.Parameters.AddWithValue("@Val1",countUser);
            command.Parameters.AddWithValue("@Val2",parameters["name"]);
            command.Parameters.AddWithValue("@Val3", countUser);
            command.Parameters.AddWithValue("@Val4", countUser);
            command.Parameters.AddWithValue("@Val5", countPhoto);
            command.Parameters.AddWithValue("@Val6", countUser);
            command.Parameters.AddWithValue("@Val7", countUser);
            command.Parameters.AddWithValue("@Val8", countUser);
            command.Parameters.AddWithValue("@Val9", 3);
            command.Parameters.AddWithValue("@Val10", countRating);
            int rowsAffected = command.ExecuteNonQuery();// Exécution de la commande SQL
        }
        
        using (SQLiteCommand command = new SQLiteCommand(queryAuth, connection))
        {
            // Ajout des paramètres avec leurs valeurs
            command.Parameters.AddWithValue("@Val1", countUser);
            command.Parameters.AddWithValue("@Val2", HashToken(parameters["password"]));
            int rowsAffected = command.ExecuteNonQuery(); // Exécution de la commande SQL
        }
        
        return "Cette utilisateur à bien été ajouter";
    }
    
    // Insert Items -----------------------------------------------------------------------------------------------------------------
    public static String InsertItem(SQLiteConnection connection, NameValueCollection parameters, int User_Id)
    {
        // Récupération des Id avec un COUNT() et une autre requete 
        int countItems = GetMaxId(connection, "Items", "Id") + 1;
        int countPhoto = GetMaxId(connection, "Photo", "Id") + 1 ;
        int countRating = GetMaxId(connection, "Rating", "Id") + 1 ;
        
        // Querys
        string queryItems = "INSERT INTO Items (Id, Name, Price, Description, Photo, Category, Seller, Rating) VALUES (@Val1, @Val2, @Val3, @Val4, @Val5, @Val6, @Val7, @Val8)";
        string queryRating = "INSERT INTO Rating (Id, Rating, Comment) VALUES (@Val1, @Val2, @Val3)";
        string queryPhoto = "INSERT INTO Photo (Id, Link) VALUES (@Val1, @Val2)";
        
        // Execution des Querys
        using (SQLiteCommand command = new SQLiteCommand(queryPhoto, connection))
        {
            // Ajout des paramètres avec leurs valeurs
            command.Parameters.AddWithValue("@Val1", countPhoto);
            command.Parameters.AddWithValue("@Val2", "");
            int rowsAffected = command.ExecuteNonQuery(); // Exécution de la commande SQL
        }
        using (SQLiteCommand command = new SQLiteCommand(queryRating, connection))
        {
            // Ajout des paramètres avec leurs valeurs
            command.Parameters.AddWithValue("@Val1", countRating);
            command.Parameters.AddWithValue("@Val2", 3);
            command.Parameters.AddWithValue("@Val3", 0);
            int rowsAffected = command.ExecuteNonQuery(); // Exécution de la commande SQL
        }
        
        using (SQLiteCommand command = new SQLiteCommand(queryItems, connection))
        {
            // Ajout des paramètres avec leurs valeurs
            command.Parameters.AddWithValue("@Val1", countItems);
            command.Parameters.AddWithValue("@Val2", parameters["name"]);
            command.Parameters.AddWithValue("@Val3", parameters["price"]);
            command.Parameters.AddWithValue("@Val4", parameters["description"]);
            command.Parameters.AddWithValue("@Val5", countPhoto);
            command.Parameters.AddWithValue("@Val6", parameters["category"]);
            command.Parameters.AddWithValue("@Val7", User_Id);
            command.Parameters.AddWithValue("@Val8", countRating);
            int rowsAffected = command.ExecuteNonQuery(); // Exécution de la commande SQL
        }
        return "Your product has been add.";
    }
    
    // Insert Address -------------------------------------------------------------------------------------------------------
    public static void InsertAddress(SQLiteConnection connection, string query)
    {
        Console.WriteLine(query);
        SQLiteCommand command = new SQLiteCommand(query, connection);
        int rowsAffected = command.ExecuteNonQuery(); // Exécution de la commande SQL
    }
    
    // Update Username ----------------------------------------------------------------------------------------------------------------------
    public static void UpdateUsername(SQLiteConnection connection, string query)
    {
        SQLiteCommand command = new SQLiteCommand(query, connection);
        SQLiteDataReader readerUser = command.ExecuteReader();
    }
    
    // Update User Photo ----------------------------------------------------------------------------------------------------------------------
    public static string UpdateUserPhoto(SQLiteConnection connection, string newPicture, int UserId)
    {
        // Recuperer Id Photo puis modifier
        int idPhoto = 0;
        string name = "";
        SQLiteCommand command = new SQLiteCommand("SELECT Photo, Name FROM User WHERE Id = '" + UserId + "';", connection);
        SQLiteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            // Traitement des résultats de la requête SELECT
            idPhoto = Convert.ToInt32(reader[0]);
            name = Convert.ToString(reader[1]);
        }

        SQLiteCommand update = new SQLiteCommand("UPDATE Photo SET Link = '" + newPicture + "' WHERE Id = " + idPhoto + ";", connection);
        SQLiteDataReader readerUpdate = update.ExecuteReader();
        return $"La photo de {name} à bien été mise à jour.";
    }
    
    // Update Item --------------------------------------------------------------------------------------------------------
    public static void UpdateItem(SQLiteConnection connection, string query)
    {
        SQLiteCommand command = new SQLiteCommand(query, connection);
        SQLiteDataReader readerUser = command.ExecuteReader();
    }
    
    // Update Item Photo ---------------------------------------------------------------------------------------------------
    public static void UpdateItemPhoto(SQLiteConnection connection, NameValueCollection parameters)
    {
        // Recuperer Id Photo puis modifier
        int idPhoto = 0;
        SQLiteCommand command = new SQLiteCommand("SELECT Photo FROM Items WHERE Id = '" + parameters["item_id"] + "';", connection);
        SQLiteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            // Traitement des résultats de la requête SELECT
            idPhoto = Convert.ToInt32(reader[0]);
        }

        SQLiteCommand update = new SQLiteCommand("UPDATE Photo SET Link = '" + parameters["picture"] + "' WHERE Id = " + idPhoto + ";", connection);
        SQLiteDataReader readerUpdate = update.ExecuteReader();
    }

    // Update Cart ----------------------------------------------------------------------------------------------------------------------
    public static string UpdateCart(SQLiteConnection connection, NameValueCollection parameters, int User_Id)
    {
        // Récupération de l'id du panier lié à l'utilisateur
        int idCart = 0;
        SQLiteCommand command = new SQLiteCommand("SELECT Cart FROM User WHERE Id = " + User_Id + ";", connection);
        SQLiteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            // Traitement des résultats de la requête SELECT
            idCart = Convert.ToInt32(reader[0]);
        }

        // Execution de la requête en fonction de l'option placé en paramètre.
        if (parameters["option"] == "add")
        {
            using (SQLiteCommand insert = new SQLiteCommand("INSERT INTO Cart (Id, Items) VALUES (@Val1, @Val2)", connection))
            {
                // Ajout des paramètres avec leurs valeurs
                insert.Parameters.AddWithValue("@Val1", idCart);
                insert.Parameters.AddWithValue("@Val2", Convert.ToInt32(parameters["item_id"]));
                int rowsAffected = insert.ExecuteNonQuery(); // Exécution de la commande SQL
            }
            return $"The item has been add successfully to your cart.";
        } 
        else if (parameters["option"] == "del")
        {
            SQLiteCommand insert = new SQLiteCommand("DELETE FROM Cart WHERE Id = " + idCart + " AND Items = " + Convert.ToInt32(parameters["item_id"]) + ";" , connection);
            int rowsAffected = insert.ExecuteNonQuery(); // Exécution de la commande SQL
            return $"The item has been delete successfully to your cart.";
        }
        return "404 - Not Found";
    }
    
    // DELETE User ----------------------------------------------------------------------------------------------------------------------
    public static void DeleteUser(SQLiteConnection connection, int UserId)
    {
        // Recupération des foreign key
        int idPhoto = 0;
        int idRating = 0;
        SQLiteCommand command = new SQLiteCommand("SELECT Photo, Rating FROM User WHERE Id = " + UserId + ";", connection);
        SQLiteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            // Traitement des résultats de la requête SELECT
            idPhoto = Convert.ToInt32(reader[0]);
            idRating = Convert.ToInt32(reader[1]);
        }
        
        SQLiteCommand delUser = new SQLiteCommand("DELETE FROM User WHERE Id = " + UserId + ";", connection);
        SQLiteCommand delLogin = new SQLiteCommand("DELETE FROM Login_info WHERE Id = " + UserId + ";", connection);
        SQLiteCommand delAddress = new SQLiteCommand("DELETE FROM Address WHERE Id = " + UserId + ";", connection);
        SQLiteCommand delPhoto = new SQLiteCommand("DELETE FROM Photo WHERE Id = " + idPhoto + ";", connection);
        SQLiteCommand delCommands = new SQLiteCommand("DELETE FROM Commands WHERE Id = " + UserId + ";", connection);
        SQLiteCommand delCart = new SQLiteCommand("DELETE FROM Cart WHERE Id = " + UserId + ";", connection);
        SQLiteCommand delInvoices = new SQLiteCommand("DELETE FROM Invoices WHERE Id = " + UserId + ";", connection);
        SQLiteCommand delRating = new SQLiteCommand("DELETE FROM Rating WHERE Id = " + idRating + ";", connection);
        
        DeleteItem(connection, UserId, false);
        int Rows = delUser.ExecuteNonQuery();
        Rows = delLogin.ExecuteNonQuery();
        Rows = delAddress.ExecuteNonQuery();
        Rows = delPhoto.ExecuteNonQuery();
        Rows = delCommands.ExecuteNonQuery();
        Rows = delCart.ExecuteNonQuery();
        Rows = delInvoices.ExecuteNonQuery();
        Rows = delRating.ExecuteNonQuery();
    }
    
    // Delete Item ----------------------------------------------------------------------------------------------------------------------
    public static void DeleteItem(SQLiteConnection connection, int Id, bool IdItem)
    {
        // Faire une select puis delete les Items dans Cart puis l'item lui-même puis sa Photo, puis son Rating. Dans cette ordre
        // Cart -> Item -> Photo -> Cart.
        int idPhoto = 0;
        int idRating = 0;
        int Rows = 0;
        if (IdItem) // delete item because of the delete account of the seller
        {
            SQLiteCommand commandItem = new SQLiteCommand("SELECT Photo, Rating FROM Items WHERE Id = '" + Id + "';", connection);
            SQLiteDataReader readerItem = commandItem.ExecuteReader();
            while (readerItem.Read())
            {
                // Traitement des résultats de la requête SELECT
                idPhoto = Convert.ToInt32(readerItem[0]);
                idRating = Convert.ToInt32(readerItem[1]);
            }
            SQLiteCommand delItem = new SQLiteCommand("DELETE FROM Items WHERE Id = " + Id + ";", connection);
            Rows = delItem.ExecuteNonQuery();
        }
        else if (!IdItem) // delete order by the seller
        {
            SQLiteCommand commandItem = new SQLiteCommand("SELECT Photo, Rating FROM Items WHERE Seller = '" + Id + "';", connection);
            SQLiteDataReader readerItem = commandItem.ExecuteReader();
            while (readerItem.Read())
            {
                // Traitement des résultats de la requête SELECT
                idPhoto = Convert.ToInt32(readerItem[0]);
                idRating = Convert.ToInt32(readerItem[1]);
            }
            SQLiteCommand delItem = new SQLiteCommand("DELETE FROM Items WHERE Seller = " + Id + ";", connection);
            Rows = delItem.ExecuteNonQuery();
        }
        
        SQLiteCommand delPhoto = new SQLiteCommand("DELETE FROM Photo WHERE Id = " + idPhoto + ";", connection);
        SQLiteCommand delRating = new SQLiteCommand("DELETE FROM Rating WHERE Id = " + idRating + ";", connection);
        
        Rows = delPhoto.ExecuteNonQuery();
        Rows = delRating.ExecuteNonQuery();
    }
    
    // Tools ----------------------------------------------------------------------------------------------------------------------
    private static int GetMaxId(SQLiteConnection connection, string table, string column)
    {
        // SQLiteCommand commandUser = new SQLiteCommand("SELECT COUNT(" + column + ") AS Number0fUser FROM "+ table +";", connection);
        SQLiteCommand commandUser = new SQLiteCommand("SELECT MAX(" + column + ") AS max FROM " + table + ";", connection);
        SQLiteDataReader readerUser = commandUser.ExecuteReader();
        while (readerUser.Read())
        {
            // Traitement des résultats de la requête SELECT
            int count = Convert.ToInt32(readerUser[0]);
            return count;
        }
        return -1; // Au cas ou ça plante ...
    }

    public static String HashToken(string mdp)
    {
        using (SHA1Managed sha1 = new SHA1Managed())
        {
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(mdp));
            var sb = new StringBuilder(hash.Length * 2);

            foreach (byte b in hash)
            {
                // can be "x2" if you want lowercase
                sb.Append(b.ToString("X2"));
            }
            
            return sb.ToString();
        }
    }
    
    public static String HashPwd(string mdp)
    {
        using (SHA256Managed sha1 = new SHA256Managed())
        {
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(mdp));
            var sb = new StringBuilder(hash.Length * 2);

            foreach (byte b in hash)
            {
                // can be "x2" if you want lowercase
                sb.Append(b.ToString("X2"));
            }
            
            return sb.ToString();
        }
    }
}
