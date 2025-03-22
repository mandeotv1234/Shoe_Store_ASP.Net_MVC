using MongoDB.Driver;
using ShoeStoreMvc.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoeStoreMvc.Services
{
    public class CartService
    {
        private readonly IMongoCollection<Cart> _cartCollection;
        private readonly ProductService _productService;
        public CartService(IMongoDatabase database, ProductService productService)
        {
            _cartCollection = database.GetCollection<Cart>("carts");
            _productService = productService;
        }

        public async Task<Cart> GetCartByUserId(string userId)
        {
            var cart = await _cartCollection.Find(c => c.UserId == userId).FirstOrDefaultAsync();

            if (cart != null && cart.Items.Any())
            {
                foreach (var item in cart.Items)
                {
                    // Truy vấn sản phẩm từ bảng Products
                    var product = await _productService.GetProductByIdAsync(item.ProductId);
                    item.Product = product; // Gán sản phẩm vào CartItem
                }
            }

            return cart;
        }


        public async Task AddToCart(string userId, CartItem newItem)
        {
            var cart = await GetCartByUserId(userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId, Items = new List<CartItem> { newItem } };
                await _cartCollection.InsertOneAsync(cart);
            }
            else
            {
                var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == newItem.ProductId && i.Size == newItem.Size && i.Color == newItem.Color);
                if (existingItem != null)
                {
                    existingItem.Quantity += newItem.Quantity;
                }
                else
                {
                    cart.Items.Add(newItem);
                }

                await _cartCollection.ReplaceOneAsync(c => c.Id == cart.Id, cart);
            }
        }

        public async Task RemoveFromCart(string userId, string productId, int size, string color)
        {
            var filter = Builders<Cart>.Filter.And(
                Builders<Cart>.Filter.Eq(c => c.UserId, userId),
                Builders<Cart>.Filter.ElemMatch(c => c.Items, item =>
                    item.ProductId == productId &&
                    item.Size == size &&
                    item.Color == color)
            );

            var update = Builders<Cart>.Update.PullFilter(c => c.Items,
                item => item.ProductId == productId && item.Size == size && item.Color == color);

            await _cartCollection.UpdateOneAsync(filter, update);
        }
        public async Task ClearCart(string userId)
        {
            await _cartCollection.DeleteOneAsync(c => c.UserId == userId);
        }
    }
}
