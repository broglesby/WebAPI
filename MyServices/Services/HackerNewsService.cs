using System;
using MyServices.Interfaces;
using MyServices.Models;
using Newtonsoft.Json;
using System.Collections.Async;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MyServices.Options;


namespace MyServices.Services
{
    public class HackerNewsService: IHackerNewsService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<ServiceOptions> _options;

        public HackerNewsService(HttpClient httpClient, IOptions<ServiceOptions> options )
        {
            _httpClient = httpClient;
            _options = options;
        }
        // <see cref="IHackerNewsService.GetSpecificArticle"/>
        public async Task<Item> GetSpecificArticle(int Id)
        {
            var result = await _httpClient.GetAsync(_options.Value.BaseUrl + $"item/{Id.ToString()}.json");
            result.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<Item>(await result.Content.ReadAsStringAsync());

        }
        // <see cref="IHackerNewsService.GetLatestArticleIdsAsync"/>
        public async Task<IEnumerable<int>> GetLatestArticleIdsAsync()
        {
            var result = await _httpClient.GetAsync(_options.Value.BaseUrl + "topstories.json");
            result.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<List<int>>(await result.Content.ReadAsStringAsync());
        }
        // <see cref="IHackerNewsService.ReconcileArticleIdsAsync"/>
        public async Task<IEnumerable<Item>> ReconcileArticleIdsAsync(List<Item> currentData)
        {
            var latestIds = await GetLatestArticleIdsAsync();
            var addedIds = latestIds.Except(currentData?.Select(x => x.Id));
            currentData = currentData.Where(t => latestIds == null || latestIds.Contains(t.Id))?.ToList();
            currentData.AddRange(await ProcessResultingArticlesAsync(new Article {Ids = addedIds.ToList()}));
            return currentData.OrderBy(x => x.Id).AsEnumerable();

        }
        public virtual async Task<IList<Item>> ProcessResultingArticlesAsync(Article articles)
        {
            var bag = new ConcurrentBag<Item>();
            await articles.Ids.ParallelForEachAsync(async item =>
            {
                // some pre stuff
                var response = await GetSpecificArticle(item);
                bag.Add(response);
                // some post stuff
            }, _options.Value.MaxParallelThreads);

            return await Task.FromResult(bag.ToList());
        }

        // <see cref="IHackerNewsService.FilterItemsAsync"/>
        public async Task<IEnumerable<Item>> FilterItemsAsync(IList<Item> results, string filter)
        {
            return await Task.FromResult(results
                .Where(x => string.IsNullOrEmpty(filter)
                            ||
                            x.Title
                                .ToLower()
                                .Contains(filter.ToLower()))
                .ToList());


        }
    }
}
