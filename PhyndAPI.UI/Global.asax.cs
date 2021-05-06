using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using PhyndAPI.DATA.Models;

namespace PhyndAPI.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(ExceptionContext filterContext)
        {
            try
            {
                ErrorLogging error = new ErrorLogging();
                error.StackTrace = filterContext.Exception.StackTrace;
                error.Message = filterContext.Exception.Message;

                error.JSON += "{";

                foreach (var viewDataItem in filterContext.Controller.ViewData)
                {
                    if (error.JSON != "{")
                        error.JSON += ", ";

                    error.JSON += viewDataItem.Key.ToString() + ": " + viewDataItem.Value.ToString();
                }
                error.JSON += "}";
                error.Save();

                if (!filterContext.ExceptionHandled && filterContext.Exception is HttpRequestValidationException)
                {
                    filterContext.Result = new RedirectResult("~/HttpError/HttpRequestValidationError");
                    filterContext.ExceptionHandled = true;
                }
            }
            catch
            {
                //this should run if there is another issue like a network error or a sql server issue.
                HttpContext.Current.Response.Redirect("~/HttpError/HttpRequestValidationError");
            }
        }
    }
}
