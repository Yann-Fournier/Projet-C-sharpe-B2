using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

class Program
{
    static async Task Main(string[] args)
    {
        // Connection à la base de données
        // Data Source = chemin de la database
        string absolutePath = @"..\..\..\BDD\database.sqlite";
        string connectionString = $"Data Source={absolutePath};Version=3;";
        // Creation de la connection
        SQLiteConnection connection = new SQLiteConnection(connectionString);
        try
        {
            // Ouvrir la connection avec la base de données
            connection.Open();
            Console.WriteLine("Connexion réussie à la base de données SQLite!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erreur de connexion: " + ex.Message);
        }

        try
        {
            // Exemple de requête SELECT ALL
            SelectAllLogin_info(connection, "SELECT * FROM Login_info");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Wrong request: " + ex.Message);
        }

        try
        {
            // Fermer la connection avec la base de données
            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Impossible to close connection: " + e.Message);
            throw;
        }
        
        
        // Création de l'api en localhost sur le port 8080
        string url = "http://localhost:8080/";
        var listener = new HttpListener();
        listener.Prefixes.Add(url);
        listener.Start();
        Console.WriteLine($"Ecoute sur {url}");

        // Boucle permettant d'ecouter les requêtes
        while (true)
        {
            var context = await listener.GetContextAsync();
            ProcessRequest(context);
        }
    }

    static void ProcessRequest(HttpListenerContext context)
    {
        string responseString = ""; // tkt

        // Recupération de la requête et mise en forme
        string path = context.Request.Url.AbsolutePath.ToLower();
        string[] split_path = path.Split("/");
        split_path= split_path.Where((source, index) => index != 0).ToArray();
        if (split_path[split_path.Length - 1] == "")
        {
            split_path= split_path.Where((source, index) => index != split_path.Length - 1).ToArray();
        }
        for (int i = 0; i < split_path.Length; i++)
        {
            split_path[i] = "/" + split_path[i];
        }
        
        // Mise en forme de la réponse.
        switch (split_path[0])
        {
            case "/":
                responseString = "Hello, this is the home page!";
                break;
            case "/post":
                if (context.Request.HttpMethod == "POST")
                {
                    try
                    {
                        switch (split_path[1])
                        {
                            case "/test":
                                responseString = "Vous êtes sur la page /post/test";
                                break;
                            case "/bdd":
                                responseString = "Vous êtes sur la page /post/bdd";
                                break;
                            default:
                                responseString = "404 - Not Found";
                                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                break;
                        }
                    }catch (Exception e)
                    {
                        using (var reader = new StreamReader(context.Request.InputStream,
                                   context.Request.ContentEncoding))
                        {
                            string requestBody = reader.ReadToEnd();
                            responseString = $"You posted: {requestBody}";
                        }
                    }
                }
                else
                {
                    responseString = "Invalid request method for this endpoint.";
                }
                break;
            case "/get":
                if (context.Request.HttpMethod == "GET")
                {
                    using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                    {
                        string requestBody = reader.ReadToEnd();
                        responseString = $"You get: {requestBody}";
                    }
                }
                else
                {
                    responseString = "Invalid request method for this endpoint.";
                }
                break;
            default:
                responseString = "404 - Not Found";
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                break;
        }
        byte[] buffer = Encoding.UTF8.GetBytes(responseString);
        context.Response.ContentLength64 = buffer.Length;
        Stream output = context.Response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        output.Close();
    }
    
    static void SelectAllLogin_info(SQLiteConnection connection, string query)
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

    static void ExecuteNonQuery(SQLiteConnection connection, string query)
    {
        SQLiteCommand command = new SQLiteCommand(query, connection);
        int rowsAffected = command.ExecuteNonQuery();
        Console.WriteLine($"Nombre de lignes affectées : {rowsAffected}");
    }
}

