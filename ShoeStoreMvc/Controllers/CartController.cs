using Microsoft.AspNetCore.Mvc;
using ShoeStoreMvc.Services;
using ShoeStoreMvc.Models;
using System.Threading.Tasks;

namespace ShoeStoreMvc.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> AddToCart(string userId, string productId, int size, string color, int quantity, int price)
        {
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "User không hợp lệ!";
                return RedirectToAction("Index", "Cart");
            }

            if (string.IsNullOrEmpty(productId) || quantity <= 0 || size <= 0 || string.IsNullOrEmpty(color))
            {
                TempData["ErrorMessage"] = "Dữ liệu sản phẩm không hợp lệ!";
                return RedirectToAction("Index", "Cart");
            }

            var newItem = new CartItem
            {
                ProductId = productId,
                Size = size,
                Color = color,
                Quantity = quantity,
                Price = price // Đảm bảo giá trị là int
            };

            await _cartService.AddToCart(userId, newItem);

            // Thêm thông báo vào TempData để hiển thị trên giao diện
            TempData["SuccessMessage"] = "Sản phẩm đã được thêm vào giỏ hàng!";

            return RedirectToAction("Index", "Cart");
        }


        [HttpGet]
        public async Task<IActionResult> ViewCart(string userId)
        {
            var cart = await _cartService.GetCartByUserId(userId);
            return View(cart);
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Lấy userId từ Claims (đảm bảo user đã đăng nhập)
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "Bạn cần đăng nhập để xem giỏ hàng!";
                return RedirectToAction("Login", "Account"); // Chuyển hướng nếu chưa đăng nhập
            }

            var cart = await _cartService.GetCartByUserId(userId);

            if (cart == null || cart.Items.Count == 0)
            {
                TempData["InfoMessage"] = "Giỏ hàng của bạn đang trống.";
            }

            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(string userId, string productId, int size, string color)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(productId))
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ!";
                return RedirectToAction("ViewCart", new { userId });
            }

            await _cartService.RemoveFromCart(userId, productId, size, color);
            TempData["SuccessMessage"] = "Sản phẩm đã được xóa khỏi giỏ hàng!";

            return RedirectToAction("Index", new { userId });
        }


    }
}
