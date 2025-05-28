using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WbeMovieUser.Models;

namespace WbeMovieUser.Controllers.Admin
{
    public class ManagerNationController : Controller
    {
        // GET: ManagerNation
        Model1 data = new Model1();
        public ActionResult Index(string search = "", int? page = 1)
        {
            int pageSize = 3; // Số lượng bản ghi mỗi trang
            int pageNumber = page ?? 1;

            var nations = data.Nations
                              .Where(n => string.IsNullOrEmpty(search) || n.nation_name.Contains(search))
                              .OrderBy(n => n.nation_id)
                              .ToPagedList(pageNumber, pageSize);

            ViewBag.CurrentSearch = search;
            return View(nations);
        }

        [HttpPost]
        public JsonResult Add(string nation_name, bool status)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nation_name))
                {
                    Response.StatusCode = 400;
                    return Json(new { message = "Tên quốc gia không được để trống." });
                }

                var existingNation = data.Nations.FirstOrDefault(n => n.nation_name.ToLower() == nation_name.ToLower());
                if (existingNation != null)
                {
                    Response.StatusCode = 400;
                    return Json(new { message = "Quốc gia này đã tồn tại." });
                }

                var nation = new Nation
                {
                    nation_name = nation_name,
                    status = status
                };

                data.Nations.Add(nation);
                data.SaveChanges();

                return Json(new { message = "Thêm quốc gia thành công." });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { message = "Lỗi khi thêm dữ liệu " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Update(Nation model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.nation_name))
                {
                    Response.StatusCode = 400;
                    return Json(new { message = "Tên quốc gia không được để trống." });
                }

                var existingNation = data.Nations.FirstOrDefault(x => x.nation_id == model.nation_id);
                if(existingNation == null)
                {
                    Response.StatusCode = 404;
                    return Json(new { message = "Quốc gia không tồn tại." });
                }
                var existingNameNation = data.Nations.FirstOrDefault(x => x.nation_name.ToLower() == model.nation_name.ToLower()
                                                                && x.nation_id != model.nation_id);

                if (existingNameNation != null) 
                {
                    Response.StatusCode = 400;
                    return Json(new { message = "Tên quốc gia đã tồn tại." });
                }


                existingNation.nation_name = model.nation_name;
                existingNation.status = model.status;
                data.SaveChanges();

                return Json(new { message = "Cập nhật quốc gia thành công." });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { message = "Lỗi khi cập nhật dữ liệu " + ex.Message });
            }
        }
        [HttpPost]
        public JsonResult Delete(int nation_id)
        {
            try
            {
                var existingNation = data.Nations.FirstOrDefault(x => x.nation_id == nation_id);
                if (existingNation == null)
                {
                    Response.StatusCode = 404;
                    return Json(new { message = "Quốc gia không tồn tại." });
                }
                data.Nations.Remove(existingNation);
                data.SaveChanges();
                return Json(new { message = "Xóa quốc gia thành công." });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { message = "Lỗi khi xóa dữ liệu " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ToggleStatus(int nation_id, bool status)
        {
            try
            {
                var existingNation = data.Nations.FirstOrDefault(x => x.nation_id == nation_id);
                if (existingNation == null)
                {
                    Response.StatusCode = 404;
                    return Json(new { message = "Quốc gia không tồn tại." });
                }
                existingNation.status = !status;
                data.SaveChanges();
                return Json(new { message = !status ? "Hiển thị quốc gia thành công." : "Ẩn quốc gia thành công." });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { message = "Lỗi khi cập nhật trạng thái " + ex.Message });
            }

        }
    }
}