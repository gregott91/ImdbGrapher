using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImdbGrapher.Models.Api
{
    /// <summary>
    /// The model for a single search result from the API
    /// </summary>
    public class SearchResultQueryResponse
    {
        /// <summary>
        /// The series title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The series IMDB ID
        /// </summary>
        public string ImdbId { get; set; }

        /// <summary>
        /// The poster
        /// </summary>
        public string Poster { get; set; }

        /// <summary>
        /// The release dates
        /// </summary>
        public string Year { get; set; }
    }
}
