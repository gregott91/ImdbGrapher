using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImdbGrapher.Models.Api
{
    /// <summary>
    /// The response to an API request for a show
    /// </summary>
    public class ShowQueryResponse
    {
        /// <summary>
        /// The title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The IMDB ID
        /// </summary>
        public string ImdbId { get; set; }

        /// <summary>
        /// The total number of seasons
        /// </summary>
        public int TotalSeasons { get; set; }

        /// <summary>
        /// The IMDB rating
        /// </summary>
        public string ImdbRating { get; set; }

        /// <summary>
        /// Whether the query completed successfully
        /// </summary>
        public bool Response { get; set; }

        /// <summary>
        /// The poster URL
        /// </summary>
        public string Poster { get; set; }

        /// <summary>
        /// The release date(s)
        /// </summary>
        public string Year { get; set; }
    }
}
