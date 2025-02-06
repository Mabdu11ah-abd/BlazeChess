using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace ChessApp.commonModels
{
    public class Game
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        public string Player1Id { get; set; }
        public string Player2Id { get; set; }
        public string CurrentFen { get; set; }
        public TimeControl TimeControl { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public GameStatus Status { get; set; }  // Fixed enum reference
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonIgnoreIfNull]
        public GameState GameState { get; set; }  // Embedded state
    }
    public enum GameStatus
    {
        Ongoing,
        Checkmate,
        Stalemate,
        Draw,
        Resigned
    }
}
