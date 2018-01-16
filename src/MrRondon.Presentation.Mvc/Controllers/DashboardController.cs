using System.Web.Mvc;

namespace MrRondon.Presentation.Mvc.Controllers
{
    public class DashboardController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
    }
}