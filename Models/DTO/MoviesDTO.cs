using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WbeMovieUser.Models.DTO
{
    public class MoviesDTO
    {
        [DisplayName("Mã phim")]
        public int movie_id { get; set; }
        [DisplayName("Tên phim")]
        public string title { get; set; }
        [DisplayName("Ảnh")]
        public string thumbnail_url { get; set; }
        [DisplayName("trailer")]
        public string trailer_url { get; set; }
        [DisplayName("Chi tiết phim")]
        public string description { get; set; }
        [DisplayName("Thời lượng phim")]
        public int? duration { get; set; }
    }
}