using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.commonModels
{
    public class MatchmakingQueue
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string UserId { get; set; }  // Reference to the user looking for a match
        public int Rating { get; set; }     // For rating-based matchmaking
        public string TimeControl { get; set; }  // Format (e.g., "5+3" for 5 minutes with 3s increment)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
