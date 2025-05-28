using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Web;

namespace WbeMovieUser.Models.DTO
{
    public class MovieViewModel
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int? Duration { get; set; }
        public string Language { get; set; }
        public string ThumbnailUrl { get; set; }
        public string TrailerUrl { get; set; }
        public string Description { get; set; }
        public List<RelatedId> RelatedIds { get; set; }
        public bool IsSeries { get; set; }
        public string MovieLink { get; set; }
        public List<EpisodeViewModel> Episodes { get; set; }
    }
}