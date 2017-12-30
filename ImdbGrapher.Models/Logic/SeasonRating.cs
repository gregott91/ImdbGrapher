using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImdbGrapher.Models.Logic
{
    /// <summary>
    /// A single season of a show
    /// </summary>
    public class SeasonRating
    {
        /// <summary>
        /// The season number
        /// </summary>
        public int Season { get; set; }

        /// <summary>
        /// The list of episodes
        /// </summary>
        public List<EpisodeRating> EpisodeRatings { get; set; }
    }
}
