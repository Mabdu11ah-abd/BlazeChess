using MongoDB.Driver;
using ChessApp.commonModels;

namespace chessApp.server.services
{
    public class MatchmakingService
    {
        private readonly IMongoCollection<MatchmakingQueue> _matchmakingCollection;

        public MatchmakingService(MongoDbContext database)
        {
            _matchmakingCollection = database.Matchmaking;
        }

        public async Task AddToQueueAsync(MatchmakingQueue queueEntry)
        {
            await _matchmakingCollection.InsertOneAsync(queueEntry);
        }

        public async Task<MatchmakingQueue?> FindMatchAsync(int rating, string timeControl)
        {
            var filter = Builders<MatchmakingQueue>.Filter.Eq(q => q.TimeControl, timeControl) &
                         Builders<MatchmakingQueue>.Filter.Lte(q => q.Rating, rating + 100) &
                         Builders<MatchmakingQueue>.Filter.Gte(q => q.Rating, rating - 100);

            return await _matchmakingCollection.Find(filter).SortBy(q => q.CreatedAt).FirstOrDefaultAsync();
        }

        public async Task RemoveFromQueueAsync(string userId)
        {
            var filter = Builders<MatchmakingQueue>.Filter.Eq(q => q.UserId, userId);
            await _matchmakingCollection.DeleteOneAsync(filter);
        }
    }
}
