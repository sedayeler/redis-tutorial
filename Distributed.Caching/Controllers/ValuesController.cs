using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace Distributed.Caching.Controllers
{
    [Route("api/values")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;

        public ValuesController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpPost]
        public async Task<IActionResult> Set(string name, string surname)
        {
            await _distributedCache.SetStringAsync("name", name, options: new()
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(5)
            });
            await _distributedCache.SetAsync("surname", Encoding.UTF8.GetBytes(surname), options: new()
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(5)
            });
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string name = await _distributedCache.GetStringAsync("name");
            byte[] surnameByte = await _distributedCache.GetAsync("surname");
            string surname = Encoding.UTF8.GetString(surnameByte);
            return Ok(new
            {
                name,
                surname
            });
        }
    }
}
