using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WbeMovieUser.Models;

namespace WbeMovieUser.Controllers.Admin
{
    public class ManagerDirectorController : Controller
    {
        Model1 data = new Model1();
        public ActionResult Index(string search = "", int? page = 1)
        {
            var directorsQuery = data.directors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                directorsQuery = directorsQuery.Where(d => d.name.Contains(search));
            }

            var directors = directorsQuery.ToList();

            ViewBag.Search = search;

            int pageSize = 5; 
            int pageNumber = (page ?? 1); 

            return View(directors.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public JsonResult Add(string name, string bio, DateTime birthdate)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    Response.StatusCode = 400;
                    return Json(new { success = false, message = "Tên đạo diễn không được để trống." });
                }

                var existingName = data.directors.FirstOrDefault(x => x.name.ToLower() == name.ToLower());
                if (existingName != null)
                {
                    Response.StatusCode = 400;
                    return Json(new { success = false, message = "Tên đạo diễn đã tồn tại." });
                }

                var newDirector = new director
                {
                    name = name,
                    bio = bio,
                    birthdate = birthdate,
                };
                data.directors.Add(newDirector);
                data.SaveChanges();
                return Json(new { message = "Thêm đạo diễn thành công!" });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { success = false, message = "Lỗi server: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Update(director model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.name))
                {
                    Response.StatusCode = 400;
                    return Json(new { success = false, message = "Tên đạo diễn không được để trống." });
                }

                var director = data.directors.Find(model.director_id);
                if (director == null)
                {
                    Response.StatusCode = 404;
                    return Json(new { success = false, message = "Đạo diễn không tồn tại." });
                }

                if (data.directors.Any(a => a.name.ToLower() == model.name.ToLower() && a.director_id != model.director_id))
                {
                    Response.StatusCode = 400;
                    return Json(new { success = false, message = "Tên đạo diễn đã tồn tại." });
                }

                director.name = model.name;
                director.bio = model.bio;
                director.birthdate = model.birthdate;
                data.SaveChanges();

                return Json(new { message = "Cập nhật đạo diễn thành công!" });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { success = false, message = "Lỗi server: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                var director = data.directors.Find(id);
                if (director == null)
                {
                    Response.StatusCode = 404;
                    return Json(new { success = false, message = "Đạo diễn không tồn tại." });
                }

                data.directors.Remove(director);
                data.SaveChanges();

                return Json(new { message = "Xóa đạo diễn thành công!" });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { success = false, message = "Lỗi server: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ToggleStatus(int director_id, bool status)
        {
            try
            {
                var director = data.directors.Find(director_id);
                if (director == null)
                {
                    Response.StatusCode = 404;
                    return Json(new { success = false, message = "Đạo diễn không tồn tại." });
                }

                director.status = status;
                data.SaveChanges();



                return Json(new { message = !status ? "Hiển thị đạo diễn thành công." : "Ẩn đạo diễn thành công." });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { success = false, message = "Lỗi server: " + ex.Message });
            }
        }

    }
}