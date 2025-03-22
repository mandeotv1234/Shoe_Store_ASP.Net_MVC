using AspNetCore.Identity.MongoDbCore.Models;  // Thêm namespace để sử dụng MongoIdentityUser
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace ShoeStoreMvc.Models
{
    [CollectionName("users")]  // Đảm bảo collection đúng tên

    public class ApplicationUser : MongoIdentityUser<string>  // Thay IdentityUser bằng MongoIdentityUser
    {
        public string Avatar { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
