using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MrRondon.Domain.Entities;
using MrRondon.Infra.CrossCutting.Helper;
using MrRondon.Infra.CrossCutting.Helper.Buttons;
using MrRondon.Infra.Data.Context;
using MrRondon.Infra.Data.Repositories;
using MrRondon.Presentation.Mvc.Extensions;

namespace MrRondon.Presentation.Mvc.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly MainContext _db = new MainContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            var repo = new RepositorioBase<Category>(_db);
            var category = repo.GetItemByExpression(x => x.CategoryId == id, "SubCategory");
            if (category == null) return HttpNotFound();
            return View(category);
        }

        public ActionResult Create()
        {
            ViewBag.SubCategories = new SelectList(_db.Categories.Where(s => s.SubCategoryId != null).OrderBy(o => o.Name), "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryId,Name,Image,SubCategoryId")] Category model, HttpPostedFileBase image)
        {
            try
            {
                if (image == null)
                {
                    ViewBag.SubCategories = new SelectList(_db.Categories.Where(s => s.SubCategoryId != null).OrderBy(o => o.Name), "CategoryId", "Name");
                    return View(model).Error("A imagem da categoria é obrigatória");
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.SubCategories = new SelectList(_db.Categories.Where(s => s.SubCategoryId != null).OrderBy(o => o.Name), "CategoryId", "Name");
                    return View(model);
                }

                var br = new BinaryReader(image.InputStream);
                model.SetImage(br.ReadBytes(image.ContentLength));
                br.Close();
                _db.Categories.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.SubCategories = new SelectList(_db.Categories.Where(s => s.SubCategoryId != null).OrderBy(o => o.Name), "CategoryId", "Name");
                return View(model).Error(ex.Message);
            }
        }

        public ActionResult Edit(int id)
        {
            var repo = new RepositorioBase<Category>(_db);
            var category = repo.GetItemByExpression(x => x.CategoryId == id, "SubCategory");
            if (category == null) return HttpNotFound();

            ViewBag.SubCategories = new SelectList(_db.Categories.Where(s => s.SubCategoryId != null).OrderBy(o => o.Name), "CategoryId", "Name");
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryId,Name,Image,SubCategoryId")] Category model, HttpPostedFileBase image)
        {
            try
            {
                if (image == null)
                {
                    ViewBag.SubCategories = new SelectList(_db.Categories.Where(s => s.SubCategoryId != null).OrderBy(o => o.Name), "CategoryId", "Name");
                    return View(model).Error("A imagem da categoria é obrigatória");
                }

                if (!ModelState.IsValid) return View(model);

                var br = new BinaryReader(image.InputStream);
                model.SetImage(br.ReadBytes(image.ContentLength));
                br.Close();
                _db.Entry(model).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.SubCategories = new SelectList(_db.Categories.Where(s => s.SubCategoryId != null).OrderBy(o => o.Name), "CategoryId", "Name");
                return View(model).Error(ex.Message);
            }
        }

        public ActionResult Delete(int id)
        {
            var repo = new RepositorioBase<Category>(_db);
            var category = repo.GetItemByExpression(x => x.CategoryId == id, "SubCategory");
            if (category == null) return HttpNotFound();
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
            var search = parameters.Search.Value?.ToLower() ?? string.Empty;
            var repo = new RepositorioBase<Category>(_db);
            var items = repo.GetItemsByExpression(w => w.Name.Contains(search), x => x.Name, 0, 10, out var recordsTotal).ToList();
            var dtResult = new DataTableResultSet(parameters.Draw, recordsTotal);

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