using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace ShoeStoreMvc.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("customerId")] // Đảm bảo tên trường trùng với MongoDB
        public string CustomerId { get; set; }

        public List<OrderItem> Items { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Đang xử lý"; // Mặc định trạng thái đơn hàng

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class OrderItem
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; }

        public int Size { get; set; }
        public string Color { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        // Thuộc tính chỉ dùng để hiển thị, không lưu vào DB
        [BsonIgnore]
        public string ProductName { get; set; }

        [BsonIgnore]
        public string ImageUrl { get; set; }
    }
}
