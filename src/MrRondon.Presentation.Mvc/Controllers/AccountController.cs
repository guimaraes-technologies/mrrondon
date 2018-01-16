using System.Web.Mvc;

namespace MrRondon.Presentation.Mvc.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Signin(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginVm { ReturnUrl = returnUrl });
        }

        [AllowAnonymous, HttpPost]
        public ActionResult Signin(LoginVm model)
        {
            return View(model);
        }
    }
}