using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using WbeMovieUser.Models;

namespace WbeMovieUser.Controllers.Admin
{
    public class ManagerActorController : Controller
    {
        Model1 data = new Model1();
        public ActionResult Index(string search = "", int? page = 1)
        {
            var actorsQuery = data.actors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                actorsQuery = actorsQuery.Where(a => a.name.Contains(search));
            }

            var actors = actorsQuery.ToList();

            ViewBag.Search = search;

            int pageSize = 5; 
            int pageNumber = (page ?? 1);

            return View(actors.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public JsonResult Add(string name, string bio, DateTime birthdate)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    Response.StatusCode = 400;
                    return Json(new { success = false, message = "Tên diễn viên không được để trống." });
                }

                var existingName = data.actors.FirstOrDefault(x => x.name == name);
                if (existingName != null)
                {
                    Response.StatusCode = 400;
                    return Json(new { success = false, message = "Tên diễn viên đã tồn tại " });
                }

                var newActor = new actor
                {
                    name = name,
                    bio = bio,
                    birthdate = birthdate,
                };
                data.actors.Add(newActor);
                data.SaveChanges();
                return Json(new { message = "Thêm diễn viên thành công!" });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { success = false, message = "Lỗi server: " + ex.Message });
            }
        }


        [HttpPost]
        public JsonResult Update (actor model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.name))
                {
                    Response.StatusCode = 400;
                    return Json(new { success = false, message = "Tên diễn viên không được để trống." });
                }

                var existingActor = data.actors.FirstOrDefault(x => x.actor_id == model.actor_id);
                if (existingActor == null)
                {
                    Response.StatusCode = 404;
                    return Json(new { success = false, message = "Diễn viên không tồn tại." });
                }

                var existingName = data.actors.FirstOrDefault(x => x.name == model.name && x.actor_id != model.actor_id);
                if(existingName != null)
                {
                    Response.StatusCode = 400;
                    return Json(new { success = false, message = "Tên diễn viên đã tồn tại." });
                }

                existingActor.name = model.name;
                existingActor.bio = model.bio;
                existingActor.birthdate = model.birthdate;
                data.SaveChanges();

                return Json(new { message = "Cập nhật diễn viên thành công!" });
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
                var existingActor = data.actors.FirstOrDefault(x => x.actor_id == id);
                if (existingActor == null)
                {
                    Response.StatusCode = 404;
                    return Json(new { success = false, message = "Diễn viên không tồn tại." });
                }

                data.actors.Remove(existingActor);
                data.SaveChanges();
                return Json(new { message = "xóa viên thành công!" });

            }
            catch (Exception ex) 
            {
                Response.StatusCode = 500;
                return Json(new { success = false, message = "Lỗi server: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ToggleStatus(int actor_id, bool status)
        {
            try
            {
                var actor = data.actors.Find(actor_id);
                if (actor == null)
                {
                    Response.StatusCode = 404;
                    return Json(new { success = false, message = "Diễn viên không tồn tại." });
                }

                actor.status = status;
                data.SaveChanges();

                return Json(new { message = status ? "Hiển thị diễn viên thành công." : "Ẩn diễn viên thành công." });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { success = false, message = "Lỗi server: " + ex.Message });
            }
        }

    }
}