using Microsoft.AspNet.Identity;
using MovieCatalog.DomainModels;
using MovieCatalog.Attributes;
using PagedList;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MovieCatalog.ViewModels;
using System.Collections.Generic;

namespace MovieCatalog.Controllers
{

    public class MovieController : Controller
    {
        #region Private

        private MoviesDBEntities entities = new MoviesDBEntities();

        #endregion

        #region GET: Index

        /// <summary>
        /// GET: list of movies
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Index(int? page)
        {
            if (Request.IsAuthenticated)
            {
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(entities.MovieSet.OrderBy(x => x.Name).ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion

        #region GET: Details

        /// <summary>
        /// GET: View movie
        /// </summary>
        /// <param name="id">Movie id</param>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            Movie movieFromDB = entities.MovieSet.Find(id);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Movie, MovieViewModel>()).CreateMapper();
            MovieViewModel movieViewModel = mapper.Map<Movie, MovieViewModel>(movieFromDB);

            if (movieViewModel == null)
            {
                return HttpNotFound();
            }
            return View(movieViewModel);
        }

        #endregion

        #region GET: Create

        /// <summary>
        /// GET: Create new movie
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        #endregion

        #region POST: Create

        /// <summary>
        /// POST: Create new movie
        /// </summary>
        /// <param name="movie">movie</param>
        /// <param name="poster">poster</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create([Bind(Exclude = "Poster")]MovieViewModel movie, HttpPostedFileBase poster)
        {
            if (ModelState.IsValid)
            {
                // if poster != null, save image in localDB
                if (poster != null)
                {
                    movie.Poster = new byte[poster.ContentLength];
                    poster.InputStream.Read(movie.Poster, 0, poster.ContentLength);
                }

                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<MovieViewModel, Movie>()).CreateMapper();
                Movie movieSaveInDB = mapper.Map<MovieViewModel, Movie> (movie);

                movieSaveInDB.UserName = User.Identity.Name;
                entities.MovieSet.Add(movieSaveInDB);
                entities.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movie);
        }

        #endregion

        #region GET: Edit

        /// <summary>
        /// Edit movie
        /// </summary>
        /// <param name="id">movie id</param>
        /// <returns></returns>
        //[MovieAuthorized]
        public ActionResult Edit(int id)
        {
            Movie movieToEdit = entities.MovieSet.Find(id);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Movie, MovieViewModel>()).CreateMapper();
            MovieViewModel movieViewModel = mapper.Map<Movie, MovieViewModel>(movieToEdit);

            if (movieViewModel == null)
            {
                return HttpNotFound();
            }

            return View(movieViewModel);
        }

        #endregion

        #region POST: Edit

        /// <summary>
        /// POST: Edit movie
        /// </summary>
        /// <param name="movieToEdit">Movie id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(MovieViewModel movieToEdit)
        {
            if (ModelState.IsValid)
            {

                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<MovieViewModel, Movie>()).CreateMapper();
                Movie movieSaveInDB = mapper.Map<MovieViewModel, Movie>(movieToEdit); 
                entities.Entry(movieSaveInDB).State = EntityState.Modified;
                entities.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movieToEdit);
        }

        #endregion
    }
}
