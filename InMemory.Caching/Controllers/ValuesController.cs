using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemory.Caching.Controllers
{
    [Route("api/values")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;

        public ValuesController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpPost]
        public void SetName(string name)
        {
            _memoryCache.Set("name", name);
        }

        [HttpGet]
        public string GetName()
        {
            return _memoryCache.Get<string>("name");
            }

        [HttpGet("set-date")]
        public void SetDate()
        {
            _memoryCache.Set<DateTime>("date", DateTime.UtcNow, options: new()
            {
                AbsoluteExpiration = DateTime.UtcNow.AddSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(5)
            });
        }

        [HttpGet("get-date")]
        public DateTime GetDate()
        {
            return _memoryCache.Get<DateTime>("date");
        }
    }
}
