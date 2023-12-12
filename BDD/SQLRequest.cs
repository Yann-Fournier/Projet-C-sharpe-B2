using System.Data.SQLite;
using System.Collections.Specialized;
using System.Runtime.InteropServices.JavaScript;

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
    
    // SELECT Commands -----------------------------------------------------------------------------------------------------------------
    public static String SelectCommands(SQLiteConnection connection, string query)
    {
        String response = "Id, Command\n";
        SQLiteCommand command = new SQLiteCommand(query, connection);
        SQLiteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            // Traitement des résultats de la requête SELECT
            String s = $"{reader["Id"]}, {reader["Command"]}\n";
            response = response + s;
        }
        return response;
    }
    
    // SELECT Cart -----------------------------------------------------------------------------------------------------------------
    public static String SelectCart(SQLiteConnection connection, string query)
    {
        String response = "Id, Items\n";
        SQLiteCommand command = new SQLiteCommand(query, connection);
        SQLiteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            // Traitement des résultats de la requête SELECT
            String s = $"{reader["Id"]}, {reader["Items"]}\n";
            response = response + s;
        }
        return response;
    }
    
    // SELECT Cart -----------------------------------------------------------------------------------------------------------------
    public static String SelectInvoice(SQLiteConnection connection, string query)
    {
        String response = "Id, Invoice\n";
        SQLiteCommand command = new SQLiteCommand(query, connection);
        SQLiteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            // Traitement des résultats de la requête SELECT
            String s = $"{reader["Id"]}, {reader["Invoice"]}\n";
            response = response + s;
        }
        return response;
    }
    
    // SELECT Category -----------------------------------------------------------------------------------------------------------------
    public static String SelectCategory(SQLiteConnection connection, string query)
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
    // Insert User -----------------------------------------------------------------------------------------------------------------
    public static String InsertUser(SQLiteConnection connection, NameValueCollection parameters)
    {
        // Récupération des Id avec un COUNT() 
        int countUser = CountLine(connection, "User", "Id") + 9;
        int countPhoto = CountLine(connection, "Photo", "Id") + 1;
        int countRating = CountLine(connection, "Rating", "Id") + 1 ;
        
        // Querys
        string queryUser = "INSERT INTO User (Id, Name, Login_info, Address, Photo, Commands, Cart, Invoices, Prefer_payment, Rating) VALUES (@Val1, @Val2, @Val3, @Val4, @Val5, @Val6, @Val7, @Val8, @Val9, @Val10)";
        string queryLoginInfo = "INSERT INTO Login_info (Id, mail, Password) VALUES (@Val1, @Val2, @Val3)";
        string queryAddress = "INSERT INTO Address (Id, Street, City, CP, State, Country) VALUES (@Val1, @Val2, @Val3, @Val4, @Val5, @Val6)";
        string queryPhoto = "INSERT INTO Photo (Id, Link) VALUES (@Val1, @Val2)";
        string queryCommands = "INSERT INTO Commands (Id, Command) VALUES (@Val1, @Val2)";
        string queryCart = "INSERT INTO Cart (Id, Items) VALUES (@Val1, @Val2)";
        string queryInvoices = "INSERT INTO Invoices (Id, Invoice) VALUES (@Val1, @Val2)";
        string queryPreferPayement = "INSERT INTO Prefer_payment (Id, Payment) VALUES (@Val1, @Val2)";
        string queryRating = "INSERT INTO Rating (Id, Rating, Comment) VALUES (@Val1, @Val2, @Val3)";
        
        // Execution des Querys
        using (SQLiteCommand command = new SQLiteCommand(queryLoginInfo, connection))
        {
            // Ajout des paramètres avec leurs valeurs
            command.Parameters.AddWithValue("@Val1", countUser);
            command.Parameters.AddWithValue("@Val2", parameters["mail"]);
            command.Parameters.AddWithValue("@Val3", parameters["password"]);
            int rowsAffected = command.ExecuteNonQuery(); // Exécution de la commande SQL
        }
        using (SQLiteCommand command = new SQLiteCommand(queryAddress, connection))
        {
            // Ajout des paramètres avec leurs valeurs
            command.Parameters.AddWithValue("@Val1", countUser);
            command.Parameters.AddWithValue("@Val2", "");
            command.Parameters.AddWithValue("@Val3", "");
            command.Parameters.AddWithValue("@Val4", 0);
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
            command.Parameters.AddWithValue("@Val2", 2);
            int rowsAffected = command.ExecuteNonQuery(); // Exécution de la commande SQL
        }
        
        using (SQLiteCommand command = new SQLiteCommand(queryCart, connection))
        {
            // Ajout des paramètres avec leurs valeurs
            command.Parameters.AddWithValue("@Val1", countUser);
            command.Parameters.AddWithValue("@Val2", 5);
            int rowsAffected = command.ExecuteNonQuery(); // Exécution de la commande SQL
        }
        
        using (SQLiteCommand command = new SQLiteCommand(queryInvoices, connection))
        {
            // Ajout des paramètres avec leurs valeurs
            command.Parameters.AddWithValue("@Val1", countUser);
            command.Parameters.AddWithValue("@Val2", 5);
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
            command.Parameters.AddWithValue("@Val9", countUser);
            command.Parameters.AddWithValue("@Val10", countRating);
            int rowsAffected = command.ExecuteNonQuery();// Exécution de la commande SQL
        }
        return "Cette utilisateur à bien été ajouter";
    }
    
    // Insert Items -----------------------------------------------------------------------------------------------------------------
    public static String InsertItem(SQLiteConnection connection, NameValueCollection parameters)
    {
        // Récupération des Id avec un COUNT() et une autre requete 
        int countItems = CountLine(connection, "Items", "Id") + 1;
        int countPhoto = CountLine(connection, "Photo", "Id") + 1 ;
        int countRating = CountLine(connection, "Rating", "Id") + 1 ;
        int idSeller = SelectUserId(connection, "SELECT * FROM  User WHERE Name = '" + parameters["username"] + "';");
        
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
            command.Parameters.AddWithValue("@Val2", parameters["product_name"]);
            command.Parameters.AddWithValue("@Val3", parameters["price"]);
            command.Parameters.AddWithValue("@Val4", parameters["description"]);
            command.Parameters.AddWithValue("@Val5", countPhoto);
            command.Parameters.AddWithValue("@Val6", parameters["id_category"]);
            command.Parameters.AddWithValue("@Val7", idSeller);
            command.Parameters.AddWithValue("@Val8", countRating);
            int rowsAffected = command.ExecuteNonQuery(); // Exécution de la commande SQL
        }
        return "Votre Produit à bien été ajouter !!!!!!!!!!!!!!!!!";
    }
    
    // Update Username ----------------------------------------------------------------------------------------------------------------------
    public static void UpdateUsername(SQLiteConnection connection, string query)
    {
        SQLiteCommand command = new SQLiteCommand(query, connection);
        SQLiteDataReader readerUser = command.ExecuteReader();
    }
    
    // Update Photo ----------------------------------------------------------------------------------------------------------------------
    public static string UpdatePhoto(SQLiteConnection connection, NameValueCollection parameters)
    {
        int idPhoto = 0;
        if (InCollection(parameters, "username"))
        {
            SQLiteCommand command = new SQLiteCommand("SELECT Photo FROM User WHERE Name = '" + parameters["username"] + "';", connection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                // Traitement des résultats de la requête SELECT
                idPhoto = Convert.ToInt32(reader[0]);
            }
            SQLiteCommand update = new SQLiteCommand("UPDATE Photo SET Link = '" + parameters["new_picture"] + "' WHERE Id = " + idPhoto +";", connection);
            SQLiteDataReader readerUpdate = update.ExecuteReader();
            return $"La photo de {parameters["username"]} à bien été mise à jour.";
        }
        else
        {
            SQLiteCommand command = new SQLiteCommand("SELECT Photo FROM Items WHERE Name = '" + parameters["product_name"] + "';", connection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                // Traitement des résultats de la requête SELECT
                idPhoto = Convert.ToInt32(reader[0]);
            }
            SQLiteCommand update = new SQLiteCommand("UPDATE Photo SET Link = '" + parameters["new_picture"] + "' WHERE Id = " + idPhoto +";", connection);
            SQLiteDataReader readerUpdate = update.ExecuteReader();

            return $"La photo du {parameters["product_name"]} à bien été mise à jour.";
        }
    }

    // Update Cart ----------------------------------------------------------------------------------------------------------------------
    public static string UpdateCart(SQLiteConnection connection, NameValueCollection parameters)
    {
        // Récupération de l'id du panier lié à l'utilisateur
        int idCart = 0;
        SQLiteCommand command = new SQLiteCommand("SELECT Cart FROM User WHERE Name = '" + parameters["username"]+ "';", connection);
        SQLiteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            // Traitement des résultats de la requête SELECT
            idCart = Convert.ToInt32(reader[0]);
        }

        // Récupération de l'id du produit
        int idItem = 0;
        SQLiteCommand commandItem = new SQLiteCommand("SELECT Id FROM Items WHERE Name = '" + parameters["product_name"]+ "';", connection);
        SQLiteDataReader readerItem = commandItem.ExecuteReader();
        while (readerItem.Read())
        {
            // Traitement des résultats de la requête SELECT
            idItem = Convert.ToInt32(readerItem[0]);
        }

        // Execution de la requête en fonction de l'option placé en paramètre.
        if (parameters["option"] == "add")
        {
            using (SQLiteCommand insert = new SQLiteCommand("INSERT INTO Cart (Id, Items) VALUES (@Val1, @Val2)", connection))
            {
                // Ajout des paramètres avec leurs valeurs
                insert.Parameters.AddWithValue("@Val1", idCart);
                insert.Parameters.AddWithValue("@Val2", idItem);
                int rowsAffected = insert.ExecuteNonQuery(); // Exécution de la commande SQL
            }
            return $"{parameters["product_name"]} à bien été ajouter au panier de {parameters["username"]}";
        } 
        else if (parameters["option"] == "del")
        {
            SQLiteCommand insert = new SQLiteCommand("DELETE FROM Cart WHERE Id = " + idCart + " AND Items = " + idItem + ";" , connection);
            int rowsAffected = insert.ExecuteNonQuery(); // Exécution de la commande SQL
            return $"{parameters["product_name"]} à bien été supprimer du panier de {parameters["username"]}";
        }
        return $"Vous avez fait une mauvaise requête.";
    }
    
    // DELETE User ----------------------------------------------------------------------------------------------------------------------
    public static void DeleteUser(SQLiteConnection connection, string query)
    {
        SQLiteCommand command = new SQLiteCommand(query, connection);
        SQLiteDataReader readerUser = command.ExecuteReader();
    }
    
    // Delete Item ----------------------------------------------------------------------------------------------------------------------
    public static void DeleteItem(SQLiteConnection connection, NameValueCollection parameters)
    {
        // Faire une select puis delete les Items dans Cart puis l'item lui-même puis sa Photo, puis son Rating. Dans cette ordre
        // Cart -> Item -> Photo -> Cart.
        int idItem = 0;
        int idPhoto = 0;
        int idRating = 0;
        SQLiteCommand commandItem = new SQLiteCommand("SELECT * FROM Items WHERE Name = '" + parameters["item_name"]+ "';", connection);
        SQLiteDataReader readerItem = commandItem.ExecuteReader();
        while (readerItem.Read())
        {
            // Traitement des résultats de la requête SELECT
            idItem = Convert.ToInt32(readerItem[0]);
        }
    }
    
    // Autre ----------------------------------------------------------------------------------------------------------------------
    private static int CountLine(SQLiteConnection connection, string table, string column)
    {
        SQLiteCommand commandUser = new SQLiteCommand("SELECT COUNT(" + column + ") AS Number0fUser FROM "+ table +";", connection);
        SQLiteDataReader readerUser = commandUser.ExecuteReader();
        while (readerUser.Read())
        {
            // Traitement des résultats de la requête SELECT
            int count = Convert.ToInt32(readerUser[0]);
            return count;
        }
        return -1; // Au cas ou ça plante ...
    }
    
    static int SelectUserId(SQLiteConnection connection, string query)
    {
        SQLiteCommand command = new SQLiteCommand(query, connection);
        SQLiteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            // Traitement des résultats de la requête SELECT
            int id = Convert.ToInt32(reader[0]);
            return id;
        }
        return 0;
    }

    static bool InCollection(NameValueCollection parameters, string chaine)
    {
        foreach (var key in parameters.AllKeys)
        {
            if (key == chaine)
            {
                return true;
            }
        }
        return false;
    }
}
