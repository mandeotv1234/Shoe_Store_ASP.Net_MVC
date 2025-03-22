using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ShoeStore.Controllers
{
    [Authorize] // Đảm bảo user phải đăng nhập mới truy cập được
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account"); // Chuyển hướng nếu chưa đăng nhập
            }

            ViewData["UserId"] = userId;
            return View();
        }
    }
}
