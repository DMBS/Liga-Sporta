using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MovieCatalog.Models;
using PagedList;
using Microsoft.AspNet.Identity;
using System.IO;

namespace MovieCatalog.Controllers
{
    public class MovieController : Controller
    {
        private MoviesDBEntities entities = new MoviesDBEntities();

        // GET: Movie
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (Request.IsAuthenticated)
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.RealesedDateSortParam = sortOrder == "Date" ? "date_desc" : "Date";
                ViewBag.DirectorSortParam = sortOrder == "Director" ? "director_desc" : "Director";
                ViewBag.UserSortParam = sortOrder == "User" ? "user_desc" : "User";

                var movies = from s in entities.MovieSet
                               select s;

                switch (sortOrder)
                {
                    case "Name":
                        movies = movies.OrderBy(s => s.Name);
                        break;
                    case "name_desc":
                        movies = movies.OrderByDescending(s => s.Name);
                        break;
                    case "Date":
                        movies = movies.OrderBy(s => s.DateReleased);
                        break;
                    case "date_desc":
                        movies = movies.OrderByDescending(s => s.DateReleased);
                        break;
                    case "Director":
                        movies = movies.OrderBy(s => s.Director);
                        break;
                    case "director_desc":
                        movies = movies.OrderByDescending(s => s.Director);
                        break;
                    case "User":
                        movies = movies.OrderBy(s => s.UserName);
                        break;
                    case "user_desc":
                        movies = movies.OrderByDescending(s => s.UserName);
                        break;
                    default:  // Name ascending 
                        movies = movies.OrderBy(s => s.Name);
                        break;
                }

                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(entities.MovieSet.OrderByDescending(x => x.Name).ToPagedList(pageNumber, pageSize));
                //return View(entities.MovieSet.ToList());
            }
            else
            {
                return View("Index", "Home");
            }
        }


        // GET: Movie/Details/5
        public ActionResult Details(int id)
        {
            Movie viewMovie = entities.MovieSet.Find(id);
            if (viewMovie == null)
            {
                return HttpNotFound();
            }
            return View(viewMovie);
        }

        // GET: Movie/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movie/Create
        [HttpPost]
        public ActionResult Create([Bind(Exclude = "Id,poster")] Movie movie, HttpPostedFileBase poster)
        {
            if (ModelState.IsValid)
            {
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

        // GET: Movie/Edit/5
        public ActionResult Edit(int id)
        {
            Movie movieToEdit = entities.MovieSet.Find(id);
            if (movieToEdit == null)
            {
                return HttpNotFound();
            }
            if (User.Identity.GetUserName() != movieToEdit.UserName)
            {
                TempData["Danger"] = "Each user can edit only their movies! Please Back to list and choose your movie.";
            }
            return View(movieToEdit);
        }

        // POST: Movie/Edit/5
        [HttpPost]
        public ActionResult Edit(Movie movieToEdit)

        {

            if(User.Identity.GetUserName() != movieToEdit.UserName)
            {
                return new HttpUnauthorizedResult();
            }

            if (ModelState.IsValid)

            {

                entities.Entry(movieToEdit).State = EntityState.Modified;

                entities.SaveChanges();

                return RedirectToAction("Index");

            }

            return View(movieToEdit);

        }

        //// GET: Movie/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Movie/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
