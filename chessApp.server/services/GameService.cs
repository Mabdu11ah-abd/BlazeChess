using chessApp.server.services;
using ChessApp.commonModels;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChessApp.Services
{
    public class GameService
    {
        private readonly IMongoCollection<Game> _games;

        public GameService(MongoDbContext database)
        {
            _games = database.Games;
        }

        public async Task<List<Game>> GetAllGamesAsync() => await _games.Find(_ => true).ToListAsync();

        public async Task<Game> GetGameByIdAsync(string id) => await _games.Find(g => g.Id == id).FirstOrDefaultAsync();

        public async Task<Game> CreateGameAsync(Game game)
        {
            await _games.InsertOneAsync(game);
            return game;
        }

        public async Task<bool> UpdateGameAsync(string id, Game updatedGame)
        {
            var result = await _games.ReplaceOneAsync(g => g.Id == id, updatedGame);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteGameAsync(string id)
        {
            var result = await _games.DeleteOneAsync(g => g.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
