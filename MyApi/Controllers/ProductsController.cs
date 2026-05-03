using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using MyApi.Data;
using MyApi.Entity;
using System.Text.Json;

namespace MyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public readonly IDistributedCache _cache;
        private readonly ProductsRepository _productsRepository;

        public ProductsController(IDistributedCache cache)
        {
            //, ProductsRepository productsRepository
            _cache = cache;
            _productsRepository = new ProductsRepository();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cacheKey = "productList";
            var cachedData = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return Ok(JsonSerializer.Deserialize<List<Product>>(cachedData));
            }

            var products = _productsRepository.GetAllProducts();
            var serializedProducts = System.Text.Json.JsonSerializer.Serialize(products);
            
            await _cache.SetStringAsync(cacheKey, serializedProducts, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) // Cache expires after 5 minutes
            });
            return Ok(products);
        }

    }
}
