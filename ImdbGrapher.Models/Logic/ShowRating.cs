using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImdbGrapher.Models.Logic
{
    public class ShowRating
    {
        public string ShowTitle { get; set; }

        public double? ImdbRating { get; set; }

        public string ImdbId { get; set; }

        public string PosterUrl { get; set; }

        public string Year { get; set; }

        public List<SeasonRating> SeasonRatings { get; set; }
    }
}
