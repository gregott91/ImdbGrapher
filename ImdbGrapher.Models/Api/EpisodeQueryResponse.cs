using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImdbGrapher.Models.Api
{
    /// <summary>
    /// Model for the API response for a single episode
    /// </summary>
    public class EpisodeQueryResponse
    {
        /// <summary>
        /// The Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The Episode number
        /// </summary>
        public int Episode { get; set; }

        /// <summary>
        /// The IMDB rating
        /// </summary>
        public string ImdbRating { get; set; }

        /// <summary>
        /// The IMDB ID
        /// </summary>
        public string ImdbId { get; set; }

        /// <summary>
        /// The release date
        /// </summary>
        public string Released { get; set; }
    }
}
