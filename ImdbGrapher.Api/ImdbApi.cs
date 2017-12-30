using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImdbGrapher.Models.Api;
using System.Net.Http;
using Newtonsoft.Json;
using log4net;
using ImdbGrapher.Interfaces.Api;

namespace ImdbGrapher.Api
{
    /// <summary>
    /// Handles API interactions
    /// </summary>
    public class ImdbApi : IShowApi
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ImdbApi));

        private IApiQueryBuilder queryBuilder;

        public ImdbApi(IApiQueryBuilder queryBuilder)
        {
            this.queryBuilder = queryBuilder;
        }

        /// <inheritdoc />
        public async Task<ShowQueryResponse> GetShowByTitleAsync(string showName, string apiKey)
        {
            string request = this.queryBuilder.BuildShowTitleRequest(showName, apiKey);

            return await GetByRequestAsync<ShowQueryResponse>(request);
        }

        /// <inheritdoc />
        public async Task<ShowQueryResponse> GetShowByIdAsync(string showId, string apiKey)
        {
            string request = this.queryBuilder.BuildShowIdRequest(showId, apiKey);

            return await GetByRequestAsync<ShowQueryResponse>(request);
        }

        /// <inheritdoc />
        public async Task<SeasonQueryResponse> GetSeasonByIdAsync(string showId, int season, string apiKey)
        {
            string request = this.queryBuilder.BuildSeasonIdRequest(showId, season, apiKey);

            return await GetByRequestAsync<SeasonQueryResponse>(request);
        }

        /// <inheritdoc />
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
                using (var client = new HttpClient())
                {
                    string result = await client.GetStringAsync(requestUrl);

                    log.Debug("Making API request for URL " + requestUrl);

                    return JsonConvert.DeserializeObject<T>(result);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while trying to make API request for URL " + requestUrl, ex);

                return default(T);
            }
        }
    }
}
