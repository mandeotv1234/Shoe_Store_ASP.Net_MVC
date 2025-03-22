using Microsoft.AspNetCore.Mvc;
using ShoeStoreMvc.Models;
using ShoeStoreMvc.Services;  // Thêm thư viện cho ProductService
using System.Diagnostics;

namespace ShoeStoreMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProductService _productService; // Thêm ProductService

        // Sửa constructor để inject ProductService vào HomeController
        public HomeController(ILogger<HomeController> logger, ProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        // Trang chủ hiển thị các sản phẩm mới nhất
        public IActionResult Index()
        {
            // Lấy các sản phẩm mới nhất từ ProductService
            var products = _productService.GetLatestProducts();

            // Trả về view với các sản phẩm
            return View(products);
        }

        // Trang Privacy
        public IActionResult Privacy()
        {
            return View();
        }

        // Xử lý lỗi
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
