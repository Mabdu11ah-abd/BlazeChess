using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

public class RefreshTokenModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string UserId { get; set; }
    public string Token { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; } // e.g., 7 days
}