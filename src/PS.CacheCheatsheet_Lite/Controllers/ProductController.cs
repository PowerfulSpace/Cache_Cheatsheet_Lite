using Microsoft.AspNetCore.Mvc;

namespace PS.CacheCheatsheet_Lite.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
