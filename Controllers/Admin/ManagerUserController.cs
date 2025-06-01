using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WbeMovieUser.Models;

namespace WbeMovieUser.Controllers.Admin
{
    public class ManagerUserController : Controller
    {
        Model1 movie = new Model1();
        public ActionResult Index(string search = "", int? page = 1)
        {
            if (Session["AdminName"] == null && Session["AdminId"] == null)
                return RedirectToAction("Index", "AuthenticationAdmin");

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            search = search.Trim();
            var users = movie.users.Where(u => u.username.Contains(search) || u.user_id.ToString().Contains(search));
            ViewBag.Search = search;

            users = users.OrderByDescending(u => u.user_id);
            var pagedUsers = users.ToPagedList(pageNumber, pageSize);

            return View(pagedUsers);
        }

        [HttpPost]
        public ActionResult ToggleStatus(int userId, bool status)
        {
            try
            {
                var user = movie.users.FirstOrDefault(x => x.user_id == userId);
                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy người dùng." });
                }

                user.is_active = !status;
                movie.SaveChanges();

                return Json(new { success = true, message = "Cập nhật trạng thái người dùng thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }
        [HttpPost]
        public ActionResult Delete(int userId)
        {
            try
            {
                var user = movie.users.FirstOrDefault(x => x.user_id == userId);
                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy người dùng." });
                }

                movie.users.Remove(user);
                movie.SaveChanges();

                return Json(new { success = true, message = "Xóa người dùng thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }
    }
}