using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MrRondon.Infra.CrossCutting.Helper;

namespace MrRondon.Presentation.Mvc.Areas.Admin.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        public ActionResult Code(int id)
        {
            switch (id)
            {
                case Constants.Codes.Unauthorized: return View("Erro401");
                case Constants.Codes.NotFound: return View("Erro404");
                default: return View("Generic");
            }
        }

        public static ICollection<string> Get(ModelStateDictionary modelState)
        {
            var errors= modelState.Values.Where(x => x.Errors.Any()).SelectMany(s => s.Errors).Select(item => $"- {item.ErrorMessage}").ToList();

            return errors;
        }

        public class Error
        {
            public ICollection<string> Errors { get; set; } = new List<string>();
        }
    }
}