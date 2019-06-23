using MovieCatalog.DomainModels;
using System;
using System.Web;
using System.Web.Mvc;

namespace MovieCatalog.Attributes
{
    /// <summary>
    /// Check user rights
    /// </summary>
    public class MovieAuthorizedAttribute : AuthorizeAttribute
    {
        private MoviesDBEntities _entities = new MoviesDBEntities();
        private bool _isAuthorizedToEditMovie;

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authorized = base.AuthorizeCore(httpContext);
            if(!authorized)
            {
                return false;
            }
            var userNameContext = httpContext.User.Identity.Name;
            var rd = httpContext.Request.RequestContext.RouteData;
            var id = rd.Values["id"];
            int movieId = Convert.ToInt32(id);

            //Load movie by Id
            Movie movie = _entities.MovieSet.Find(movieId);
            var userNameMovie = movie.UserName;

            if (userNameContext == userNameMovie)
            {
                _isAuthorizedToEditMovie = true;
            }
            else
            {
                _isAuthorizedToEditMovie = false;
            }
            return _isAuthorizedToEditMovie;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            //If user dont have a rights to edit movie show warning alert
            if (!_isAuthorizedToEditMovie)
            {
                filterContext.Controller.TempData.Add("RedirectReason", "Unauthorized");
            }
        }
    }
}