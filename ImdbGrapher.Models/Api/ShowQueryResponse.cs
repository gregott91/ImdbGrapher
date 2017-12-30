using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImdbGrapher.Models.Api
{
    public class ShowQueryResponse
    {
        public string Title { get; set; }

        public string ImdbId { get; set; }

        public int TotalSeasons { get; set; }

        public string ImdbRating { get; set; }

        public bool Response { get; set; }

        public string Poster { get; set; }

        public string Year { get; set; }
    }
}
