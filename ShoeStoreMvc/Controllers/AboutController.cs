using Microsoft.AspNetCore.Mvc;
using ShoeStoreMvc.Services;
using ShoeStoreMvc.Models;
using MongoDB.Bson;

namespace ShoeStoreMvc.Controllers
{
    public class AboutController : Controller
    {
       

        public IActionResult Index()
        {
           
            return View();
        }


       
    }
}
