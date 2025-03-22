using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ShoeStoreMvc.Models
{
    public class Cart
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; }             // ID người dùng
        public List<CartItem> Items { get; set; }      // Danh sách các mục trong giỏ hàng
        public string Status { get; set; }             // Trạng thái giỏ hàng (pending, completed, etc.)
        public DateTime CreatedAt { get; set; }        // Ngày tạo giỏ hàng
    }
}
