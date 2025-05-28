namespace WbeMovieUser.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class comment
    {
        [Key]
        public int comment_id { get; set; }

        public int user_id { get; set; }

        public int movie_id { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string content { get; set; }

        public DateTime? created_at { get; set; }

        public virtual movy movy { get; set; }

        public virtual user user { get; set; }
    }
}
