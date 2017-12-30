using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImdbGrapher.Models.Logic
{
    /// <summary>
    /// A single search result
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// The result title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The poster URL
        /// </summary>
        public string PosterUrl { get; set; }

        /// <summary>
        /// The IMDB ID
        /// </summary>
        public string ImdbId { get; set; }

        /// <summary>
        /// The airdates
        /// </summary>
        public string Year { get; set; }
    }
}
