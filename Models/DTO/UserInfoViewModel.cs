using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WbeMovieUser.Models.DTO
{
    public class UserInfoViewModel
    {
        public bool IsLoggedIn { get; set; }
        public string Username { get; set; }
        public string AvatarUrl { get; set; }
        public string Email { get; set; }
    }
}