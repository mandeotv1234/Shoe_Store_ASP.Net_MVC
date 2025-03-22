using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ShoeStoreMvc.Models
{
    public class Product
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("material")]
        public string Material { get; set; }

        [BsonElement("style")]
        public string Style { get; set; }

        [BsonElement("gender")]
        public string Gender { get; set; }

        [BsonElement("originalPrice")]
        public decimal OriginalPrice { get; set; }

        [BsonElement("salePrice")]
        public decimal SalePrice { get; set; }

        [BsonElement("saleDuration")]
        public int SaleDuration { get; set; }

        [BsonElement("totalPurchased")]
        public int TotalPurchased { get; set; }

        [BsonElement("images")]
        public string[] Images { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

        [BsonElement("availability")]
        public string Availability { get; set; }

        [BsonElement("brand")]
        public string Brand { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("rate")]
        public double Rate { get; set; }

        [BsonElement("warranty")]
        public string Warranty { get; set; }

        [BsonElement("slug")]
        public string Slug { get; set; }

        [BsonElement("tags")]
        public string Tags { get; set; }

        [BsonElement("stock")]
        public Stock[] Stock { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }
        public int __v { get; set; }

        [BsonElement("quantity")] // Ánh xạ với trường "quantity" trong MongoDB
        public int Quantity { get; set; }

        //[BsonElement("sizesavailable")] // Ánh xạ với trường "quantity" trong MongoDB
        public int[] sizesAvailable { get; set; }

    }


}
