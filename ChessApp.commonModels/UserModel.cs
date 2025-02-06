using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.commonModels
{
    public class UserModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString(); // MongoDB ObjectId as string
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int EloRating { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> FriendIds { get; set; } = new(); // Store friend user IDs as ObjectIds
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> GameIds { get; set; } = new(); // Store game IDs as ObjectIds
        public string BoardTheme { get; set; } = "default";
        public string PieceType { get; set; } = "classic";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
