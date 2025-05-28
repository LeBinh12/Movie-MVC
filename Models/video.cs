namespace WbeMovieUser.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class video
    {
        [Key]
        public int video_id { get; set; }

        public int? movie_id { get; set; }

        public int? episode_id { get; set; }

        [Required]
        [StringLength(255)]
        public string file_url { get; set; }

        [StringLength(20)]
        public string resolution { get; set; }

        [StringLength(10)]
        public string format { get; set; }

        public virtual episode episode { get; set; }

        public virtual movy movy { get; set; }
    }
}
