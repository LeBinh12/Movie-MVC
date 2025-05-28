using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WbeMovieUser.Models;

namespace WbeMovieUser.Controllers
{
    public class ChildController : Controller
    {
        Model1 data = new Model1();
        public ActionResult Search(string search = "", int? page = 1)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                ViewBag.Message = "Vui lòng nhập từ khóa để tìm kiếm.";
                return View(new List<movy>().ToPagedList(page ?? 1, 6));
            }
            var movieSearch = data.movies.Where(x => x.title.ToLower().Contains(search.ToLower())).ToList();
            if (movieSearch == null)
            {
                ViewBag.Message = "Không tồn tại phim bạn tìm";
                return View(new List<movy>().ToPagedList(page ?? 1, 6));
            }
            ViewBag.Search = search;
            int pageSize = 12;
            int pageNumber = (page ?? 1);


            return View(movieSearch.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Category(int id, int? page)
        {
            var category = data.Categories.FirstOrDefault(c => c.category_id == id);
            if (category == null)
            {
                ViewBag.Message = "Thể loại không tồn tại.";
                return View(new List<movy>().ToPagedList(page ?? 1, 12));
            }

            ViewBag.CategoryName = category.category_name;
            ViewBag.CategoryId = id;

            var movies = data.movies.Where(x => x.Categories.Any(p => p.category_id == id)).ToList();

            int pageSize = 12;
            int pageNumber = (page ?? 1);

            return View(movies.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult SingleMovie(int? page)
        {
            var singleMovie = data.movies.Where(x => !x.episodes.Any()).ToList();
            if (singleMovie.Count <= 0 || singleMovie == null)
            {
                ViewBag.Message = "Không có phim lẻ";
            }
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            return View(singleMovie.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Series(int? page)
        {
            var singleMovie = data.movies.Where(x => x.episodes.Any()).ToList();
            if (singleMovie.Count <= 0 || singleMovie == null)
            {
                ViewBag.Message = "Không có phim bộ";
            }
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            return View(singleMovie.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Nation(int id, int? page)
        {
            var nation = data.Nations.FirstOrDefault(c => c.nation_id == id);
            if (nation == null)
            {
                ViewBag.Message = "Quốc gia không tồn tại.";
                return View(new List<movy>().ToPagedList(page ?? 1, 12));
            }

            ViewBag.NationName = nation.nation_name;
            ViewBag.NationId = id;

            var movies = data.movies.Where(x => x.Nations.Any(p => p.nation_id == id)).ToList();

            int pageSize = 12;
            int pageNumber = (page ?? 1);

            return View(movies.ToPagedList(pageNumber, pageSize));
        }



        public ActionResult History(int? page)
        {
            if (Session["user"] != null)
            {
                var user = Session["user"] as user;

                var existingHistory = data.watch_history.Where(x => x.user_id == user.user_id).ToList();
                if (existingHistory == null && existingHistory.Count <= 0)
                {
                    ViewBag.Message = "Lịch sử bạn đang rỗng";
                    return View();
                }

                int pageSize = 12;
                int pageNumber = page ?? 1;

                var pagedHistory = existingHistory.ToPagedList(pageNumber, pageSize);
                return View(pagedHistory);
            }
            else
            {
                return RedirectToAction("Index", "HomePage");
            }
        }

        public ActionResult Actor(int id, int? page)
        {
            var actors = data.actors.FirstOrDefault(c => c.actor_id == id);
            if (actors == null)
            {
                ViewBag.Message = "Quốc gia không tồn tại.";
                return View(new List<movy>().ToPagedList(page ?? 1, 12));
            }

            ViewBag.ActorName = actors.name;
            ViewBag.ActorId = id;

            var movies = data.movies.Where(x => x.actors.Any(p => p.actor_id == id)).ToList();

            int pageSize = 12;
            int pageNumber = (page ?? 1);

            return View(movies.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Director(int id, int? page)
        {
            var directors = data.directors.FirstOrDefault(c => c.director_id == id);
            if (directors == null)
            {
                ViewBag.Message = "Quốc gia không tồn tại.";
                return View(new List<movy>().ToPagedList(page ?? 1, 12));
            }

            ViewBag.DirectorName = directors.name;
            ViewBag.DirectorId = id;

            var movies = data.movies.Where(x => x.directors.Any(p => p.director_id == id)).ToList();

            int pageSize = 12;
            int pageNumber = (page ?? 1);

            return View(movies.ToPagedList(pageNumber, pageSize));
        }



        //Dành cho PartialView
        public PartialViewResult Ranks()
        {
            var topRatedMovies = data.movies
                .OrderByDescending(m => m.rating) 
                .Take(5)
                .ToList();

            var topViewedMovies = data.movies
               .OrderByDescending(m => m.view_count) 
               .Take(5)
               .ToList();

            var model = new Tuple<List<movy>, List<movy>>(topRatedMovies, topViewedMovies);
            return PartialView("_Ranks", model);
        }



    }
}