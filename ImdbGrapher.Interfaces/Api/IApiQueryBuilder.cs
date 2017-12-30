using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImdbGrapher.Interfaces.Api
{
    public interface IApiQueryBuilder
    {
        /// <summary>
        /// Builds a request for a show by title
        /// </summary>
        /// <param name="title">The show title</param>
        /// <param name="apiKey">The API key</param>
        /// <returns>The request URL</returns>
        string BuildShowTitleRequest(string title, string apiKey);

        /// <summary>
        /// Builds a request for a show by ID
        /// </summary>
        /// <param name="id">The show ID</param>
        /// <param name="apiKey">The API key</param>
        /// <returns>The request URL</returns>
        string BuildShowIdRequest(string id, string apiKey);

        /// <summary>
        /// Builds a request for a season
        /// </summary>
        /// <param name="id">The show ID</param>
        /// <param name="season">The season number</param>
        /// <param name="apiKey">The API key</param>
        /// <returns>The request URL</returns>
        string BuildSeasonIdRequest(string id, int season, string apiKey);

        /// <summary>
        /// Builds a request for a search by showname
        /// </summary>
        /// <param name="showName">The show name</param>
        /// <param name="apiKey">The API key</param>
        /// <returns>The request URL</returns>
        string BuildSearchRequest(string showName, string apiKey);
    }
}
