using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImdbGrapher.Models.Api
{
    public class SearchResultQueryResponse
    {
        public string Title { get; set; }

        public string ImdbId { get; set; }

        public string Poster { get; set; }

        public string Year { get; set; }
    }
}
