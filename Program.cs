using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        string url = "http://localhost:8080/";
        var listener = new HttpListener();
        listener.Prefixes.Add(url);
        listener.Start();
        Console.WriteLine($"Ecoute sur {url}");

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
}

