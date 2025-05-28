namespace WbeMovieUser.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class watch_history
    {
        [Key]
        public int history_id { get; set; }

        public int user_id { get; set; }

        public int? movie_id { get; set; }

        public int? episode_id { get; set; }

        public DateTime? watched_at { get; set; }

        public int? progress { get; set; }

        public virtual episode episode { get; set; }

        public virtual movy movy { get; set; }

        public virtual user user { get; set; }
    }
}
