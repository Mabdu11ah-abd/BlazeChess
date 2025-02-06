using ChessApp.commonModels;
using MongoDB.Driver;

namespace chessApp.server.services
{
    public class UserService
    {

        private readonly IMongoCollection<UserModel> _users;

        public UserService(MongoDbContext dbContext)
        {
            _users = dbContext.Users;
        }

        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            return await _users.Find(_ => true).ToListAsync();
        }

        public async Task<UserModel?> GetUserByIdAsync(string id)
        {
            return await _users.Find(user => user.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateUserAsync(UserModel user)
        {
            await _users.InsertOneAsync(user);
        }

        public async Task<bool> UpdateUserAsync(string id, UserModel updatedUser)
        {
            var result = await _users.ReplaceOneAsync(user => user.Id == id, updatedUser);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var result = await _users.DeleteOneAsync(user => user.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
