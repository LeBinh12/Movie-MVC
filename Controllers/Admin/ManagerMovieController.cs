using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WbeMovieUser.Models;
using WbeMovieUser.Models.DTO;

namespace WbeMovieUser.Controllers.Admin
{
    public class ManagerMovieController : Controller
    {
        Model1 movie = new Model1();
        public ActionResult Index(string search = "", int? page = 1)
        {
            var movieQuery = movie.movies.AsQueryable();

            // Tìm kiếm theo tiêu đề
            if (!string.IsNullOrWhiteSpace(search))
            {
                movieQuery = movieQuery.Where(x => x.title.Contains(search));
            }

            var movieSearch = movieQuery.ToList();

            // Hiển thị tất cả thông tin cần thiết cho phần thêm
            var categories = movie.Categories.Where(x => x.status == true).ToList();
            var directors = movie.directors.ToList();
            var actors = movie.actors.ToList();
            var nations = movie.Nations.Where(x => x.status == true).ToList();

            ViewBag.Categories = categories;
            ViewBag.Directors = directors;
            ViewBag.Actors = actors;
            ViewBag.Nations = nations;
            
            if (movieSearch.Count <= 0)
            {
                ViewBag.Message = "Không có phim bạn tìm kiếm";
            }

            int pageSize = 5; // Số phim trên mỗi trang
            int pageNumber = (page ?? 1); // Trang hiện tại, mặc định là 1
            return View(movieSearch.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult AddMovie(MovieViewModel model)
        {
            try
            {
                var existingName = movie.movies.FirstOrDefault(x => x.title == model.Title);
                if (existingName != null)
                {
                    Response.StatusCode = 400;
                    return Json(new { message = "Tên phim này đã tồn tại." });
                }
                var newMovie = new movy
                {
                    title = model.Title,
                    release_date = model.ReleaseDate,
                    duration = model.Duration,
                    language = model.Language,
                    thumbnail_url = model.ThumbnailUrl,
                    trailer_url = model.TrailerUrl,
                    created_at = DateTime.Now,
                    description = model.Description,
                    view_count = 0,
                    rating = 0
                };

                if (model.RelatedIds != null)
                {
                    foreach (var related in model.RelatedIds)
                    {
                        switch (related.Type)
                        {
                            case "category":
                                var category = movie.Categories.Find(related.Id);
                                if (category != null)
                                    newMovie.Categories.Add(category);
                                break;
                            case "actor":
                                var actor = movie.actors.Find(related.Id);
                                if (actor != null)
                                    newMovie.actors.Add(actor);
                                break;
                            case "director":
                                var director = movie.directors.Find(related.Id);
                                if (director != null)
                                    newMovie.directors.Add(director);
                                break;
                            case "nation":
                                var nation = movie.Nations.Find(related.Id);
                                if (nation != null)
                                    newMovie.Nations.Add(nation);
                                break;
                        }
                    }
                }
                movie.movies.Add(newMovie);
                movie.SaveChanges();

                // Nếu thêm phim lẻ vào
                if (!model.IsSeries && !string.IsNullOrEmpty(model.MovieLink))
                {
                    var video = new video
                    {
                        movie_id = newMovie.movie_id,
                        file_url = model.MovieLink,
                        format = "mp4",
                        resolution = "1080p"
                    };
                    movie.videos.Add(video);
                }

                // Xử lý thêm phim bộ 
                if (model.IsSeries && model.Episodes != null)
                {
                    foreach (var episode in model.Episodes)
                    {
                        var newEpisodes = new episode
                        {
                            series_id = newMovie.movie_id,
                            season_number = episode.SeasonNumber,
                            episode_number = episode.EpisodeNumber,
                            duration = episode.Duration,
                            title = episode.Title,
                            air_date = newMovie.release_date
                        };
                        movie.episodes.Add(newEpisodes);
                        movie.SaveChanges();

                        var video = new video
                        {
                            episode_id = newEpisodes.episode_id,
                            movie_id = newMovie.movie_id,
                            file_url = episode.Link,
                            format = "mp4",
                            resolution = "1080p"
                        };
                        movie.videos.Add(video);
                    }
                }

                movie.SaveChanges();
                return Json(new { success = true, message = "Thêm phim thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi thêm phim: " + ex.Message });
            }
        }


        public ActionResult EditMovie(int id)
        {
            try
            {
                var movieToEdit = movie.movies.FirstOrDefault(x => x.movie_id == id);
                if (movieToEdit == null)
                    return HttpNotFound();

                var viewModel = new MovieViewModel
                {
                    MovieId = movieToEdit.movie_id,
                    Title = movieToEdit.title,
                    ReleaseDate = movieToEdit.release_date,
                    Duration = movieToEdit.duration,
                    Language = movieToEdit.language,
                    ThumbnailUrl = movieToEdit.thumbnail_url,
                    TrailerUrl = movieToEdit.trailer_url,
                    IsSeries = movieToEdit.episodes.Any(),
                    Description = movieToEdit.description,
                    RelatedIds = new List<RelatedId>()
                };

                viewModel.RelatedIds.AddRange(movieToEdit.Categories.Select(c => new RelatedId { Id = c.category_id, Type = "category" }));
                viewModel.RelatedIds.AddRange(movieToEdit.actors.Select(a => new RelatedId { Id = a.actor_id, Type = "actor" }));
                viewModel.RelatedIds.AddRange(movieToEdit.directors.Select(d => new RelatedId { Id = d.director_id, Type = "director" }));
                viewModel.RelatedIds.AddRange(movieToEdit.Nations.Select(n => new RelatedId { Id = n.nation_id, Type = "nation" }));

                if (viewModel.IsSeries)
                {
                    viewModel.Episodes = movieToEdit.episodes.Select(x => new EpisodeViewModel
                    {
                        EpisodeNumber = x.episode_number,
                        Title = x.title,
                        Duration = x.duration,
                        Link = x.videos.FirstOrDefault()?.file_url,
                        SeasonNumber = x.season_number
                    }).ToList();
                }
                else
                {
                    viewModel.MovieLink = movieToEdit.videos.FirstOrDefault()?.file_url;
                }
                return Json(viewModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi cập nhật phim: " + ex.Message });
            }
        }


        [HttpPost]
        public ActionResult UpdateMovie(MovieViewModel model)
        {
            try
            {
                var movieToUpdate = movie.movies.FirstOrDefault(x => x.movie_id == model.MovieId);
                if (movieToUpdate == null)
                    return Json(new { success = false, message = "Phim không tồn tại!" });

                movieToUpdate.title = model.Title;
                movieToUpdate.release_date = model.ReleaseDate;
                movieToUpdate.duration = model.Duration;
                movieToUpdate.language = model.Language;
                movieToUpdate.thumbnail_url = model.ThumbnailUrl;
                movieToUpdate.trailer_url = model.TrailerUrl;
                movieToUpdate.description = model.Description;

                // Xóa các quan hệ cũ
                movieToUpdate.Categories.Clear();
                movieToUpdate.actors.Clear();
                movieToUpdate.directors.Clear();
                movieToUpdate.Nations.Clear();

                if (model.RelatedIds != null)
                {
                    foreach (var related in model.RelatedIds)
                    {
                        switch (related.Type)
                        {
                            case "category":
                                var category = movie.Categories.Find(related.Id);
                                if (category != null)
                                    movieToUpdate.Categories.Add(category);
                                break;
                            case "actor":
                                var actor = movie.actors.Find(related.Id);
                                if (actor != null)
                                    movieToUpdate.actors.Add(actor);
                                break;
                            case "director":
                                var director = movie.directors.Find(related.Id);
                                if (director != null)
                                    movieToUpdate.directors.Add(director);
                                break;
                            case "nation":
                                var nation = movie.Nations.Find(related.Id);
                                if (nation != null)
                                    movieToUpdate.Nations.Add(nation);
                                break;
                        }
                    }
                }

                var existingVideos = movie.videos.Where(v => v.movie_id == model.MovieId).ToList();
                foreach (var video in existingVideos)
                {
                    movie.videos.Remove(video);
                }

                var existingEpisodes = movie.episodes.Where(e => e.series_id == model.MovieId).ToList();
                foreach (var episode in existingEpisodes)
                {
                    movie.episodes.Remove(episode);
                }

                if (!model.IsSeries && !string.IsNullOrEmpty(model.MovieLink))
                {
                    var video = new video
                    {
                        movie_id = model.MovieId,
                        file_url = model.MovieLink,
                        format = "mp4",
                        resolution = "1080p"
                    };
                    movie.videos.Add(video);
                }


                if (model.IsSeries && model.Episodes != null)
                {
                    foreach (var episode in model.Episodes)
                    {
                        var newEpisode = new episode
                        {
                            series_id = model.MovieId,
                            season_number = episode.SeasonNumber,
                            episode_number = episode.EpisodeNumber,
                            duration = episode.Duration,
                            title = episode.Title,
                            air_date = model.ReleaseDate
                        };
                        movie.episodes.Add(newEpisode);
                        movie.SaveChanges();

                        var video = new video
                        {
                            episode_id = newEpisode.episode_id,
                            movie_id = model.MovieId,
                            file_url = episode.Link,
                            format = "mp4",
                            resolution = "1080p"
                        };
                        movie.videos.Add(video);
                    }
                }

                movie.SaveChanges();
                return Json(new { success = true, message = "Cập nhật phim thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi cập nhật phim: " + ex.Message });
            }
        }


        [HttpPost]
        public ActionResult DeleteMovie(int id)
        {
            try 
            {
                var movieToDelete = movie.movies.FirstOrDefault(x => x.movie_id == id);
                if (movieToDelete == null)
                    return Json(new { success = false, message = "Phim không tồn tại!" });
                var episodes = movie.episodes.Where(e => e.series_id == id).ToList();
                foreach (var episode in episodes)
                {
                    var videos = movie.videos.Where(v => v.episode_id == episode.episode_id).ToList();
                    foreach (var video in videos)
                    {
                        movie.videos.Remove(video);
                    }
                    movie.episodes.Remove(episode);
                }

                var videoToOne = movie.videos.Where(v => v.movie_id == id).ToList();
                foreach (var video in videoToOne)
                {
                    movie.videos.Remove(video);
                }

                movieToDelete.Categories.Clear();
                movieToDelete.actors.Clear();
                movieToDelete.directors.Clear();
                movieToDelete.Nations.Clear();

                movie.movies.Remove(movieToDelete);
                movie.SaveChanges();
                return Json(new { success = true, message = "Xóa phim thành công!" });

            } catch(Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi xóa phim: " + ex.Message });
            }
        }
    }
}