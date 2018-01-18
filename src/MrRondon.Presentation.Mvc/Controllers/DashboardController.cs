using System.Web.Mvc;
using MrRondon.Presentation.Mvc.Extensions;

namespace MrRondon.Presentation.Mvc.Controllers
{
    public class DashboardController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View().Success("Welcome");
        }
    }
}