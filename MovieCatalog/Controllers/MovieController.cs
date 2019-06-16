using Microsoft.AspNet.Identity;
using MovieCatalog.Models;
using MovieCatalog.Attributes;
using PagedList;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
                return View("Index", "Home");
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
            Movie viewMovie = entities.MovieSet.Find(id);
            if (viewMovie == null)
            {
                return HttpNotFound();
            }
            return View(viewMovie);
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
        public ActionResult Create([Bind(Exclude = "Id, poster")] Movie movie, HttpPostedFileBase poster)
        {
            if (ModelState.IsValid)
            {
                // if poster != null, save image in localDB
                if (poster != null)
                {
                    movie.Poster = new byte[poster.ContentLength];
                    poster.InputStream.Read(movie.Poster, 0, poster.ContentLength);
                }
                movie.UserName = User.Identity.Name;
                entities.MovieSet.Add(movie);
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
        [MovieAuthorized]
        public ActionResult Edit(int id)
        {
            Movie movieToEdit = entities.MovieSet.Find(id);
            if (movieToEdit == null)
            {
                return HttpNotFound();
            }

            return View(movieToEdit);
        }

        #endregion

        #region POST: Edit

        /// <summary>
        /// POST: Edit movie
        /// </summary>
        /// <param name="movieToEdit">Movie id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Movie movieToEdit)
        {
            if (ModelState.IsValid)
            {
                entities.Entry(movieToEdit).State = EntityState.Modified;
                entities.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movieToEdit);
        }

        #endregion
    }
}
