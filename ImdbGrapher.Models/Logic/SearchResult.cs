using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImdbGrapher.Models.Logic
{
    public class SearchResult
    {
        public string Title { get; set; }

        public string PosterUrl { get; set; }

        public string ImdbId { get; set; }

        public string Year { get; set; }
    }
}
