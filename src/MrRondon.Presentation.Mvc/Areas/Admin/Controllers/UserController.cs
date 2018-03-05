using System.Linq;
using System.Web.Mvc;
using MrRondon.Domain.Entities;
using MrRondon.Infra.CrossCutting.Helper;
using MrRondon.Infra.CrossCutting.Helper.Buttons;
using MrRondon.Infra.Data.Context;
using MrRondon.Infra.Data.Repositories;

namespace MrRondon.Presentation.Mvc.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private readonly MainContext _db = new MainContext();

        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public JsonResult CpfEstaEmUso(string value)
        {
            return Json(!_usuarioAppService.CpfEstaEmUso(value), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPagination(DataTableParameters parameters)
        {
            var search = parameters.Search.Value?.ToLower() ?? string.Empty;
            var repo = new RepositoryBase<User>(_db);
            var items = repo.GetItemsByExpression(w => w.FullName.Contains(search) || w.Cpf.Replace(".", "").Replace("-", "").Contains(search), x => x.FullName, parameters.Start, parameters.Length, out var recordsTotal)
                .Select(s => new
                {
                    s.UserId,
                    s.FullName,
                    s.Cpf,
                    s.IsActive
                }).ToList();
            var dtResult = new DataTableResultSet(parameters.Draw, recordsTotal);

            var buttonsEntidade = new ButtonsUser();
            foreach (var item in items)
            {
                dtResult.data.Add(new[]
                {
                    item.IsActive.ToString(),
                    item.FullName,
                    item.Cpf,
                    buttonsEntidade.ToPagination(item.UserId, item.IsActive)
                });
            }
            return Json(dtResult, JsonRequestBehavior.AllowGet);
        }

    }
}