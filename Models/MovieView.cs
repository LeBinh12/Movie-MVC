namespace WbeMovieUser.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MovieView
    {
        public int Id { get; set; }

        public int MovieId { get; set; }

        [Required]
        [StringLength(100)]
        public string IpAddress { get; set; }

        public DateTime? ViewedAt { get; set; }

        public virtual movy movy { get; set; }
    }
}
