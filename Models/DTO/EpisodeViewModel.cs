using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WbeMovieUser.Models.DTO
{
    public class EpisodeViewModel
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public int? Duration { get; set; }
        public int SeasonNumber { get; set; }
        public int EpisodeNumber { get; set; }
    }
}