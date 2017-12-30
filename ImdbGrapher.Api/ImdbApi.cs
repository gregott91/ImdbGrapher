using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImdbGrapher.Models.Api;
using System.Net.Http;
using Newtonsoft.Json;
using log4net;

namespace ImdbGrapher.Api
{
    /// <summary>
    /// Handles API interactions
    /// </summary>
    public class ImdbApi
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ImdbApi));

        private ApiQueryBuilder queryBuilder;

        private HttpClient client;

        public ImdbApi()
        {
            this.queryBuilder = new ApiQueryBuilder();
            this.client = new HttpClient();
        }

        /// <summary>
        /// Gets the show by title
        /// </summary>
        /// <param name="showName">The show name</param>
        /// <param name="apiKey">The API key</param>
        /// <returns>The API response</returns>
        public async Task<ShowQueryResponse> GetShowByTitleAsync(string showName, string apiKey)
        {
            string request = this.queryBuilder.BuildShowTitleRequest(showName, apiKey);

            return await GetByRequestAsync<ShowQueryResponse>(request);
        }

        /// <summary>
        /// Gets a show by ID
        /// </summary>
        /// <param name="showId">The show ID</param>
        /// <param name="apiKey">The API key</param>
        /// <returns>The API response</returns>
        public async Task<ShowQueryResponse> GetShowByIdAsync(string showId, string apiKey)
        {
            string request = this.queryBuilder.BuildShowIdRequest(showId, apiKey);

            return await GetByRequestAsync<ShowQueryResponse>(request);
        }

        /// <summary>
        /// Gets a show season
        /// </summary>
        /// <param name="showId">The show ID</param>
        /// <param name="season">The season number</param>
        /// <param name="apiKey">The API key</param>
        /// <returns>The API response</returns>
        public async Task<SeasonQueryResponse> GetSeasonByIdAsync(string showId, int season, string apiKey)
        {
            string request = this.queryBuilder.BuildSeasonIdRequest(showId, season, apiKey);

            return await GetByRequestAsync<SeasonQueryResponse>(request);
        }

        /// <summary>
        /// Searches for shows
        /// </summary>
        /// <param name="showName">The show name</param>
        /// <param name="apiKey">The API key</param>
        /// <returns>The API response</returns>
        public async Task<SearchQueryResponse> SearchForShowAsync(string showName, string apiKey)
        {
            string request = this.queryBuilder.BuildSearchRequest(showName, apiKey);

            return await GetByRequestAsync<SearchQueryResponse>(request);
        }

        /// <summary>
        /// Makes the request and deserializes the result
        /// </summary>
        /// <typeparam name="T">The type to deserialize to</typeparam>
        /// <param name="requestUrl">The URL</param>
        /// <returns>The response</returns>
        private async Task<T> GetByRequestAsync<T>(string requestUrl)
        {
            try
            {
                string result = await client.GetStringAsync(requestUrl);

                log.Debug("Making API request for URL " + requestUrl);

                return JsonConvert.DeserializeObject<T>(result);
            }
            catch (Exception ex)
            {
                log.Error("Error while trying to make API request for URL " + requestUrl, ex);

                return default(T);
            }
        }
    }
}
