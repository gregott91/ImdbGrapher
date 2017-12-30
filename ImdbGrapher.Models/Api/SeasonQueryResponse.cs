using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImdbGrapher.Models.Api
{
    /// <summary>
    /// The model for the API response to a season query
    /// </summary>
    public class SeasonQueryResponse
    {
        /// <summary>
        /// The list of episodes
        /// </summary>
        public List<EpisodeQueryResponse> Episodes { get; set; }

        /// <summary>
        /// The season number
        /// </summary>
        public string Season { get; set; }

        /// <summary>
        /// Whether the response returned successfully
        /// </summary>
        public bool Response { get; set; }
    }
}
