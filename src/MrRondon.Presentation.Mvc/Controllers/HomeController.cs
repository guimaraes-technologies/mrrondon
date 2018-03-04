using System.Web.Mvc;

namespace MrRondon.Presentation.Mvc.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
    }
}