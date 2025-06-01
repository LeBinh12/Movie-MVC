using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WbeMovieUser.Models;
using WbeMovieUser.Models.DTO;

namespace WbeMovieUser.Controllers.Admin
{
    public class AuthenticationAdminController : Controller
    {
        Model1 movie = new Model1();
        public ActionResult Index()
        {
            if (Session["Admin"] != null)
            {
                return RedirectToAction("Index", "ManagerMovie");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    return Json(new { success = false, message = "Tên đăng nhập và mật khẩu không được để trống." });
                }

                // Query the Admin table to find a matching admin
                var admin = movie.Admins
                    .FirstOrDefault(a => a.admin_username == username && a.admin_password == password);

                if (admin == null)
                {
                    return Json(new { success = false, message = "Tên đăng nhập hoặc mật khẩu không đúng." });
                }

                // Create session for the authenticated admin
                Session["AdminName"] = admin.admin_name;
                Session["AdminId"] = admin.admin_id;
                return Json(new { success = true, message = "Đăng nhập thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        public ActionResult Logout()
        {
            // Clear the session
            Session.Remove("Admin");
            return RedirectToAction("Index", "AuthenticationAdmin");
        }
    }
}