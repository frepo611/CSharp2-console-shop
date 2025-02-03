using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Security.Authentication;

namespace ConsoleShoppen.MongoDB;
internal class Connection
{
    private static MongoClient GetClient()
    {
        string connectionString = "mongodb+srv://fredrikvpost:g3qQUy4PCvVbC9mC@lagerlager.6wqjd.mongodb.net/?retryWrites=true&w=majority&appName=lagerlager";
        MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));

        return new MongoClient(settings);
    }
    private static IMongoCollection<Order> GetMongoOrderCollection(IConfiguration configuration)
    {
        var client = GetClient();
        var database = client.GetDatabase("consoleShoppen");
        return database.GetCollection<Order>("order");
    }
}

