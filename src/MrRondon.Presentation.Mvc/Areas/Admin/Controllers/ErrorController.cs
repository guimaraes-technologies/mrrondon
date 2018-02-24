using System.Web.Mvc;

namespace MrRondon.Presentation.Mvc.Areas.Admin.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        public ActionResult Code(int id)
        {
            switch (id)
            {
                case 404: return View("Erro404");
                default: return View("Generic");
            }
        }
    }
}