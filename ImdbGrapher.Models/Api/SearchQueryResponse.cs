using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImdbGrapher.Models.Api
{
    public class SearchQueryResponse
    {
        public List<SearchResultQueryResponse> Search { get; set; }

        public bool Response { get; set; }
    }
}
