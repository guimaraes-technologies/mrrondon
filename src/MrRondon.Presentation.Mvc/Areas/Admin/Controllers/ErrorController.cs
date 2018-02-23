using System.Web.Mvc;

namespace MrRondon.Presentation.Mvc.Areas.Admin.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        public ActionResult Code(int code)
        {
            switch (code)
            {
                case 404: return View("Erro404");
                default: return View("Generic");
            }
        }
    }
}