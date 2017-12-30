using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImdbGrapher.Models.Api
{
    public class SeasonQueryResponse
    {
        public List<EpisodeQueryResponse> Episodes { get; set; }

        public string Season { get; set; }

        public bool Response { get; set; }
    }
}
