using Microsoft.AspNetCore.Mvc;
using PS.CacheCheatsheet_Lite.Caching.Filters;
using PS.CacheCheatsheet_Lite.Models;

namespace PS.CacheCheatsheet_Lite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private static readonly List<Product> _products =
        [
            new() { Id = 1, Name = "Ноутбук", Price = 75000, Category = "Электроника" },
            new() { Id = 2, Name = "Мышь", Price = 1500, Category = "Электроника" },
            new() { Id = 3, Name = "Книга", Price = 500, Category = "Книги" },
            new() { Id = 4, Name = "Телефон", Price = 45000, Category = "Электроника" },
            new() { Id = 5, Name = "Наушники", Price = 3000, Category = "Электроника" }
        ];

        private static async Task SimulateDelayAsync()
        {
            await Task.Delay(1000);
        }

        [HttpGet]
        [Cache(5)]
        public async Task<IActionResult> GetAll()
        {
            await SimulateDelayAsync();
            return Ok(_products);
        }

        [HttpGet("{id:int}")]
        [Cache(1)]
        public async Task<IActionResult> GetById(int id)
        {
            await SimulateDelayAsync();

            var product = _products.FirstOrDefault(p => p.Id == id);

            if (product is null)
                return NotFound();

            return Ok(product);
        }

        [HttpGet("category/{category}")]
        [Cache(1, useSlidingExpiration: true)]
        public async Task<IActionResult> GetByCategory(string category)
        {
            await SimulateDelayAsync();

            var products = _products
                .Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(products);
        }
    }
}
