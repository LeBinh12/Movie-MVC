using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WbeMovieUser.Models;

namespace WbeMovieUser.Controllers
{
    public class WatchController : Controller
    {
        private Model1 data = new Model1();
        public ActionResult Index(int idMovie, int? idEpcode)
        {
            var existingMovie = data.movies.FirstOrDefault(x => x.movie_id == idMovie);
            var existingVideoMovie = data.videos.FirstOrDefault(x => x.movie_id == idMovie && x.episode_id == null);
            if (existingMovie == null)
            {
                ViewBag.Message = "Hiện không tồn tại phim này";
                return View();
            }
            if (existingVideoMovie == null)
            {
                if (idEpcode != null)
                {
                    var existingEpcode = data.episodes.Where(x => x.episode_id == idEpcode).ToList();
                    var existingVideoEpcode = data.videos.FirstOrDefault(x => x.movie_id == idMovie && x.episode_id == idEpcode);
                    if (existingEpcode == null)
                    {
                        ViewBag.Message = "Tập phim này không tồn tại";
                        return View();
                    }

                    if (existingVideoEpcode == null)
                    {
                        ViewBag.Message = "Tập phim của phim này không tồn tại";
                    }

                    ViewBag.VideoEpcode = existingVideoEpcode;
                    ViewBag.Epcode = existingEpcode;
                    return View(existingMovie);
                }
                ViewBag.Message = "Hiện chưa có video về phim này";
                return View();
            } else
                ViewBag.VideoMovie = existingVideoMovie;
            
            return View(existingMovie);
        }
    }
}