using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Text.Json;
using DotNetEnv;

public class MongoDBConnection
{
    private readonly MongoClient _client;
    private readonly IMongoDatabase _database;

    public MongoDBConnection()
    {
        // Cargar las variables de entorno desde el archivo .env
        Env.Load();

        // Obtener la URI de la base de datos desde la variable de entorno
        var connectionUri = Environment.GetEnvironmentVariable("MONGO_URI");
        if (string.IsNullOrEmpty(connectionUri))
        {
            throw new InvalidOperationException("Connection URI is not set.");
        }

        var settings = MongoClientSettings.FromConnectionString(connectionUri);

        // Set the ServerApi field of the settings object to set the version of the Stable API on the client
        settings.ServerApi = new ServerApi(ServerApiVersion.V1);

        // Create a new client and connect to the server
        _client = new MongoClient(settings);

        try
        {
            // Ping to confirm connection
            var result = _client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
            Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to MongoDB: {ex.Message}");
        }

        // Set the database you want to work with
        _database = _client.GetDatabase("yourDatabaseName"); // Cambia "yourDatabaseName" por el nombre de tu base de datos
    }

    public IMongoDatabase GetDatabase()
    {
        return _database;
    }
}
