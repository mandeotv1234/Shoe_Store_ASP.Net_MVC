using MongoDB.Bson.Serialization.Attributes;

namespace ShoeStoreMvc.Models
{
    public class CartItem
    {
        public string ProductId { get; set; }        // ID của sản phẩm
        public int Size { get; set; }                // Kích thước của sản phẩm
        public string Color { get; set; }            // Màu sắc của sản phẩm
        public int Quantity { get; set; }            // Số lượng sản phẩm
        public int Price { get; set; }           // Giá của sản phẩm

        [BsonIgnore] // Không lưu vào MongoDB
        public Product Product { get; set; } // Thêm thuộc tính này
    }
}
