using MongoDB.Bson;
using MongoDB.Driver;
using ShoeStoreMvc.Models;

namespace ShoeStoreMvc.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IMongoDatabase database)
        {
            _users = database.GetCollection<User>("users");  // Tên collection là "users"
        }

        public List<User> GetAllUsers()
        {
            return _users.Find(user => true).ToList();
        }

        public User GetUserById(ObjectId id)
        {
            return _users.Find(user => user.Id == id).FirstOrDefault();
        }

        public void CreateUser(User user)
        {
            _users.InsertOne(user);
        }
    }
}
