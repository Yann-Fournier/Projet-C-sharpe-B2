using System.Net;
using System.Text;
using System.Data.SQLite;
using System.Collections.Specialized;

using System.Web;
using BDD; // Notre bibliothèque de requêtes SQL.

class Program
{
    static async Task Main(string[] args)
    {
        // Reset database ...
        // SQLRequest.createDatabaseFile();
        
        // Connection à la base de données
        SQLiteConnection connection = SQLRequest.openSqLiteConnection();
        
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
            ProcessRequest(context, connection);
        }
    }
    
    static void ProcessRequest(HttpListenerContext context, SQLiteConnection connection)
    {
        string responseString = ""; // tkt

        // Recupération de la requête et mise en forme -------------------------------------------------------------
        // Récupération du chemin et mise en forme
        string path = context.Request.Url.AbsolutePath.ToLower(); 
        string[] split_path = path.Split("/");
        split_path = split_path.Where((source, index) => index != 0).ToArray();
        if (split_path[split_path.Length - 1] == "" && split_path.Length != 1) // Pour gérer le cas du 'home page'
        {
            split_path= split_path.Where((source, index) => index != split_path.Length - 1).ToArray();
        }
        for (int i = 0; i < split_path.Length; i++)
        {
            split_path[i] = "/" + split_path[i];
        }
        
        // Récupération des paramètres et mise en forme (?test=123&aze=aze)
        string param = context.Request.Url.Query;
        NameValueCollection paramet = HttpUtility.ParseQueryString(param); // Parse les paramètres de la chaîne de requête
        NameValueCollection parameters = new NameValueCollection();;
        foreach (String key in paramet)
        {
            parameters.Add(key, paramet[key].Replace("+", " "));
        }
        
        // Recupération du token d'authentification
        NameValueCollection auth = context.Request.Headers;
        try
        {
            string token = auth["Authorization"].Replace("Bearer ", "");
            Console.WriteLine(token + " " + token.Length);
        }
        catch (Exception e) {}
        
        
        
        // Execution de la requête et mise en forme de la réponse.
        switch (split_path[0])
        {
            case "/":
                responseString = "Hello, this is the home page!";
                break;
            case "/auth":
                if (context.Request.HttpMethod == "GET")
                {
                    string query = "SELECT Token FROM Auth JOIN User ON Auth.Id = User.Id JOIN Login_info ON User.Id = Login_info.Id WHERE Login_info.mail = '" + parameters["mail"] + "' AND Login_info.Password = '" + SQLRequest.hashPwd(parameters["password"]) + "';";
                    responseString = "Voici votre Token d'authentification: " + SQLRequest.getToken(connection, query);
                }
                break;
            case "/select":
                if (context.Request.HttpMethod == "GET")
                {
                    try
                    {
                        switch (split_path[1])
                        {
                            case "/user_info":
                                
                                // Exemple de requête SELECT ALL User
                                responseString = SQLRequest.SelectUserInfo(connection, parameters);
                                
                                break;
                            case "/item":
                                try
                                {
                                    switch (split_path[2])
                                    {
                                        case "/by_id":
                                            responseString = SQLRequest.SelectItems(connection, "SELECT * FROM  Items WHERE Id = '" + parameters["id"] + "';");
                                            break;
                                        case "/by_name":
                                            responseString = SQLRequest.SelectItems(connection, "SELECT * FROM  Items WHERE Name = '" + parameters["name"] + "';");
                                            break;
                                        case "/by_price":
                                            if (parameters["option"] == "eq")
                                            {
                                                responseString = SQLRequest.SelectItems(connection, "SELECT * FROM  Items WHERE Price = '" + parameters["price"] + "';");
                                            }
                                            else if (parameters["option"] == "up")
                                            {
                                                responseString = SQLRequest.SelectItems(connection, "SELECT * FROM  Items WHERE Price > '" + parameters["price"] + "';");
                                            }
                                            else if (parameters["option"] == "down")
                                            {
                                                responseString = SQLRequest.SelectItems(connection, "SELECT * FROM  Items WHERE Price < '" + parameters["price"] + "';");
                                            }
                                            break;
                                        default:
                                            responseString = "404 - Not Found";
                                            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                            break;
                                    }
                                }
                                catch (Exception e)
                                {
                                    // Exemple de requête SELECT ALL Item
                                    responseString = SQLRequest.SelectItems(connection, "SELECT * FROM  Items");
                                }
                                break;
                            case "/commands":
                                try
                                {
                                    switch (split_path[2])
                                    {
                                        case "/by_id":
                                            responseString = SQLRequest.SelectCommands(connection, "SELECT * FROM  Commands WHERE Id = '" + parameters["id"] + "';");
                                            break;
                                        default:
                                            responseString = "404 - Not Found";
                                            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                            break;
                                    }
                                }
                                catch (Exception e)
                                {
                                    responseString = SQLRequest.SelectCommands(connection, "SELECT * FROM  Commands");
                                }
                                break;
                            case "/cart":
                                try
                                {
                                    switch (split_path[2])
                                    {
                                        case "/by_id":
                                            responseString = SQLRequest.SelectCart(connection, "SELECT * FROM  Cart WHERE Id = '" + parameters["id"] + "';");
                                            break;
                                        default:
                                            responseString = "404 - Not Found";
                                            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                            break;
                                    }
                                }
                                catch (Exception e)
                                {
                                    responseString = SQLRequest.SelectCart(connection, "SELECT * FROM  Cart");
                                }
                                break;
                            case "/invoice":
                                try
                                {
                                    switch (split_path[2])
                                    {
                                        case "/by_id":
                                            responseString = SQLRequest.SelectInvoice(connection, "SELECT * FROM Invoices WHERE Id = '" + parameters["id"] + "';");
                                            break;
                                        default:
                                            responseString = "404 - Not Found";
                                            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                            break;
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Invoices");
                                    responseString = SQLRequest.SelectInvoice(connection, "SELECT * FROM Invoices;");
                                }
                                break;
                            case "/category":
                                responseString = SQLRequest.SelectCategory(connection, "SELECT * FROM Category;");
                                break;
                            default:
                                responseString = "404 - Not Found";
                                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                break;
                        }
                    }catch (Exception e)
                    {
                        responseString = "Vous êtes sur la page /select. Voici les différentes possibilitée de selection.";
                    }
                }
                else
                {
                    responseString = "Invalid request method for this endpoint.";
                }
                break;
            case "/insert":
                if (context.Request.HttpMethod == "POST")
                {
                    try
                    {
                        switch (split_path[1])
                        {
                            case "/user":
                                responseString = SQLRequest.InsertUser(connection, parameters);
                                break;
                            case "/item":
                                responseString = SQLRequest.InsertItem(connection, parameters);
                                break;
                            default:
                                responseString = "404 - Not Found";
                                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                break;
                        }
                    }catch (Exception e)
                    {
                        responseString = "Vous êtes sur la page /insert. Voici les différentes possibilitée d'insertion.";
                    }
                }
                else
                {
                    responseString = "Invalid request method for this endpoint.";
                }
                break;
            case "/update":
                if (context.Request.HttpMethod == "POST")
                {
                    try
                    {
                        switch (split_path[1])
                        {
                            case "/username":
                                SQLRequest.UpdateUsername(connection, "UPDATE User SET Name = '" + parameters["new_name"] + "' WHERE Name = '" + parameters["old_name"] + "';");
                                responseString = $"Votre username est maintenant: {parameters["new_name"]}.";
                                break;
                            case "/item":
                                responseString = "Vous êtes sur la page /uptdate/item. Vous pouvez changer les élément d'un produit avec les paramètres:\n    - item_name\n   - new_name(opt)\n   - new_price(opt)\n  - new_description(opt)\n    - new_categorie(opt).";                                                     
                                break;
                            case "/adresse":
                                responseString = "Vous êtes sur la page /uptdate/adresse. Vous pouvez changer l'adresse d'un utilisateur avec les paramètres:\n     - username\n    - street(opt)\n    - city(opt)\n  - cp(opt)\n  - state(opt)\n   - country(opt).";                                                                                
                                break;
                            case "/photo":
                                responseString = SQLRequest.UpdatePhoto(connection, parameters);
                                break;
                            case "/cart":
                                responseString = SQLRequest.UpdateCart(connection, parameters);
                                break;
                            default:
                                responseString = "404 - Not Found";
                                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                break;
                        }
                    }catch (Exception e)
                    {
                        responseString = "Vous êtes sur la page /update. Voici les différentes possibilitée d'amélioration.";
                    }
                }
                else
                {
                    responseString = "Invalid request method for this endpoint.";
                }
                break;
            case "/delete":
                if (context.Request.HttpMethod == "POST")
                {
                    try
                    {
                        switch (split_path[1])
                        {
                            case "/user":
                                responseString = "Vous êtes sur la page /select/user. Vous pouvez supprimer un utilisateur avec les paramètres:\n   - username.";
                                break;
                            case "/item":
                                responseString = "Vous êtes sur la page /select/item. Vous pouvez supprimer un item avec les paramètres:\n   - item_name.";
                                break;
                            default:
                                responseString = "404 - Not Found";
                                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                break;
                        }
                    }catch (Exception e)
                    {
                        responseString = "Vous êtes sur la page /insert. Voici les différentes possibilitée de suppression.";
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
        
        // Envoie de la réponse.
        byte[] buffer = Encoding.UTF8.GetBytes(responseString);
        context.Response.ContentLength64 = buffer.Length;
        Stream output = context.Response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        output.Close();
    }
}
