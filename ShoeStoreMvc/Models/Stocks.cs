using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ShoeStoreMvc.Models
{
    public class Stock
    {
        [BsonId]            // Đánh dấu đây là trường _id trong MongoDB
        public ObjectId Id { get; set; }

        [BsonElement("quantity")] // Ánh xạ với trường "quantity" trong MongoDB
        public int Quantity { get; set; }

        [BsonElement("size")]    // Ánh xạ với trường "size" trong MongoDB
        public int Size { get; set; }

        [BsonElement("color")]   // Ánh xạ với trường "color" trong MongoDB
        public string Color { get; set; }


    }
}
