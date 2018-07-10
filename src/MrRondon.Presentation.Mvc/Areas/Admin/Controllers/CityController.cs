using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MrRondon.Domain.Entities;
using MrRondon.Infra.CrossCutting.Helper;
using MrRondon.Infra.CrossCutting.Helper.Buttons;
using MrRondon.Infra.Data.Context;
using MrRondon.Infra.Data.Repositories;
using MrRondon.Infra.Security.Extensions;
using MrRondon.Infra.Security.Helpers;
using MrRondon.Presentation.Mvc.Extensions;

namespace MrRondon.Presentation.Mvc.Areas.Admin.Controllers
{
    public class CityController : Controller
    {
        private readonly MainContext _db = new MainContext();

        [HasAny(Constants.Roles.GeneralAdministrator, Constants.Roles.CityAdministrator, Constants.Roles.ReadOnly)]
        public ActionResult Index()
        {
            return View();
        }

        [HasAny(Constants.Roles.GeneralAdministrator, Constants.Roles.CityAdministrator, Constants.Roles.ReadOnly)]
        public ActionResult Details(int id)
        {
            var repo = new RepositoryBase<City>(_db);
            var city = repo.GetItemByExpression(x => x.CityId == id);
            if (city == null) return HttpNotFound();
            return View(city);
        }

        [HasAny(Constants.Roles.GeneralAdministrator, Constants.Roles.CityAdministrator)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasAny(Constants.Roles.GeneralAdministrator, Constants.Roles.CityAdministrator)]
        public ActionResult Create(City model)
        {
            try
            {
                if (!ModelState.IsValid) return View(model);

                _db.Cities.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(model).Error(ex.Message);
            }
        }

        [HasAny(Constants.Roles.GeneralAdministrator, Constants.Roles.CityAdministrator)]
        public ActionResult Edit(int id)
        {
            var repo = new RepositoryBase<City>(_db);
            var city = repo.GetItemByExpression(x => x.CityId == id);
            if (city == null) return HttpNotFound();

            return View(city);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasAny(Constants.Roles.GeneralAdministrator, Constants.Roles.CityAdministrator)]
        public ActionResult Edit(City model)
        {
            try
            {
                if (!ModelState.IsValid) return View(model);

                _db.Entry(model).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(model).Error(ex.Message);
            }
        }

        [HttpPost]
        [HasAny(Constants.Roles.GeneralAdministrator, Constants.Roles.CityAdministrator, Constants.Roles.ReadOnly)]
        public JsonResult GetPagination(DataTableParameters parameters)
        {
            var search = parameters.Search.Value?.ToLower() ?? string.Empty;
            var repo = new RepositoryBase<City>(_db);
            var items = repo.GetItemsByExpression(w => w.Name.Contains(search), x => x.Name, parameters.Start, parameters.Length, out var recordsTotal).ToList();
            var dtResult = new DataTableResultSet(parameters.Draw, recordsTotal);

            var buttons = new ButtonsCity();
            foreach (var item in items)
            {
                dtResult.data.Add(new object[]
                {
                    item.Name,
                    buttons.ToPagination(item.CityId, Account.Current.Roles)
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