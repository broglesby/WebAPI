using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MyServices.Interfaces;
using MyServices.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HackerNews.WebClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HackerNewsController : ControllerBase
    {
        private readonly IHackerNewsService _service;
        private readonly IMemoryCache _memoryCache;

        public HackerNewsController(IHackerNewsService service, IMemoryCache memoryCache)
        {
            _service = service;
            _memoryCache = memoryCache;
        }
        // GET: api/HackerNews
        [HttpGet("[action]")]
        public async Task<IActionResult> HackerNewsArticles([FromQuery]int pageIndex, [FromQuery]int pageSize, string filter)
        {
            _memoryCache.TryGetValue("values", out string stringEntry);
            var cacheEntry = string.IsNullOrEmpty(stringEntry) ? null : JsonConvert.DeserializeObject<List<Item>>(stringEntry);
            var results = await _service.ReconcileArticleIdsAsync(cacheEntry ?? new List<Item>());
            _memoryCache.Set("values", JsonConvert.SerializeObject(results));
            var itemsAfterFilter = await _service.FilterItemsAsync(results.ToList(), filter);

            if (itemsAfterFilter == null)
                return NoContent();

            return Ok(new ItemResults
            {
                Items = itemsAfterFilter
                    .Skip(pageSize * pageIndex)
                    .Take(pageSize),
                TotalCount = itemsAfterFilter?.Count() ?? 0
            });
        }

        // GET: api/HackerNews/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
    }
}
