using MongoDB.Driver;
using ShoeStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoeStoreMvc.Services
{
    public class ChatService
    {
        private readonly IMongoCollection<Message> _messages;

        public ChatService(IMongoDatabase database)
        {
            _messages = database.GetCollection<Message>("messages");
        }

        public async Task SaveMessageAsync(Message message)
        {
            await _messages.InsertOneAsync(message);
        }
        public async Task<List<string>> GetChatUsers()
        {
            return await _messages
                .Distinct<string>("SenderId", Builders<Message>.Filter.Ne(m => m.SenderId, "admin"))
                .ToListAsync();
        }
        //public async Task<List<Message>> GetMessagesAsync(string user, string admin)
        //{
        //    return await _messages
        //        .Find(m => (m.SenderId == user && m.ReceiverId == admin) || (m.SenderId == admin && m.ReceiverId == user))
        //        .SortBy(m => m.SentAt)
        //        .ToListAsync();
        //}

        // ✅ Lấy lịch sử chat giữa user và admin
        public async Task<List<Message>> GetMessagesAsync(string userId, string adminId)
        {
            var filter = Builders<Message>.Filter.Or(
                Builders<Message>.Filter.And(
                    Builders<Message>.Filter.Eq(m => m.SenderId, userId),
                    Builders<Message>.Filter.Eq(m => m.ReceiverId, adminId)
                ),
                Builders<Message>.Filter.And(
                    Builders<Message>.Filter.Eq(m => m.SenderId, adminId),
                    Builders<Message>.Filter.Eq(m => m.ReceiverId, userId)
                )
            );

            return await _messages.Find(filter).SortBy(m => m.SentAt).ToListAsync();
        }
    }
}
