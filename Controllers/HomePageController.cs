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
        // GET: HomePage

        private Model1 data = new Model1();
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
            }).Take(3).ToList();
            var listMovie1 = data.movies.Select(x => new MoviesDTO
            {
                movie_id = x.movie_id,
                title = x.title,
                thumbnail_url = x.thumbnail_url,
                trailer_url = x.trailer_url,
            }).Take(5).ToList();
            var listMovie2 = data.movies.Select(x => new MoviesDTO
            {
                movie_id = x.movie_id,
                title = x.title,
                thumbnail_url = x.thumbnail_url,
                trailer_url = x.trailer_url,
            }).Take(5).ToList();
            ViewBag.listBanner = listBanner;
            ViewBag.listMovie1 = listMovie1;
            ViewBag.listMovie2 = listMovie2;
            return View();
        }

        public ActionResult Detail(int id)
        {
            var existingMovie = data.movies.FirstOrDefault(x => x.movie_id == id);
            if(existingMovie == null)
            {
                ViewBag.Message = "Phim này hiện không tồn tại";
                return View();
            }
            var categories = existingMovie.Categories.ToList();
            var espisodes = data.episodes.Where(x => x.series_id == id).ToList();
            if(espisodes != null)
            {
                ViewBag.Espisodes = espisodes;
            }
            ViewBag.Categories = categories;
            return View(existingMovie);
        }
    }
}