using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ShoeStore.Models
{
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string SenderId { get; set; } // User gửi
        public string ReceiverId { get; set; } // Admin nhận
        public string Content { get; set; } // Nội dung tin nhắn

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}
