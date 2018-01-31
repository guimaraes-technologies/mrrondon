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

namespace MrRondon.Presentation.Mvc.Areas.Admin.Controllers
{
    [AllowAnonymous]
    public class CategoryController : Controller
    {
        private readonly MainContext _db = new MainContext();

        public ActionResult Index()
        {
            return View(_db.Categories.Where(x => x.SubCategoryId == null).ToList());
        }

        public ActionResult Details(int id)
        {
            var category = _db.Categories.Find(id);
            if (category == null) return HttpNotFound();
            return View(category);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryId,Name,Image,SubCategoryId")] Category category)
        {
            if (!ModelState.IsValid) return View(category);

            _db.Categories.Add(category);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var category = _db.Categories.Find(id);
            if (category == null) return HttpNotFound();
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryId,Name,Image,SubCategoryId")] Category category)
        {
            if (!ModelState.IsValid) return View(category);

            _db.Entry(category).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var category = _db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var category = _db.Categories.Find(id);
                if (category == null) return RedirectToAction("Index").Success("Categoria removida com sucesso");

                _db.Categories.Remove(category);
                _db.SaveChanges();
                return RedirectToAction("Index").Success("Categoria removida com sucesso");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index").Error(ex.Message);
            }
        }

        [HttpPost]
        public JsonResult GetPagination(DataTableParameters parameters)
        {
            int recordsTotal;
            var search = parameters.Search.Value?.ToLower() ?? string.Empty;
            var repo = new RepositorioBase<Category>(_db);
            var items = repo.GetItemsByExpression(w => w.Name.Contains(search), x => x.Name, 0, 10, out recordsTotal).ToList();
            var dtResult = new DataTableResultSet(parameters.Draw, items.Count);

            var buttons = new ButtonsCategory();
            foreach (var item in items)
            {
                dtResult.data.Add(new[]
                {
                    item.CategoryId.ToString(),
                    $"{item.Name}",
                    buttons.ToPagination(item.CategoryId)
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