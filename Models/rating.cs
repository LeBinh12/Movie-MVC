namespace WbeMovieUser.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class rating
    {
        [Key]
        public int rating_id { get; set; }

        public int user_id { get; set; }

        public int movie_id { get; set; }

        public decimal? rating_value { get; set; }

        public DateTime? created_at { get; set; }

        public virtual movy movy { get; set; }

        public virtual user user { get; set; }
    }
}
