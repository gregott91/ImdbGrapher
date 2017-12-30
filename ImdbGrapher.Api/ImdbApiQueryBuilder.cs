using ImdbGrapher.Interfaces.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImdbGrapher.Api
{
    /// <summary>
    /// Handles building the API query
    /// </summary>
    public class ApiQueryBuilder : IApiQueryBuilder
    {
        private const string Url = "http://www.omdbapi.com";

        private const string IdParameter = "i";

        private const string TitleParameter = "t";

        private const string SearchParameter = "s";

        private const string ApiKeyParameter = "apiKey";

        private const string SeasonParameter = "season";

        private const string TypeParameterValue = "type=series";

        /// <summary>
        /// Builds a request for a show by title
        /// </summary>
        /// <param name="title">The show title</param>
        /// <param name="apiKey">The API key</param>
        /// <returns>The request URL</returns>
        public string BuildShowTitleRequest(string title, string apiKey)
        {
            return string.Format("{0}/?{1}={2}&{3}&{4}={5}",
                Url,
                TitleParameter,
                title,
                TypeParameterValue,
                ApiKeyParameter,
                apiKey);
        }

        /// <summary>
        /// Builds a request for a show by ID
        /// </summary>
        /// <param name="id">The show ID</param>
        /// <param name="apiKey">The API key</param>
        /// <returns>The request URL</returns>
        public string BuildShowIdRequest(string id, string apiKey)
        {
            return string.Format("{0}/?{1}={2}&{3}&{4}={5}",
                Url,
                IdParameter,
                id,
                TypeParameterValue,
                ApiKeyParameter,
                apiKey);
        }

        /// <summary>
        /// Builds a request for a season
        /// </summary>
        /// <param name="id">The show ID</param>
        /// <param name="season">The season number</param>
        /// <param name="apiKey">The API key</param>
        /// <returns>The request URL</returns>
        public string BuildSeasonIdRequest(string id, int season, string apiKey)
        {
            return string.Format("{0}/?{1}={2}&{3}&{4}={5}&{6}={7}",
                Url,
                IdParameter,
                id,
                TypeParameterValue,
                SeasonParameter,
                season,
                ApiKeyParameter,
                apiKey);
        }

        /// <summary>
        /// Builds a request for a search by showname
        /// </summary>
        /// <param name="showName">The show name</param>
        /// <param name="apiKey">The API key</param>
        /// <returns>The request URL</returns>
        public string BuildSearchRequest(string showName, string apiKey)
        {
            return string.Format("{0}/?{1}={2}&{3}&{4}={5}",
                Url,
                SearchParameter,
                showName,
                TypeParameterValue,
                ApiKeyParameter,
                apiKey);
        }
    }
}
