using ChessApp.commonModels;
using chessApp.server.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace chessApp.server.services
{

    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDBSettings> mongoSettings)
        {
            var client = new MongoClient(mongoSettings.Value.ConnectionString);
            _database = client.GetDatabase(mongoSettings.Value.DatabaseName);
        }

        // Define collections for each model
        public IMongoCollection<UserModel> Users => _database.GetCollection<UserModel>("Users");
        public IMongoCollection<Game> Games => _database.GetCollection<Game>("Games");
        public IMongoCollection<MatchmakingQueue> Matchmaking => _database.GetCollection<MatchmakingQueue>("Matchmaking");
    }

}
