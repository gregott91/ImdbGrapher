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

        public async Task<ShowQueryResponse> GetShowByTitleAsync(string showName, string apiKey)
        {
            string request = this.queryBuilder.BuildShowTitleRequest(showName, apiKey);

            return await GetByRequestAsync<ShowQueryResponse>(request);
        }

        public async Task<ShowQueryResponse> GetShowByIdAsync(string showId, string apiKey)
        {
            string request = this.queryBuilder.BuildShowIdRequest(showId, apiKey);

            return await GetByRequestAsync<ShowQueryResponse>(request);
        }

        public async Task<SeasonQueryResponse> GetSeasonByIdAsync(string showId, int season, string apiKey)
        {
            string request = this.queryBuilder.BuildSeasonIdRequest(showId, season, apiKey);

            return await GetByRequestAsync<SeasonQueryResponse>(request);
        }

        public async Task<SearchQueryResponse> SearchForShowAsync(string showName, string apiKey)
        {
            string request = this.queryBuilder.BuildSearchRequest(showName, apiKey);

            return await GetByRequestAsync<SearchQueryResponse>(request);
        }

        private async Task<T> GetByRequestAsync<T>(string request)
        {
            try
            {
                string result = await client.GetStringAsync(request);

                log.Debug("Making API request for URL " + request);

                return JsonConvert.DeserializeObject<T>(result);
            }
            catch (Exception ex)
            {
                log.Error("Error while trying to make API request for URL " + request, ex);

                return default(T);
            }
        }
    }
}
