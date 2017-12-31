using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImdbGrapher.Models.Logic
{
    /// <summary>
    /// The rating for a show
    /// </summary>
    public class ShowRating
    {
        /// <summary>
        /// The show title
        /// </summary>
        public string ShowTitle { get; set; }

        /// <summary>
        /// The IMDB rating
        /// </summary>
        public double? ImdbRating { get; set; }

        /// <summary>
        /// The IMDB ID
        /// </summary>
        public string ImdbId { get; set; }

        /// <summary>
        /// The poster URL
        /// </summary>
        public string PosterUrl { get; set; }

        /// <summary>
        /// The air dates
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// The actors
        /// </summary>
        public string Actors { get; set; }

        /// <summary>
        /// The plot
        /// </summary>
        public string Plot { get; set; }

        /// <summary>
        /// The number of votes
        /// </summary>
        public string ImdbVotes { get; set; }

        /// <summary>
        /// The total seasons
        /// </summary>
        public int TotalSeasons { get; set; }
    }
}
