using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WbeMovieUser.Models;
using WbeMovieUser.Models.DTO;

namespace WbeMovieUser.Controllers
{
    public class HomePageController : Controller
    {
        private Model1 data = new Model1();

        [ChildActionOnly]
        public ActionResult GetCategories()
        {
            var categories = data.Categories.Where(x => x.status == true).ToList();
            return PartialView("_CategoriesPartial", categories);
        }

        [ChildActionOnly]
        public ActionResult GetNations()
        {
            var nations = data.Nations.Where(x => x.status == true).ToList();
            return PartialView("_NationsPartial", nations);
        }

        [ChildActionOnly]
        public ActionResult GetUserInfo()
        {
            var userInfo = new UserInfoViewModel
            {
                IsLoggedIn = false,
                Username = "",
                AvatarUrl = ""
            };

            if (Session["user"] != null)
            {
                var user = Session["user"] as user;
                if (user != null)
                {
                    userInfo.IsLoggedIn = true;
                    userInfo.Username = user.username;
                    userInfo.AvatarUrl = user.avatar_url;
                    userInfo.Email = user.email;

                }
            }

            return PartialView("_UserInfoPartial", userInfo);
        }

        public ActionResult Index()
        {
            var listBanner = data.movies.Select(x => new MoviesDTO
            {
                movie_id = x.movie_id,
                title = x.title,
                thumbnail_url = x.thumbnail_url,
                trailer_url = x.trailer_url,
                description = x.description,
                duration = x.duration
            }).OrderByDescending(x => x.movie_id).Take(3).ToList();
            var listMovie1 = data.movies.Select(x => new MoviesDTO
            {
                movie_id = x.movie_id,
                title = x.title,
                thumbnail_url = x.thumbnail_url,
                trailer_url = x.trailer_url,
            }).OrderByDescending(x => x.movie_id).Skip(3).Take(5).ToList();
            var listMovie2 = data.movies.Select(x => new MoviesDTO
            {
                movie_id = x.movie_id,
                title = x.title,
                thumbnail_url = x.thumbnail_url,
                trailer_url = x.trailer_url,
            }).OrderByDescending(x => x.movie_id).Skip(8).Take(5).ToList();
            ViewBag.listBanner = listBanner;
            ViewBag.listMovie1 = listMovie1;
            ViewBag.listMovie2 = listMovie2;


            var listMovieCategories1A = data.movies
                .Where(x => x.Categories.Any(c => c.category_id == 1))
                .OrderByDescending(x => x.movie_id)
                .Take(5)
                .ToList();
            var listMovieCategories1B = data.movies
                .Where(x => x.Categories.Any(c => c.category_id == 1))
                .OrderByDescending(x => x.movie_id)
                .Skip(5)
                .Take(5)
                .ToList();

            ViewBag.listMovieCategories1A = listMovieCategories1A;
            ViewBag.listMovieCategories1B = listMovieCategories1B;


            var listMovieCategories2A = data.movies
                .Where(x => x.Categories.Any(c => c.category_id == 7))
                .OrderByDescending(x => x.movie_id)
                .Take(5)
                .ToList();
            var listMovieCategories2B = data.movies
                .Where(x => x.Categories.Any(c => c.category_id == 7))
                .OrderByDescending(x => x.movie_id)
                .Skip(5)
                .Take(5)
                .ToList();

            ViewBag.listMovieCategories2A = listMovieCategories2A;
            ViewBag.listMovieCategories2B = listMovieCategories2B;

            var listMovieCategories3A = data.movies
                .Where(x => x.Categories.Any(c => c.category_id == 40))
                .OrderByDescending(x => x.movie_id)
                .Take(5)
                .ToList();
            var listMovieCategories3B = data.movies
                .Where(x => x.Categories.Any(c => c.category_id == 40))
                .OrderByDescending(x => x.movie_id)
                .Skip(5)
                .Take(5)
                .ToList();

            ViewBag.listMovieCategories3A = listMovieCategories3A;
            ViewBag.listMovieCategories3B = listMovieCategories3B;

            var listMovieCategories4A = data.movies
                .Where(x => x.Categories.Any(c => c.category_id == 24))
                .OrderByDescending(x => x.movie_id)
                .Take(5)
                .ToList();
            var listMovieCategories4B = data.movies
                .Where(x => x.Categories.Any(c => c.category_id == 24))
                .OrderByDescending(x => x.movie_id)
                .Skip(5)
                .Take(5)
                .ToList();

            ViewBag.listMovieCategories4A = listMovieCategories4A;
            ViewBag.listMovieCategories4B = listMovieCategories4B;

            return View();
        }

        public ActionResult Detail(int id)
        {
            var existingMovie = data.movies.FirstOrDefault(x => x.movie_id == id);
            if (existingMovie == null)
            {
                ViewBag.Message = "Phim này hiện không tồn tại";
                return View();
            }
            var categories = existingMovie.Categories.ToList();
            var categoryIds = categories.Select(ca => ca.category_id).ToList();
            var episodes = data.episodes.Where(x => x.series_id == id).ToList();

            if (episodes != null)
            {
                ViewBag.Espisodes = episodes;
            }

            var recommendedMovies = data.movies
                .Where(x => x.movie_id != id
                    && x.Categories.Any(p => categoryIds.Contains(p.category_id)))
                .ToList();

            ViewBag.RecommendedMovies = recommendedMovies;
            ViewBag.Categories = categories;
            return View(existingMovie);
        }
    }
}