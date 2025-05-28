namespace WbeMovieUser.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class user
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public user()
        {
            comments = new HashSet<comment>();
            ratings = new HashSet<rating>();
            watch_history = new HashSet<watch_history>();
        }

        [Key]
        public int user_id { get; set; }

        [Required]
        [StringLength(50)]
        public string username { get; set; }

        [Required]
        [StringLength(100)]
        public string email { get; set; }

        [Required]
        [StringLength(255)]
        public string password_hash { get; set; }

        [StringLength(255)]
        public string avatar_url { get; set; }

        public DateTime? created_at { get; set; }

        public DateTime? last_login { get; set; }

        public bool? is_active { get; set; }

        [StringLength(255)]
        public string reset_password_token { get; set; }

        public DateTime? reset_password_expiry { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<comment> comments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rating> ratings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<watch_history> watch_history { get; set; }
    }
}
