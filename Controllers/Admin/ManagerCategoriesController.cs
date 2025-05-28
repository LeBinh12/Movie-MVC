using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WbeMovieUser.Models;

namespace WbeMovieUser.Controllers.Admin
{
    public class ManagerCategoriesController : Controller
    {
        Model1 movie = new Model1();
        public ActionResult Index(string search = "", int? page = 1)
        {
            var categoriesQuery = movie.Categories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                categoriesQuery = categoriesQuery.Where(c => c.category_name.Contains(search));
            }

            var categories = categoriesQuery.ToList();

            ViewBag.Search = search;

            int pageSize = 5; 
            int pageNumber = (page ?? 1);

            return View(categories.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public JsonResult Add(string categoryName, bool status = false)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                Response.StatusCode = 400;
                return Json(new { message = "Tên thể loại không được để trống." });
            }
            var existingName = movie.Categories.FirstOrDefault(x => x.category_name == categoryName);
            if(existingName != null)
            {
                Response.StatusCode = 400;
                return Json(new { message = "Tên thể loại này đã tồn tại." });
            }
            var newCategory = new Category
            {
                category_name = categoryName,
                status = status
            };

            movie.Categories.Add(newCategory);
            movie.SaveChanges();

            return Json(new { message = "Thêm thành công." });
        }

        [HttpPost]
        public JsonResult Update(int category_id, string category_name, bool status)
        {
            if (string.IsNullOrWhiteSpace(category_name))
            {
                Response.StatusCode = 400;
                return Json(new { message = "Tên thể loại không được để trống." });
            }

            var category = movie.Categories.FirstOrDefault(c => c.category_id == category_id);
            if (category == null)
            {
                Response.StatusCode = 404;
                return Json(new { message = "Không tìm thấy thể loại cần cập nhật." });
            }

            category.category_name = category_name;
            category.status = status;

            movie.SaveChanges();

            return Json(new { message = "Cập nhật thành công." });
        }


        [HttpPost]
        public JsonResult Delete(int categoryId)
        {
            try
            {
                var category = movie.Categories.FirstOrDefault(c => c.category_id == categoryId);
                if (category == null)
                {
                    Response.StatusCode = 404;
                    return Json(new { success = false, message = "Không tìm thấy thể loại cần xóa." });
                }

                movie.Categories.Remove(category);
                movie.SaveChanges();

                return Json(new { success = true, message = "Xóa thể loại thành công." });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { success = false, message = "Lỗi server: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ToggleStatus(int categoryId, bool status)
        {
            var category = movie.Categories.FirstOrDefault(c => c.category_id == categoryId);
            if (category == null)
            {
                Response.StatusCode = 404;
                return Json(new { success = false, message = "Không tìm thấy thể loại cần cập nhật trạng thái." });
            }

            try
            {
                category.status = status;
                movie.SaveChanges();

                return Json(new { success = true, message = "Cập nhật trạng thái thành công.", data = new { categoryId = category.category_id, status = category.status } });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { success = false, message = "Lỗi server: " + ex.Message });
            }
        }

    }
}