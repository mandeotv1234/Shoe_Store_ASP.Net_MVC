using AspNetCore.Identity.MongoDbCore.Models;  // Thêm namespace để sử dụng MongoIdentityRole

namespace ShoeStoreMvc.Models
{
    public class ApplicationRole : MongoIdentityRole<string>  // Thay IdentityRole bằng MongoIdentityRole
    {
        // Bạn có thể thêm các thuộc tính tùy chỉnh nếu cần
    }
}
