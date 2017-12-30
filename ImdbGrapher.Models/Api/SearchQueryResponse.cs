using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImdbGrapher.Models.Api
{
    /// <summary>
    /// The model for an API response to a search query
    /// </summary>
    public class SearchQueryResponse
    {
        /// <summary>
        /// The list of results
        /// </summary>
        public List<SearchResultQueryResponse> Search { get; set; }

        /// <summary>
        /// Whether the search completed successfully
        /// </summary>
        public bool Response { get; set; }
    }
}
