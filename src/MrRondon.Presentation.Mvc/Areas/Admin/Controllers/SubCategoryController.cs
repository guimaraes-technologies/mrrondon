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
    public class SubCategoryController : Controller
    {
        private readonly MainContext _db = new MainContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            var repo = new RepositoryBase<SubCategory>(_db);
            var category = repo.GetItemByExpression(x => x.CategoryId == id, "Category");
            if (category == null) return HttpNotFound();
            return View(category);
        }

        public ActionResult Create()
        {
            ViewBag.Categories = new SelectList(_db.SubCategories.Where(s => s.CategoryId == null).OrderBy(o => o.Name), "SubCategoryId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SubCategory model, HttpPostedFileBase image)
        {
            try
            {
                if (image == null)
                {
                    return View(model).Error("A imagem da sub categoria é obrigatória");
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.Categories = new SelectList(_db.SubCategories.Where(s => s.CategoryId == null).OrderBy(o => o.Name), "SubCategoryId", "Name", model.SubCategoryId);
                    return View(model);
                }

                var br = new BinaryReader(image.InputStream);
                model.SetImage(br.ReadBytes(image.ContentLength));
                br.Close();
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
            var category = repo.GetItemByExpression(x => x.CategoryId == id);
            if (category == null) return HttpNotFound();

            ViewBag.Categories = new SelectList(_db.SubCategories.Where(s => s.CategoryId == null).OrderBy(o => o.Name), "SubCategoryId", "Name", category.SubCategoryId);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SubCategory model, HttpPostedFileBase image)
        {
            try
            {
                if (image == null)
                {
                    ViewBag.Categories = new SelectList(_db.SubCategories.Where(s => s.CategoryId == null).OrderBy(o => o.Name), "SubCategoryId", "Name", model.SubCategoryId);
                    return View(model).Error("A imagem da categoria é obrigatória");
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.Categories = new SelectList(_db.SubCategories.Where(s => s.CategoryId == null).OrderBy(o => o.Name), "SubCategoryId", "Name", model.SubCategoryId);
                    return View(model);
                }

                var br = new BinaryReader(image.InputStream);
                model.SetImage(br.ReadBytes(image.ContentLength));
                br.Close();
                _db.Entry(model).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Categories = new SelectList(_db.SubCategories.Where(s => s.CategoryId == null).OrderBy(o => o.Name), "SubCategoryId", "Name", model.SubCategoryId);
                return View(model).Error(ex.Message);
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var category = _db.SubCategories.Find(id);
                if (category == null) return RedirectToAction("Index").Success("Categoria removida com sucesso");

                _db.SubCategories.Remove(category);
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
            var repo = new RepositoryBase<SubCategory>(_db);
            var items = repo.GetItemsByExpression(w => w.CategoryId == null && w.Name.Contains(search)).ToList();
            var dtResult = new DataTableResultSet(parameters.Draw, 10);

            var buttons = new ButtonsSubCategory();
            foreach (var item in items.ToList())
            {
                dtResult.data.Add(new[]
                {
                    item.CategoryId.ToString(),
                    $"{item.Name}",
                    $"{item.Category?.Name ?? "Não informada"}",
                    buttons.ToPagination(item.SubCategoryId)
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