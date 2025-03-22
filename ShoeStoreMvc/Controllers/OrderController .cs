using Microsoft.AspNetCore.Mvc;
using ShoeStoreMvc.Models;
using ShoeStoreMvc.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoeStoreMvc.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;
        private readonly CartService _cartService;
        private readonly ProductService _productService;

        public OrderController(OrderService orderService, CartService cartService, ProductService productService)
        {
            _orderService = orderService;
            _cartService = cartService;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "Bạn cần đăng nhập để xem đơn hàng!";
                return RedirectToAction("Login", "Account"); // Chuyển hướng đến trang đăng nhập
            }

            var orders = await _orderService.GetOrdersByUserId(userId);
            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(string userId)
        {
            var cart = await _cartService.GetCartByUserId(userId);
            if (cart == null || cart.Items.Count == 0)
            {
                TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống.";
                return RedirectToAction("Index", "Cart");
            }

            // Tạo đơn hàng mới
            var newOrder = new Order
            {
                CustomerId = userId,
                CreatedAt = DateTime.UtcNow,
                Status = "Chờ xác nhận",
                TotalAmount = cart.Items.Sum(i => i.Quantity * i.Price),
                Items = cart.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    Size = i.Size,
                    Color = i.Color
                }).ToList()
            };

            // Lưu đơn hàng vào MongoDB
            await _orderService.CreateOrderAsync(newOrder);

            // Xóa giỏ hàng sau khi đặt hàng thành công
            await _cartService.ClearCart(userId);

            TempData["SuccessMessage"] = "Đơn hàng của bạn đã được đặt thành công!";
            return RedirectToAction("Index", new { userId });
        }

        [HttpGet]
        public async Task<IActionResult> Details(string orderId)
        {
            var order = await _orderService.GetOrderById(orderId);
            if (order == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy đơn hàng!";
                return RedirectToAction("Index");
            }

            // Lấy thông tin sản phẩm từ ProductId
            foreach (var item in order.Items)
            {
                var product = await _productService.GetProductByIdAsync(item.ProductId);
                if (product != null)
                {
                    item.ProductName = product.Name;  // Cập nhật tên sản phẩm
                    item.ImageUrl = product.Images?.FirstOrDefault() ?? "/images/default.jpg"; // Ảnh mặc định nếu không có
                }
            }

            return View(order);
        }
    }
}
