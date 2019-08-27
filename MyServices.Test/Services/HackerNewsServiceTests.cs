using Moq;
using Moq.Protected;
using MyServices.Interfaces;
using MyServices.Options;
using MyServices.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MyServices.Models;
using Xunit;

namespace MyServices.Test.Services
{
    public class HackerNewsServiceTests
    {
        /// <summary>
        /// Tests that an existing article is removed when it no longer in the latest article lists
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ReconcileArticleIdsAsync_Removes_Value()
        {
            var serviceOptions = Microsoft.Extensions.Options.Options.Create(new ServiceOptions());
            var stringContent = JsonConvert.SerializeObject(new List<int>() { 1, 2, 4, 5, 6 });
            var httpClient = await SetupMockHttpClient(stringContent);
            var service = new Mock<HackerNewsService>(httpClient, serviceOptions);
            service.Setup(x => x.ProcessResultingArticlesAsync(It.IsAny<Article>()))
                .ReturnsAsync((Article itemList) => new List<Item>()
                {
                    new Item {Id = 4},
                    new Item {Id = 5},
                    new Item {Id = 6}
                });

            var items = new List<Item>()
            {
                new Item {Id = 1},
                new Item {Id = 2},
                new Item {Id = 3},

            };
            var result = await service.Object.ReconcileArticleIdsAsync(items);
            Assert.NotNull(result);
            Assert.Equal(result.Count(), 5);
            Assert.Null(result.FirstOrDefault(x => x.Id == 3));

        }
        /// <summary>
        /// Tests that new articles are added when they aren't currently in the memory cache
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ReconcileArticleIdsAsync_Values_Set()
        {
            var serviceOptions = Microsoft.Extensions.Options.Options.Create(new ServiceOptions());
            var stringContent = JsonConvert.SerializeObject(new List<int>() { 1, 2, 3, 4, 5, 6 });
            var httpClient = await SetupMockHttpClient(stringContent);
            var service = new Mock<HackerNewsService>(httpClient, serviceOptions);
            service.Setup(x => x.ProcessResultingArticlesAsync(It.IsAny<Article>()))
                .ReturnsAsync((Article itemList) => new List<Item>()
                {
                    new Item {Id = 4},
                    new Item {Id = 5},
                    new Item {Id = 6}
                });

            var items = new List<Item>()
            {
                new Item {Id = 1},
                new Item {Id = 2},
                new Item {Id = 3},

            };
            var result = await service.Object.ReconcileArticleIdsAsync(items);
            Assert.NotNull(result);
            Assert.Equal( result.Count(), 6);

        }
        /// <summary>
        /// Tests that an Item is returned from the service
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task FilterItemsAsync_Value()
        {
            var serviceOptions = Microsoft.Extensions.Options.Options.Create(new ServiceOptions());
            var enumerableItems = new List<Item>() {
                new Item
            {
                Id = 1,
                Title = "Unit Testing is funnnnddamental!"
            }, new Item
                {
                    Id = 2,
                    Title = "Unit Testing is ok."
                },
            };
            var httpClient = await SetupMockHttpClient("");
            var service = new HackerNewsService(httpClient, serviceOptions);
            var result = await service.FilterItemsAsync(enumerableItems, "funnnn");

            Assert.NotNull(result);
            Assert.Equal(result.FirstOrDefault()?.Id, 1);
        }
        /// <summary>
        /// Tests that an Item is returned from the service
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetSpecificArticle_Returns_Value()
        {
            var serviceOptions = Microsoft.Extensions.Options.Options.Create(new ServiceOptions());
            var stringContent = JsonConvert.SerializeObject(new Item { Id = 1 });
            var httpClient = await SetupMockHttpClient(stringContent);
            var service = new HackerNewsService(httpClient, serviceOptions);
            var result = await service.GetSpecificArticle(1);

            Assert.NotNull(result);
            Assert.Equal(result.Id, 1);
        }
        /// <summary>
        /// Tests that GetLatestArticleIds returns values
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetLatestArticleIdsAsync_Returns_Values()
        {
            var serviceOptions = Microsoft.Extensions.Options.Options.Create(new ServiceOptions());
            var stringContent = JsonConvert.SerializeObject(new List<int>() { 1, 2, 3, 4, 5 });
            var httpClient = await SetupMockHttpClient(stringContent);
            var service = new HackerNewsService(httpClient, serviceOptions);
            var result = await service.GetLatestArticleIdsAsync();

            Assert.NotNull(result);
            Assert.True(result.Count() == 5);
        }

        private async Task<HttpClient> SetupMockHttpClient(string result)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(result),
                })
                .Verifiable();

            // use real http client with mocked handler here
            return await Task.FromResult(new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            });
        }
    }
}