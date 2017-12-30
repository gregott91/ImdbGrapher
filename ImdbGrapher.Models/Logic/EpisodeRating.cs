﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImdbGrapher.Models.Logic
{
    public class EpisodeRating
    {
        public string Title { get; set; }

        public int Episode { get; set; }

        public double ImdbRating { get; set; }

        public string ImdbId { get; set; }

        public string Released { get; set; }
    }
}
