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
        [ChildActionOnly]
        public ActionResult Index()
        {
            //if(Session["AdminName"] == null || Session["AdminId"] == null)
            //    return RedirectToAction("Index", "AuthenticationAdmin");
            //var admin = Session["AdminName"];
            //ViewBag.AdminName = admin;
            return View();
        }

        public PartialViewResult AdminNavbar()
        {
            if (Session["AdminName"] != null)
            {
                ViewBag.AdminName = Session["AdminName"];
            }
            else
            {
                ViewBag.AdminName = "Admin"; // fallback
            }

            return PartialView("_AdminNavbar");
        }
    }
}