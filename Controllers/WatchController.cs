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
            var categories = existingMovie.Categories.ToList();
            var episodes = data.episodes.Where(x => x.series_id == idMovie).ToList();
            if (Session["user"] != null) 
            {
                var user = Session["user"] as user;
                var existingHistoryEpisodes = data.watch_history.FirstOrDefault(x => x.user_id == user.user_id 
                                                                                && x.movie_id == idMovie 
                                                                                && x.episode_id == idEpcode);
                if(existingHistoryEpisodes != null)
                {
                    existingHistoryEpisodes.progress += 1;
                } else
                {
                    var newHistory = new watch_history
                    {
                        user_id = user.user_id,
                        episode_id = idEpcode,
                        movie_id = idMovie,
                        watched_at = DateTime.Now,
                        progress = 1
                    };
                    data.watch_history.Add(newHistory);
                }

                data.SaveChanges();
            }
            if (existingMovie == null)
            {
                ViewBag.Message = "Hiện không tồn tại phim này";
                return View();
            }

            // Kiểm tra phim lẻ hoặc phim bộ
            if (episodes.Any()) // Nếu có episodes, đây là phim bộ
            {
                ViewBag.Episodes = episodes;
                ViewBag.IsSeries = true;

                // Mặc định chọn tập 1 nếu không có idEpcode
                if (!idEpcode.HasValue)
                {
                    var defaultEpisode = episodes.OrderBy(x => x.episode_number).FirstOrDefault();
                    if (defaultEpisode != null)
                    {
                        idEpcode = defaultEpisode.episode_id;
                    }
                }

                if (idEpcode.HasValue)
                {
                    var existingEpisode = episodes.FirstOrDefault(x => x.episode_id == idEpcode);
                    var existingVideoEpcode = data.videos.FirstOrDefault(x => x.movie_id == idMovie && x.episode_id == idEpcode);

                    if (existingEpisode == null)
                    {
                        ViewBag.Message = "Tập phim này không tồn tại";
                        return View();
                    }

                    if (existingVideoEpcode == null)
                    {
                        ViewBag.Message = "Tập phim của phim này không tồn tại";
                        return View();
                    }

                    ViewBag.VideoEpcode = existingVideoEpcode;
                    ViewBag.Epcode = existingEpisode;
                }
                else
                {
                    ViewBag.Message = "Không có tập phim nào để phát";
                    return View();
                }
            }
            else // Phim lẻ
            {
                ViewBag.IsSeries = false;
                var existingVideoMovie = data.videos.FirstOrDefault(x => x.movie_id == idMovie && x.episode_id == null);

                if (existingVideoMovie == null)
                {
                    ViewBag.Message = "Hiện chưa có video về phim này";
                    return View();
                }

                ViewBag.VideoMovie = existingVideoMovie;
            }
            var categoryIds = categories.Select(ca => ca.category_id).ToList();
            var recommendedMovies = data.movies.Where(x => x.movie_id != idMovie
                                            && x.Categories.Any(p => categoryIds.Contains(p.category_id)))
                                            .ToList();

            ViewBag.RecommendedMovies = recommendedMovies;

            ViewBag.Categories = categories;

            if (Request.UserHostAddress != null && existingMovie != null)
            {
                string ipAddress = Request.UserHostAddress;
                DateTime timeThreshold = DateTime.Now.AddMinutes(-30);

                bool hasViewed = data.MovieViews.Any(v => v.MovieId == idMovie
                                                          && v.IpAddress == ipAddress
                                                          && v.ViewedAt >= timeThreshold);

                if (!hasViewed)
                {
                    var movieView = new MovieView
                    {
                        MovieId = idMovie,
                        IpAddress = ipAddress,
                        ViewedAt = DateTime.Now
                    };
                    data.MovieViews.Add(movieView);

                    existingMovie.view_count += 1;

                    data.SaveChanges();
                }
            }


            return View(existingMovie);
        }

        [HttpPost]
        public ActionResult RateMovie(int movieId, decimal rating)
        {
            if (Session["user"] == null)
                return Json(new { success = false, message = "Vui lòng đăng nhập để đánh giá." });

            var user = Session["user"] as user;

            var existingRating = data.ratings.FirstOrDefault(x => x.user_id == user.user_id && x.movie_id == movieId);
            var getMovie = data.movies.FirstOrDefault(x => x.movie_id == movieId);
            if (existingRating != null)
            {
                existingRating.rating_value = rating;
                existingRating.created_at = DateTime.Now;
            }
            else
            {
                var newRating = new rating
                {
                    user_id = user.user_id,
                    movie_id = movieId,
                    rating_value = rating,
                    created_at = DateTime.Now
                };
                data.ratings.Add(newRating);
            }

            data.SaveChanges();
            var avgRating = data.ratings
                .Where(r => r.movie_id == movieId)
                .Average(r => (decimal?)r.rating_value) ?? 0;

            ViewBag.AverageRating = Math.Round(avgRating, 1);

            getMovie.rating = (double)Math.Round(avgRating, 1);
            data.SaveChanges();

            return Json(new { success = true, average = Math.Round(avgRating, 1) });
        }
    }
}

