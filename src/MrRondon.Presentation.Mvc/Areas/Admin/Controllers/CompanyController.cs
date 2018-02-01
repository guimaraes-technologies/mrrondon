using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MrRondon.Domain.Entities;
using MrRondon.Infra.CrossCutting.Helper;
using MrRondon.Infra.CrossCutting.Helper.Buttons;
using MrRondon.Infra.Data.Context;
using MrRondon.Infra.Data.Repositories;
using MrRondon.Presentation.Mvc.Extensions;
using MrRondon.Presentation.Mvc.ViewModels;

namespace MrRondon.Presentation.Mvc.Areas.Admin.Controllers
{
    public class CompanyController : Controller
    {
        private readonly MainContext _db = new MainContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(Guid id)
        {
            var repo = new RepositorioBase<Company>(_db);
            var model = repo.GetItemByExpression(x => x.CompanyId == id);
            if (model == null) return HttpNotFound();
            return View(new CompanyAddressVm
            {
                Company = model,
            });
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CompanyAddressVm model, int categoryId, int? subCategoryId)
        {
            try
            {
                ModelState.Remove(nameof(model.Company.SegmentId));
                if (!ModelState.IsValid) return View(model);

                model.Company.SegmentId = subCategoryId ?? categoryId;
                _db.Companies.Add(model.Company);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(model).Error(ex.Message);
            }
        }

        public ActionResult Edit(Guid id)
        {
            var repo = new RepositorioBase<Company>(_db);
            var model = repo.GetItemByExpression(x => x.CompanyId == id, "Segment");
            if (model == null) return HttpNotFound();

            return View(new CompanyAddressVm
            {
                Company = model,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Company model, int categoryId, int? subCategoryId)
        {
            var company = new CompanyAddressVm { Company = model };
            try
            {
                ModelState.Remove(nameof(model.SegmentId));
                if (!ModelState.IsValid) return View(company);

                model.SegmentId = subCategoryId ?? categoryId;
                _db.Entry(model).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(company).Error(ex.Message);
            }
        }

        [HttpPost]
        public JsonResult GetPagination(DataTableParameters parameters)
        {
            int recordsTotal;
            var search = parameters.Search.Value?.ToLower() ?? string.Empty;
            var repo = new RepositorioBase<Company>(_db);
            var items = repo.GetItemsByExpression(w => w.Name.Contains(search), x => x.Name, parameters.Start, parameters.Length, out recordsTotal).ToList();
            var dtResult = new DataTableResultSet(parameters.Draw, recordsTotal);

            var buttons = new ButtonsCompany();
            foreach (var item in items)
            {
                dtResult.data.Add(new[]
                {
                    item.CompanyId.ToString(),
                    $"{item.Name}",
                    buttons.ToPagination(item.CompanyId)
                });
            }
            return Json(dtResult, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                GC.SuppressFinalize(this);
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}