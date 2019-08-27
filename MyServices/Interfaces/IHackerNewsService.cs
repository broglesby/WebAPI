using System.Collections.Generic;
using System.Threading.Tasks;
using MyServices.Models;

namespace MyServices.Interfaces
{
    public interface IHackerNewsService
    {
        /// <summary>
        /// Filters the result set based on the user supplied value
        /// </summary>
        /// <param name="results"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<IEnumerable<Item>> FilterItemsAsync(IList<Item> results, string filter);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<Item> GetSpecificArticle(int Id);
        /// <summary>
        /// Checks for new items and adds/subtracts from the existing data
        /// </summary>
        /// <param name="currentData"></param>
        /// <returns></returns>
        Task<IEnumerable<Item>> ReconcileArticleIdsAsync(List<Item> currentData);
        /// <summary>
        /// Multi-threaded solution for getting article data
        /// </summary>
        /// <param name="articles"></param>
        /// <returns></returns>
        Task<IList<Item>> ProcessResultingArticlesAsync(Article articles);
        /// <summary>
        /// Gets the latest set of Ids
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<int>> GetLatestArticleIdsAsync();
    }
}
