using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImdbGrapher.Models.Logic
{
    /// <summary>
    /// A single epsiode of a show
    /// </summary>
    public class EpisodeRating
    {
        /// <summary>
        /// The title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The episode number
        /// </summary>
        public int Episode { get; set; }

        /// <summary>
        /// The IMDB rating
        /// </summary>
        public double ImdbRating { get; set; }

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
