using System;
using System.Web.Mvc;
using MrRondon.Infra.Data.Repositories;
using MrRondon.Presentation.Mvc.Extensions;

namespace MrRondon.Presentation.Mvc.Controllers
{
    public class DatabaseManagerController : Controller
    {
        [AllowAnonymous]
        public ActionResult Update()
        {
            try
            {
                var databaseManager = new DataBaseManagerRepository();
                databaseManager.UpdateToLastedVersion();
                return RedirectToAction("Index", "Home").Success("Banco de dados atualizado com sucesso");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home").Error(ex.Message);
            }
        }
    }
}