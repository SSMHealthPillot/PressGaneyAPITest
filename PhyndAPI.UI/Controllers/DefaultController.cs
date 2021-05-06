using System.Web.Mvc;

namespace PhyndAPI.UI.Controllers
{
    [Models.CustomActionFilter]
    public abstract class DefaultController : Controller
    {

        public DefaultController()
        {

        }
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            //add any windows auth information here.
        }
    }
}