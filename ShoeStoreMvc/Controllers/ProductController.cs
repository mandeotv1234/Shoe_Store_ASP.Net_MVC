using Microsoft.AspNetCore.Mvc;
using ShoeStoreMvc.Services;
using ShoeStoreMvc.Models;
using MongoDB.Bson;

namespace ShoeStoreMvc.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            var products = _productService.GetAllProducts();
            var categories = products.Select(p => p.Category).Distinct().ToList();  // Lấy danh sách category từ products
            var brands = products.Select(p => p.Brand).Distinct().ToList();        // Lấy danh sách brand từ products

            ViewBag.Categories = categories;
            ViewBag.Brands = brands;
            return View(products);
        }


        [Route("/product/details/{slug}")]
        public IActionResult Details(string slug)
        {
            var product = _productService.GetProductBySlug(slug);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
    }
}
