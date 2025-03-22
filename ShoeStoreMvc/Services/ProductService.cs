using MongoDB.Bson;
using MongoDB.Driver;
using ShoeStoreMvc.Models;

namespace ShoeStoreMvc.Services
{
    public class ProductService
    {
        private readonly IMongoCollection<Product> _products;

        public ProductService(IMongoDatabase database)
        {
            _products = database.GetCollection<Product>("products");  // Tên collection là "products"
        }

        public List<Product> GetAllProducts()
        {
            
            return _products.Find(product => true).ToList();
        }

        // Phương thức lấy sản phẩm theo productId
        // Phương thức lấy sản phẩm theo productId
        public async Task<Product> GetProductByIdAsync(string productId)
        {
            if (!ObjectId.TryParse(productId, out var objectId))
            {
                return null; // Trả về null nếu productId không hợp lệ
            }

            var filter = Builders<Product>.Filter.Eq("_id", objectId);
            var product = await _products.Find(filter).FirstOrDefaultAsync();

            return product;
        }

        public Product GetProductById(ObjectId id)
        {
            return _products.Find(product => product.Id == id).FirstOrDefault();
        }

        public void CreateProduct(Product product)
        {
            _products.InsertOne(product);
        }

        public List<Product> GetLatestProducts()
        {
            // Truy vấn các sản phẩm, sắp xếp theo updatedAt (giảm dần) và giới hạn 4 sản phẩm mới nhất
            return _products.Find(product => true)
                            .SortByDescending(product => product.UpdatedAt)
                            .Limit(4)
                            .ToList();
        }

        // Lấy sản phẩm theo bộ lọc
        public List<Product> GetFilteredProducts(string category, string brand)
        {
            var filter = Builders<Product>.Filter.Where(p => (category == null || p.Category == category) && (brand == null || p.Brand == brand));
            return _products.Find(filter).ToList();
        }

        public Product GetProductBySlug(string slug)
        {
            return _products.Find(p => p.Slug == slug).FirstOrDefault();
        }
    }
}
