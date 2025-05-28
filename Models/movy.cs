namespace WbeMovieUser.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class movy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public movy()
        {
            comments = new HashSet<comment>();
            episodes = new HashSet<episode>();
            MovieViews = new HashSet<MovieView>();
            ratings = new HashSet<rating>();
            videos = new HashSet<video>();
            watch_history = new HashSet<watch_history>();
            actors = new HashSet<actor>();
            directors = new HashSet<director>();
            Categories = new HashSet<Category>();
            Nations = new HashSet<Nation>();
        }

        [Key]
        public int movie_id { get; set; }

        [StringLength(255)]
        public string title { get; set; }

        public string description { get; set; }

        [Column(TypeName = "date")]
        public DateTime? release_date { get; set; }

        public int? duration { get; set; }

        public double? rating { get; set; }

        [StringLength(100)]
        public string language { get; set; }

        [StringLength(255)]
        public string thumbnail_url { get; set; }

        [StringLength(500)]
        public string trailer_url { get; set; }

        public DateTime? created_at { get; set; }

        public int? view_count { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<comment> comments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<episode> episodes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MovieView> MovieViews { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rating> ratings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<video> videos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<watch_history> watch_history { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<actor> actors { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<director> directors { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Category> Categories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Nation> Nations { get; set; }
    }
}
