namespace WbeMovieUser.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ChatMessage
    {
        public int Id { get; set; }

        [Required]
        [StringLength(450)]
        public string SenderId { get; set; }

        [Required]
        [StringLength(450)]
        public string ReceiverId { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
