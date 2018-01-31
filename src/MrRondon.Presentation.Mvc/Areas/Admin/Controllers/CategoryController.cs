using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MrRondon.Domain.Entities;
using MrRondon.Infra.Data.Context;
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