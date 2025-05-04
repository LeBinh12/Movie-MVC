namespace WbeMovieUser.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class episode
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public episode()
        {
            videos = new HashSet<video>();
            watch_history = new HashSet<watch_history>();
        }

        [Key]
        public int episode_id { get; set; }

        public int series_id { get; set; }

        public int season_number { get; set; }

        public int episode_number { get; set; }

        [StringLength(255)]
        public string title { get; set; }

        [Column(TypeName = "text")]
        public string description { get; set; }

        [Column(TypeName = "date")]
        public DateTime? air_date { get; set; }

        public int? duration { get; set; }

        public virtual movy movy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<video> videos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<watch_history> watch_history { get; set; }
    }
}
