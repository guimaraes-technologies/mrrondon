using MrRondon.Domain.Entities;
using MrRondon.Infra.CrossCutting.Helper;
using MrRondon.Infra.CrossCutting.Helper.Buttons;
using MrRondon.Infra.Data.Context;
using MrRondon.Infra.Data.Repositories;
using MrRondon.Presentation.Mvc.Extensions;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MrRondon.Infra.Security.Extensions;
using MrRondon.Infra.Security.Helpers;

namespace MrRondon.Presentation.Mvc.Areas.Admin.Controllers
{
    public class SubCategoryController : Controller
    {
        private readonly MainContext _db = new MainContext();

        [HasAny(Constants.Roles.GeneralAdministrator, Constants.Roles.SubCategoryAdministrator, Constants.Roles.ReadOnly)]
        public ActionResult Index()
        {
            return View();
        }

        [HasAny(Constants.Roles.GeneralAdministrator, Constants.Roles.SubCategoryAdministrator, Constants.Roles.ReadOnly)]
        public ActionResult Details(int id)
        {
            var repo = new RepositoryBase<SubCategory>(_db);
            var sub = repo.GetItemByExpression(x => x.SubCategoryId == id, x => x.Category);
            if (sub == null) return HttpNotFound();
            return View(sub);
        }

        [HasAny(Constants.Roles.GeneralAdministrator, Constants.Roles.SubCategoryAdministrator)]
        public ActionResult Create()
        {
            ViewBag.Categories = new SelectList(_db.SubCategories.Where(s => s.CategoryId == null).OrderBy(o => o.Name), "SubCategoryId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasAny(Constants.Roles.GeneralAdministrator, Constants.Roles.SubCategoryAdministrator)]
        public ActionResult Create(SubCategory model, HttpPostedFileBase image)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Categories = new SelectList(_db.SubCategories.Where(s => s.CategoryId == null).OrderBy(o => o.Name), "SubCategoryId", "Name", model.SubCategoryId);
                    return View(model);
                }

                _db.SubCategories.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Categories = new SelectList(_db.SubCategories.Where(s => s.CategoryId == null).OrderBy(o => o.Name), "SubCategoryId", "Name", model.SubCategoryId);
                return View(model).Error(ex.Message);
            }
        }

        public ActionResult Edit(int id)
        {
            var repo = new RepositoryBase<SubCategory>(_db);
            var category = repo.GetItemByExpression(x => x.SubCategoryId == id);
            if (category == null) return HttpNotFound();

            ViewBag.Categories = new SelectList(_db.SubCategories.Where(s => s.CategoryId == null).OrderBy(o => o.Name), "SubCategoryId", "Name", category.CategoryId);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasAny(Constants.Roles.GeneralAdministrator, Constants.Roles.SubCategoryAdministrator)]
        public ActionResult Edit(SubCategory model, HttpPostedFileBase image)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Categories = new SelectList(_db.SubCategories.Where(s => s.CategoryId == null).OrderBy(o => o.Name), "SubCategoryId", "Name", model.CategoryId);
                    return View(model);
                }

                _db.Entry(model).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Categories = new SelectList(_db.SubCategories.Where(s => s.CategoryId == null).OrderBy(o => o.Name), "SubCategoryId", "Name", model.CategoryId);
                return View(model).Error(ex.Message);
            }
        }

        public JsonResult GetSubCategories(int categoryId)
        {
            var results = _db.SubCategories.Where(x => x.CategoryId == categoryId).Select(x => new
            {
                name = x.Name,
                text = x.Name,
                value = x.SubCategoryId
            }).ToList();

            return Json(new { results }, JsonRequestBehavior.AllowGet);
        }

        [HasAny(Constants.Roles.GeneralAdministrator, Constants.Roles.SubCategoryAdministrator)]
        public ActionResult ShowOnApp(int id)
        {
            var repo = new RepositoryBase<SubCategory>(_db);
            var category = repo.GetItemByExpression(x => x.SubCategoryId == id);
            if (category == null) return HttpNotFound();
            _db.Entry(category).Property(p => p.ShowOnApp).CurrentValue = !category.ShowOnApp;
            _db.SaveChanges();

            return RedirectToAction("Index").Success("Operação realizada com sucesso");
        }

        [HttpPost]
        [HasAny(Constants.Roles.GeneralAdministrator, Constants.Roles.SubCategoryAdministrator, Constants.Roles.ReadOnly)]
        public JsonResult GetPagination(DataTableParameters parameters)
        {
            var search = parameters.Search.Value?.ToLower() ?? string.Empty;
            var repo = new RepositoryBase<SubCategory>(_db);
            var items = repo.GetItemsByExpression(w => w.CategoryId.HasValue && w.Name.Contains(search), x => x.Name, parameters.Start, parameters.Length, out var recordsTotal, x => x.Category).ToList();
            var dtResult = new DataTableResultSet(parameters.Draw, recordsTotal);

            var buttons = new ButtonsSubCategory();
            foreach (var item in items.ToList())
            {
                dtResult.data.Add(new object[]
                {
                    item.Name,
                    $"{item.Category?.Name ?? "Não informada"}",
                    buttons.ToPagination(item.SubCategoryId, item.ShowOnApp, Account.Current.Roles)
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