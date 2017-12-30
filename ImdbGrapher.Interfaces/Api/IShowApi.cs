using ImdbGrapher.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImdbGrapher.Interfaces.Api
{
    public interface IShowApi
    {
        /// <summary>
        /// Gets the show by title
        /// </summary>
        /// <param name="showName">The show name</param>
        /// <param name="apiKey">The API key</param>
        /// <returns>The API response</returns>
        Task<ShowQueryResponse> GetShowByTitleAsync(string showName, string apiKey);

        /// <summary>
        /// Gets a show by ID
        /// </summary>
        /// <param name="showId">The show ID</param>
        /// <param name="apiKey">The API key</param>
        /// <returns>The API response</returns>
        Task<ShowQueryResponse> GetShowByIdAsync(string showId, string apiKey);

        /// <summary>
        /// Gets a show season
        /// </summary>
        /// <param name="showId">The show ID</param>
        /// <param name="season">The season number</param>
        /// <param name="apiKey">The API key</param>
        /// <returns>The API response</returns>
        Task<SeasonQueryResponse> GetSeasonByIdAsync(string showId, int season, string apiKey);

        /// <summary>
        /// Searches for shows
        /// </summary>
        /// <param name="showName">The show name</param>
        /// <param name="apiKey">The API key</param>
        /// <returns>The API response</returns>
        Task<SearchQueryResponse> SearchForShowAsync(string showName, string apiKey);
    }
}
