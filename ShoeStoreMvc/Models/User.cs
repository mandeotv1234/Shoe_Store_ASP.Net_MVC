using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ShoeStoreMvc.Models
{
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsConfirmed { get; set; }
        public string Avatar { get; set; }
        public string Status { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
