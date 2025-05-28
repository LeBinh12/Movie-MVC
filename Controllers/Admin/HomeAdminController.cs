using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WbeMovieUser.Models;

namespace WbeMovieUser.Controllers.Admin
{
    public class HomeAdminController : Controller
    {
        public ActionResult Index(string search="")
        {
            return View();
        }
    }
}