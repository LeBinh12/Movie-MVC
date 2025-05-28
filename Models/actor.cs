namespace WbeMovieUser.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class actor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public actor()
        {
            movies = new HashSet<movy>();
        }

        [Key]
        public int actor_id { get; set; }

        [Required]
        [StringLength(100)]
        public string name { get; set; }

        [Column(TypeName = "text")]
        public string bio { get; set; }

        [Column(TypeName = "date")]
        public DateTime? birthdate { get; set; }

        public bool? status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<movy> movies { get; set; }
    }
}
