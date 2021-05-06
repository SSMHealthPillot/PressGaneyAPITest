using System.Web.Mvc;

namespace PhyndAPI.UI.Controllers
{
    public class HttpErrorController : DefaultController
    {
        // GET: HttpError
        public ActionResult HttpRequestValidationError()
        {
            return View();
        }

        public ActionResult AccessDenied(string appName)
        {
            ViewBag.ApplicationName = appName;
            return View();
        }
    }
}