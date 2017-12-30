using ImdbGrapher.Models.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImdbGrapher.Interfaces.Business
{
    public interface IApiLogic
    {        
        /// <summary>
        /// Gets the show ID from the title
        /// </summary>
        /// <param name="showName">The show name</param>
        /// <returns>The show ID</returns>
        Task<string> GetShowIdFromTitleAsync(string showName);

        /// <summary>
        /// Gets the list of ratings for a show
        /// </summary>
        /// <param name="showId">The show ID</param>
        /// <returns>The show rating</returns>
        Task<ShowRating> GetShowEpisodeRatingsAsync(string showId);

        /// <summary>
        /// Searches for shows based off the show name
        /// </summary>
        /// <param name="showName">The show name</param>
        /// <returns>The list of results</returns>
        Task<List<SearchResult>> SearchForShowsAsync(string showName);
    }
}
