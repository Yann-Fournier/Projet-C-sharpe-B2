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
        // Execution de la requête et mise en forme de la réponse.
        switch (split_path[0])
        {
            case "/":
                responseString = "Hello, this is the home page!";
                break;
            case "/select":
                if (context.Request.HttpMethod == "GET")
                {
                    try
                    {
                        switch (split_path[1])
                        {
                            case "/user":
                                try
                                {
                                    switch (split_path[2])
                                    {
                                        case "/by_id":
                                            responseString = SQLRequest.SelectUser(connection, "SELECT * FROM  User WHERE Id = " + parameters["id"] + ";");
                                            break;
                                        case "/by_name":
                                            responseString = SQLRequest.SelectUser(connection, "SELECT * FROM  User WHERE Name = '" + parameters["name"] + "';");
                                            break;
                                        default:
                                            responseString = "404 - Not Found";
                                            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                            break;
                                    }
                                }
                                catch (Exception e)
                                {
                                    // Exemple de requête SELECT ALL User
                                    responseString = SQLRequest.SelectUser(connection, "SELECT * FROM  User");
                                }
                                break;
                            case "/item":
                                try
                                {
                                    switch (split_path[2])
                                    {
                                        case "/by_id":
                                            // responseString = "Vous êtes sur la page /select/item/by_id. Voici le produit correspondant à l'identifiant donnée.\n     param: id";
                                            responseString = SQLRequest.SelectItems(connection, "SELECT * FROM  Items WHERE Id = '" + parameters["id"] + "';");
                                            break;
                                        case "/by_name":
                                            // responseString = "Vous êtes sur la page /select/item/by_name. Voici le produit correspondant au nom donnée.\n     param: name";
                                            responseString = SQLRequest.SelectItems(connection, "SELECT * FROM  Items WHERE Name = '" + parameters["name"] + "';");
                                            break;
                                        case "/by_price":
                                            // responseString = "Vous êtes sur la page /select/item/by_price. Voici les correspondant à la fourchette de prix donnée.\n     param: price, option";
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
                                            // responseString = "Vous êtes sur la page /select/commands/by_id. Voici la commande correspondant à l'identifiant donnée.\n     param: id";
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
                                    // responseString = "Vous êtes sur la page /select/commands. Voici la liste de toutes les commandes.";
                                    responseString = SQLRequest.SelectCommands(connection, "SELECT * FROM  Commands");
                                }
                                break;
                            case "/cart":
                                try
                                {
                                    switch (split_path[2])
                                    {
                                        case "/by_id":
                                            // responseString = "Vous êtes sur la page /select/cart/by_id. Voici le panier correspondant à l'identifiant donnée.\n     param: id";
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
                                    // responseString = "Vous êtes sur la page /select/cart. Voici la liste de tous les paniers.";
                                    responseString = SQLRequest.SelectCart(connection, "SELECT * FROM  Cart");
                                }
                                break;
                            case "/invoice":
                                try
                                {
                                    switch (split_path[2])
                                    {
                                        case "/by_id":
                                            // responseString = "Vous êtes sur la page /select/invoice/by_id. Voici la facture correspondant à l'identifiant donnée.\n     param: id";
                                            responseString = SQLRequest.SelectInvoice(connection, "SELECT * FROM  Invoice WHERE Id = '" + parameters["id"] + "';");
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
                                    // responseString = "Vous êtes sur la page /select/invoices. Voici la liste de toutes les factures.";
                                    responseString = SQLRequest.SelectInvoice(connection, "SELECT * FROM  Invoice;");
                                }
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
                                responseString = "Vous êtes sur la page /insert/user. Vous pouvez  créer un nouvel utilisateur avec les paramètres:\n   - name\n    - mail\n    - password.";
                                break;
                            case "/item":
                                responseString = "Vous êtes sur la page /insert/item. Vous pouvez  créer un nouveau produit avec les paramètres:\n   - name\n    - price\n    - description\n   - catégorie.";
                                break;
                            case "/adresse":
                                responseString = "Vous êtes sur la page /insert/adresse. Vous pouvez ajouter une nouvelle adresse lié à un utilisateur avec les paramètres:\n   - username\n    - street\n    - city\n  - cp\n  - state\n   - country.";                                                                         
                                break;
                            case "/picture":
                                responseString = "Vous êtes sur la page /insert/picture. Vous pouvez ajouter une nouvelle photo lié à un utilisateur avec les paramètres:\n   - username\n    - picture.";
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
                                responseString = "Vous êtes sur la page /uptdate/username. Vous pouvez changer votre nom d'utilisateur avec les paramètres:\n    - old_username\n    - new_one.";
                                break;
                            case "/item":
                                responseString = "Vous êtes sur la page /uptdate/item. Vous pouvez changer les élément d'un produit avec les paramètres:\n    - item_name\n   - new_name(opt)\n   - new_price(opt)\n  - new_description(opt)\n    - new_categorie(opt).";                                                     
                                break;
                            case "/adresse":
                                responseString = "Vous êtes sur la page /uptdate/adresse. Vous pouvez changer l'adresse d'un utilisateur avec les paramètres:\n     - username\n    - street(opt)\n    - city(opt)\n  - cp(opt)\n  - state(opt)\n   - country(opt).";                                                                                
                                break;
                            case "/picture":
                                responseString = "Vous êtes sur la page /uptdate/picture. Vous pouvez changer la photo d'un utilisateur avec les paramètres:\n      - username/product_name\n    - new_picture";
                                break;
                            case "/cart":
                                responseString = "Vous êtes sur la page /uptdate/cart. Vous pouvez changer la composition de votre panier avec les paramètres:\n    - username\n    - item_name\n   - option.";
                                break;
                            default:
                                responseString = "404 - Not Found";
                                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                break;
                        }
                    }catch (Exception e)
                    {
                        responseString = "Vous êtes sur la page /insert. Voici les différentes possibilitée d'amélioration.";
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
