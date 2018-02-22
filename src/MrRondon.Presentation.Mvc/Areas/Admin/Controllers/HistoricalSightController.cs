using System;
using System.Linq;
using System.Data.Entity;
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
    public class HistoricalSightController : Controller
    {
        private readonly MainContext _db = new MainContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            SetBiewBags(null);
            return View();
        }

        [HttpPost]
        public ActionResult Create(CrudHistoricalSightVm model, Address address)
        {
            try
            {
                model.HistoricalSight.Address = address;
                model.HistoricalSight.Address.SetCoordinates(address.LatitudeString, address.LongitudeString);

                if (model.HistoricalSight.Logo == null || model.LogoFile != null)
                    model.HistoricalSight.Logo = FileUpload.GetBytes(model.LogoFile, "Logo");
                if (model.HistoricalSight.Cover == null || model.CoverFile != null)
                    model.HistoricalSight.Cover = FileUpload.GetBytes(model.CoverFile, "Capa");

                ModelState.Remove("HistoricalSight.Logo");
                ModelState.Remove("HistoricalSight.Cover");
                if (!ModelState.IsValid)
                {
                    SetBiewBags(model);
                    return View(model).Error(ModelState);
                }

                _db.HistoricalSights.Add(model.HistoricalSight);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                SetBiewBags(model);
                return View(model).Error(ex.Message);
            }
        }

        public ActionResult Edit(int id)
        {
            var repo = new RepositoryBase<HistoricalSight>(_db);
            var historicalSight = repo.GetItemByExpression(x => x.HistoricalSightId == id, "Address");
            if (historicalSight == null) return HttpNotFound();

            var crud = new CrudHistoricalSightVm
            {
                HistoricalSight = historicalSight
            };
            crud.HistoricalSight.Address.SetCoordinates();
            SetBiewBags(crud);

            return View(crud);
        }

        [HttpPost]
        public ActionResult Edit(CrudHistoricalSightVm model, Address address)
        {
            try
            {
                model.HistoricalSight.Address = address;
                model.HistoricalSight.Address.SetCoordinates(address.LatitudeString, address.LongitudeString);

                if (model.HistoricalSight.Logo == null || model.LogoFile != null)
                    model.HistoricalSight.Logo = FileUpload.GetBytes(model.LogoFile, "Logo");

                if (model.HistoricalSight.Cover == null || model.CoverFile != null)
                    model.HistoricalSight.Cover = FileUpload.GetBytes(model.CoverFile, "Capa");

                ModelState.Remove("HistoricalSight.Logo");
                ModelState.Remove("HistoricalSight.Cover");

                if (!ModelState.IsValid)
                {
                    SetBiewBags(model);
                    return View(model).Error(ModelState);
                }

                var oldHistoricalSight = _db.HistoricalSights
                    .Include(c => c.Address)
                    .FirstOrDefault(x => x.HistoricalSightId == model.HistoricalSight.HistoricalSightId);

                if (oldHistoricalSight == null) return RedirectToAction("Index").Success("Patrimônio Histórico atualizado com sucesso");
                
                _db.Entry(oldHistoricalSight).CurrentValues.SetValues(model.HistoricalSight);

                oldHistoricalSight.Address.UpdateAddress(model.HistoricalSight.Address);
                _db.Entry(oldHistoricalSight.Address).State = EntityState.Modified;
                _db.SaveChanges();

                return RedirectToAction("Index").Success("Patrimônio Histórico atualizado com sucesso");
            }
            catch (Exception ex)
            {
                SetBiewBags(model);

                return View(model).Error(ex.Message);
            }
        }

        [HttpPost]
        public JsonResult GetPagination(DataTableParameters parameters)
        {
            var search = parameters.Search.Value?.ToLower() ?? string.Empty;
            var repo = new RepositoryBase<HistoricalSight>(_db);
            var items = repo.GetItemsByExpression(w => w.Name.Contains(search), x => x.Name, parameters.Start, parameters.Length, out var recordsTotal).ToList();
            var dtResult = new DataTableResultSet(parameters.Draw, recordsTotal);

            var buttons = new ButtonsHistoricalSight();
            foreach (var item in items)
            {
                dtResult.data.Add(new[]
                {
                    item.HistoricalSightId.ToString(),
                    $"{buttons.Image(item.Logo)}",
                    $"{item.Name}",
                    buttons.ToPagination(item.HistoricalSightId)
                });
            }
            return Json(dtResult, JsonRequestBehavior.AllowGet);
        }

        private void SetBiewBags(CrudHistoricalSightVm model)
        {
            ViewBag.Cities = new SelectList(_db.Cities, "CityId", "Name", model?.HistoricalSight?.Address?.CityId);
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