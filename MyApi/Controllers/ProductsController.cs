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
        public readonly ProductsRepository ProductsRepository = new ProductsRepository();

        public ProductsController(IDistributedCache cache)
        {
            _cache = cache;
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

            var products = ProductsRepository.GetAllProducts();
            var serializedProducts = System.Text.Json.JsonSerializer.Serialize(products);
            
            await _cache.SetStringAsync(cacheKey, serializedProducts, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) // Cache expires after 5 minutes
            });
            return Ok(products);
        }

    }
}
