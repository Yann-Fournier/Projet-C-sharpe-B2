using System.Net;
using System.Text;
// using System.Data.SQLite;
using MySqlConnector;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Web;
using WEB;

class Program
{
    static async Task Main(string[] args)
    {
        // Reset database ...
        // SQLRequest.CreateDatabaseFile();
        
        // Connection à la base de données
        MySqlConnection connection = SQLRequest.OpenMySqlConnection();
        
        
        // Création de l'api en localhost sur le port 8080
        // string url = "http://localhost:8080/";
        // var listener = new HttpListener();
        // listener.Prefixes.Add(url);
        // listener.Start();
        // Console.WriteLine($"Ecoute sur {url}");
        //
        // // Boucle permettant de récuperer les requêtes
        // while (true)
        // {
        //     var context = await listener.GetContextAsync();
        //     ProcessRequest(context, connection);
        // }
    }
    
    static void ProcessRequest(HttpListenerContext context, MySqlConnection connection)
    {
        string responseString = ""; // Initialisation de la réponse
        bool pasOk = false;
        
        // Récupération du chemin et mise en forme
        string path = context.Request.Url.AbsolutePath.ToLower();
        if (path[path.Length - 1] == '/')
        {
            path = path.Substring(0, path.Length - 1);
        }
        
        string param = context.Request.Url.Query;
        // Parse les paramètres de la chaîne de requête
        NameValueCollection paramet = HttpUtility.ParseQueryString(param); 
        NameValueCollection parameters = new NameValueCollection();;
        foreach (String key in paramet)
        {
            parameters.Add(key, paramet[key].Replace("+", " "));
        }
        
        // Recupération du token d'authentification
        NameValueCollection auth = context.Request.Headers;
        string token = "";
        int User_Id = -1;
        try
        {
            token = auth["Authorization"].Replace("Bearer ", "");
            // Console.WriteLine(token + " " + token.Length);

            User_Id = int.Parse(SQLRequest.GetIdFromToken(connection,
                "SELECT User.Id AS Id FROM User JOIN Auth ON User.Id = Auth.Id WHERE Auth.Token = '" + token + "';"));
        }
        catch (Exception e)
        {
            pasOk = true;
        }
        
        // Exécution de la requête
        if (path == "") // Page d'acceuil, méthode http pas importante.
        {
            if (parameters.Count != 0)
            {
                responseString = "They are too many parameters.";
            }
            else
            {
                responseString = "Hello! Welcome to the home page of this API. This is a project for our school. You can find the documentation at : https://github.com/Yann-Fournier/Projet-C-sharpe-B2.";
            }
        }
        else if (context.Request.HttpMethod == "GET")
        {
            switch (path)
            {
                case "/auth/token": // email, password
                    if (parameters.Count != 2)
                    {
                        responseString = "You have to add 2 parameters only: email, password.";
                    }
                    else
                    {
                        string query = "SELECT Token FROM Auth JOIN User ON Auth.Id = User.Id JOIN Login_info ON User.Id = Login_info.Id WHERE Login_info.mail = '" + parameters["email"] + "' AND Login_info.Password = '" + SQLRequest.HashPwd(parameters["password"]) + "';";
                        responseString = "Voici votre Token d'authentification: " + SQLRequest.GetToken(connection, query);
                    }
                    break;
                case "/address/get": // Authentification
                    if (parameters.Count != 0)
                    {
                        responseString = "They are too many parameters.";
                    }
                    else
                    {
                        responseString = SQLRequest.SelectAddress(connection, "SELECT * FROM Address WHERE Id = " + User_Id + ";");
                    }
                    break;
                case "/category": //type de produit
                    if (parameters.Count != 0)
                    {
                        responseString = "They are too many parameters.";
                    }
                    else
                    {
                        responseString = SQLRequest.SelectCategory(connection, "SELECT * FROM Category;");
                    }
                    break;
                case "/user/get_info": // information sur le compte
                    if (parameters.Count != 0)
                    {
                        responseString = "They are too many parameters.";
                    }
                    else
                    {
                        responseString = SQLRequest.SelectUserInfo(connection, "SELECT * FROM User WHERE Id = " + User_Id + ";");
                    }
                    break;
                case "/user/get_items": // information sur les items
                    if (parameters.Count != 0)
                    {
                        responseString = "They are too many parameters.";
                    }
                    else
                    {
                        responseString = SQLRequest.SelectItems(connection, "SELECT * FROM  Items WHERE Seller = " + User_Id + ";", 1);
                    }
                    break;
                case "/user/commands": // information sur les commandes
                    if (parameters.Count != 0)
                    {
                        responseString = "They are too many parameters.";
                    }
                    else
                    {
                        responseString = SQLRequest.SelectCommands(connection, "SELECT * FROM  Commands JOIN User ON Commands.Id = User.Commands WHERE User.Id =" + User_Id + ";");
                    }
                    break;
                case "/user/cart": // information sur le panier
                    if (parameters.Count != 0)
                    {
                        responseString = "They are too many parameters.";
                    }
                    else
                    {
                        responseString = SQLRequest.SelectCart(connection, "SELECT * FROM Cart JOIN User ON Cart.Id = User.Cart WHERE User.Id =" + User_Id + ";");
                    }
                    break;
                case "/user/invoices": // information sur les factures
                    if (parameters.Count != 0)
                    {
                        responseString = "They are too many parameters.";
                    }
                    else
                    {
                        responseString = SQLRequest.SelectInvoice(connection, "SELECT * FROM Invoices JOIN User ON Invoices.Id = User.Invoices WHERE User.Id =" + User_Id + ";");
                    }
                    break;
                case "/select/items": // produits choisit
                    if (parameters.Count != 0)
                    {
                        responseString = "They are too many parameters.";
                    }
                    else
                    {
                        responseString = SQLRequest.SelectItems(connection, "SELECT * FROM  Items", 0);
                    }
                    break;
                case "/select/items/by_id": // id
                    if (parameters.Count != 1)
                    {
                        responseString = "You have to add 1 parameter only: id.";
                    }
                    else
                    {
                        responseString = SQLRequest.SelectItems(connection, "SELECT * FROM  Items WHERE Id = '" + parameters["id"] + "';", 0);
                    }
                    break;
                case "/select/items/by_name": // name
                    if (parameters.Count != 1)
                    {
                        responseString = "You have to add 1 parameter only: name.";
                    }
                    else
                    {
                        responseString = SQLRequest.SelectItems(connection, "SELECT * FROM  Items WHERE Name = '" + parameters["name"] + "';", 0);
                    }
                    break;
                case "/select/items/by_price": // prix, option
                    if (parameters.Count != 2)
                    {
                        responseString = "You have to add 2 parameter only: id, option.";
                    }
                    else
                    {
                        if (parameters["option"] == "eq")
                        {
                            responseString = SQLRequest.SelectItems(connection, "SELECT * FROM  Items WHERE Price = '" + parameters["price"] + "';", 0);
                        }
                        else if (parameters["option"] == "up")
                        {
                            responseString = SQLRequest.SelectItems(connection, "SELECT * FROM  Items WHERE Price > '" + parameters["price"] + "';", 0);
                        }
                        else if (parameters["option"] == "down")
                        {
                            responseString = SQLRequest.SelectItems(connection, "SELECT * FROM  Items WHERE Price < '" + parameters["price"] + "';", 0);
                        }
                    }
                    break;
                default:
                    responseString = "404 - Not Found:\n\n   - Verify the request method\n   - Verify the url\n   - Verify the parameters\n   - Verify your token";
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
            }
        }
        else if (context.Request.HttpMethod == "POST")
        {
            switch (path)
            {
                case "/create_account": // username, password, email
                    if (parameters.Count != 3)
                    {
                        responseString = "You have to add 3 parameter only: username, password, email.";
                    }
                    else
                    {
                        responseString = SQLRequest.InsertUser(connection, parameters);
                    }
                    break;
                case "/address/add": // Authentification, street, city, cp, state, country
                    if (parameters.Count != 5)
                    {
                        responseString = "You have to add 5 parameter only: street, city, cp, state, country.";
                    }
                    else
                    {
                        SQLRequest.InsertAddress(connection, "UPDATE Address SET Street = '" + parameters["street"] + "', City = '" + parameters["city"] + "', CP = " + parameters["cp"] + ", State = '" + parameters["state"] + "', Country = '" + parameters["country"] + "' WHERE Id = " + User_Id + ";");
                        responseString = "Your address has been add.";
                    }
                    break;
                case "/address/update": // update de la table address
                    foreach (var key in parameters)
                    {
                        switch (key)
                        {
                            case "street":
                                SQLRequest.InsertAddress(connection, "UPDATE Address SET Street = '" + parameters["street"] + "' WHERE Id = " + User_Id + ";");
                                break;
                            case "city":
                                SQLRequest.InsertAddress(connection, "UPDATE Address SET City = '" + parameters["city"] + "' WHERE Id = " + User_Id + ";");
                                break;
                            case "cp":
                                SQLRequest.InsertAddress(connection, "UPDATE Address SET CP = " + parameters["cp"] + " WHERE Id = " + User_Id + ";");
                                break;
                            case "state":
                                SQLRequest.InsertAddress(connection, "UPDATE Address SET State = '" + parameters["state"] + "' WHERE Id = " + User_Id + ";");
                                break;
                            case "country":
                                SQLRequest.InsertAddress(connection, "UPDATE Address SET  Country = '" + parameters["country"] + "' WHERE Id = " + User_Id + ";");
                                break;
                        }
                    }
                    responseString = "Your address has been update.";
                    break;
                case "/user/change_username": // Auth, changement de username
                    if (parameters.Count != 1)
                    {
                        responseString = "You have to add 1 parameter only: new_name.";
                    }
                    else
                    {
                        SQLRequest.UpdateUsername(connection, "UPDATE User SET Name = '" + parameters["new_name"] + "' WHERE Id = " + User_Id + ";");
                        responseString = $"Votre username est maintenant: {parameters["new_name"]}.";
                    }
                    break;
                case "/user/change_photo": // Auth, ajout de photo
                    if (parameters.Count != 1)
                    {
                        responseString = "You have to add 1 parameter only: new_photo.";
                    }
                    else
                    {
                        responseString = SQLRequest.UpdateUserPhoto(connection, parameters["new_photo"], User_Id);
                    }
                    break;
                case "/user/add_item": // Auth, name, prix, description, photo, category
                    if (parameters.Count != 5)
                    {
                        responseString = "You have to add 5 parameter only: name, price, description, photo, category";
                    }
                    else
                    {
                        responseString = SQLRequest.InsertItem(connection, parameters, User_Id);
                    }
                    break;
                case "/user/update_item": // Auth, item_id (name, price, description, photo, category)
                    if (parameters.Count >= 1)
                    {
                        responseString = "You have to add at least 1 parameter: item_id.";
                    }
                    else
                    {
                        foreach (var key in parameters)
                        {
                            switch (key)
                            {
                                case "name":
                                    SQLRequest.UpdateItem(connection, "UPDATE Items SET Name = '" + parameters["name"] + "' WHERE Seller = " + User_Id + " AND Id = " + parameters["item_id"] + ";");
                                    break;
                                case "price":
                                    SQLRequest.UpdateItem(connection, "UPDATE Items SET Price = '" + parameters["Price"] + "' WHERE Seller = " + User_Id + " AND Id = " + parameters["item_id"] + ";");
                                    break;
                                case "description":
                                    SQLRequest.UpdateItem(connection, "UPDATE Items SET Description = '" + parameters["description"] + "' WHERE Seller = " + User_Id + " AND Id = " + parameters["item_id"] + ";");
                                    break;
                                case "picture":
                                    SQLRequest.UpdateItemPhoto(connection, parameters);
                                    break;
                                case "category":
                                    SQLRequest.UpdateItem(connection, "UPDATE Items SET Category = '" + parameters["category"] + "' WHERE Seller = " + User_Id + " AND Id = " + parameters["item_id"] + ";");
                                    break;
                            }
                        }
                        responseString = "Your item has been update";
                    }
                    break;
                case "/user/update_cart": // Auth, item_id, option
                    if (parameters.Count != 2)
                    {
                        responseString = "You have to add 2 parameter only: item_id, option (add, del).";
                    }
                    else
                    {
                        responseString = SQLRequest.UpdateCart(connection, parameters, User_Id);
                    }
                    break;
                case "/delete/user": // Authentification, suppression du compte
                    if (parameters.Count != 0)
                    {
                        responseString = "They are too many parameters.";
                    }
                    else
                    {
                        SQLRequest.DeleteUser(connection, User_Id);
                        responseString = "Your account has been delete successfuly";
                    }
                    break;
                case "/delete/item": // Auth, suppresion d'un item
                    if (parameters.Count != 1)
                    {
                        responseString = "You have to add 1 parameter only: item_id.";
                    }
                    else
                    {
                        SQLRequest.DeleteItem(connection, Convert.ToInt32(parameters["item_id"]), true);
                        responseString = "Your item has been delete successfuly";
                    }
                    break;
                default:
                    responseString = "404 - Not Found:\n\n   - Verify the request method\n   - Verify the url\n   - Verify the parameters\n   - Verify your token";
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
            }
        }
        else
        {
            responseString = "404 - Not Found:\n\n   - Verify the request method\n   - Verify the url\n   - Verify the parameters\n   - Verify your token";
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        }
        
        if (pasOk)
        {
            responseString = "404 - Not Found:\n\n   - Verify the request method\n   - Verify the url\n   - Verify the parameters\n   - Verify your token";
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        }
        
        // Envoie de la réponse.
        byte[] buffer = Encoding.UTF8.GetBytes(responseString);
        context.Response.ContentLength64 = buffer.Length;
        Stream output = context.Response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        output.Close();
    }
}
