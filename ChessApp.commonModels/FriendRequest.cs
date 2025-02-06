using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChessApp.commonModels
{
    public class FriendRequest
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        public string FromUserId { get; set; }
        public string ToUserId { get; set; }

        [BsonRepresentation(BsonType.String)]
        public FriendRequestStatus Status { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
    public enum FriendRequestStatus
    {
        Pending,
        Accepted,
        Rejected
    }

}
