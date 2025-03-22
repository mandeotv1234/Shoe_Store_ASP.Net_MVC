using MongoDB.Driver;
using ShoeStoreMvc.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoeStoreMvc.Services
{
    public class OrderService
    {
        private readonly IMongoCollection<Order> _orders;

        public OrderService(IMongoDatabase database)
        {
            _orders = database.GetCollection<Order>("orders");
        }

        public async Task<List<Order>> GetOrdersByUserId(string userId)
        {
            return await _orders.Find(o => o.CustomerId == userId).ToListAsync();
        }

        public async Task<Order> GetOrderById(string orderId)
        {
            return await _orders.Find(o => o.Id == orderId).FirstOrDefaultAsync();
        }

        public async Task CreateOrderAsync(Order order)
        {
            await _orders.InsertOneAsync(order);
        }
    }
}
