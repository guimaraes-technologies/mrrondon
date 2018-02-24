using System;
using System.Collections.Generic;
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
    public class EventController : Controller
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
        public ActionResult Create(CrudEventVm model, AddressForEventVm address)
        {
            try
            {
                model.Event.Address = model.GetAddress(address);
                model.Event.Address.SetCoordinates(address.LatitudeString, address.LongitudeString);

                if (model.Event.Logo == null || model.LogoFile != null)
                    model.Event.Logo = FileUpload.GetBytes(model.LogoFile, "Logo");
                if (model.Event.Cover == null || model.CoverFile != null)
                    model.Event.Cover = FileUpload.GetBytes(model.CoverFile, "Capa");

                ModelState.Remove("Event.Logo");
                ModelState.Remove("Event.Cover");
                if (!ModelState.IsValid)
                {
                    SetBiewBags(model);
                    return View(model);
                }

                _db.Events.Add(model.Event);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                SetBiewBags(model);
                return View(model).Error(ex.Message);
            }
        }

        public ActionResult Details(Guid id)
        {
            var repo = new RepositoryBase<Event>(_db);
            var entity = repo.GetItemByExpression(x => x.EventId == id, x => x.Address.City, i => i.Organizer);
            if (entity == null) return HttpNotFound();
            return View(entity);
        }

        public ActionResult Edit(Guid id)
        {
            var repo = new RepositoryBase<Event>(_db);
            var entity = repo.GetItemByExpression(x => x.EventId == id, x => x.Address);
            if (entity == null) return HttpNotFound();

            var model = GetCrudVm(entity);
            SetBiewBags(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Event entity, AddressForEventVm address)
        {
            var model = new CrudEventVm { Event = entity };
            try
            {
                model.Event.Address = model.GetAddress(address);
                model.Event.Address.SetCoordinates(address.LatitudeString, address.LongitudeString);

                if (model.Event.Logo == null || model.LogoFile != null)
                    model.Event.Logo = FileUpload.GetBytes(model.LogoFile, "Logo");
                if (model.Event.Cover == null || model.CoverFile != null)
                    model.Event.Cover = FileUpload.GetBytes(model.CoverFile, "Capa");

                ModelState.Remove("Event.Logo");
                ModelState.Remove("Event.Cover");
                if (!ModelState.IsValid)
                {
                    SetBiewBags(model);
                    return View(model);
                }

                _db.Entry(entity).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                SetBiewBags(model);

                return View(model).Error(ex.Message);
            }
        }

        [AllowAnonymous]
        public ActionResult Contacts(CrudEventVm eventContact)
        {
            UrlsContact();
            eventContact = eventContact ?? new CrudEventVm();
            eventContact.Contacts = eventContact.Contacts ?? new List<Contact>();
            return PartialView("_Contacts", eventContact.Contacts);
        }

        [AllowAnonymous, HttpPost]
        public ActionResult AddContact(CrudEventVm eventContact)
        {
            eventContact.Contacts = eventContact.Contacts ?? new List<Contact>();
            eventContact.Contacts.Add(new Contact { Description = eventContact.Description, ContactType = eventContact.ContactType });
            eventContact.Description = string.Empty;
            eventContact.ContactType = 0;
            UrlsContact();
            return PartialView("_Contacts", eventContact.Contacts);
        }

        [AllowAnonymous, HttpPost]
        public ActionResult RemoveContact(CrudEventVm eventContact, int index)
        {
            UrlsContact();
            eventContact.Contacts?.RemoveAt(index);
            return PartialView("_Contacts", eventContact.Contacts);
        }

        public void UrlsContact()
        {
            ViewBag.UrlAdd = Url.Action("AddContact", "Event", new { area = "Admin" });
            ViewBag.UrlRemove = Url.Action("RemoveContact", "Event", new { area = "Admin" });
        }

        private static CrudEventVm GetCrudVm(Event model)
        {
            var eventVm = new CrudEventVm { Event = model };

            return eventVm;
        }

        private void SetBiewBags(CrudEventVm model)
        {
            string cityId;
            if (model?.Event == null) cityId = string.Empty;
            else cityId = model.Event.SameAddressAsOganizer ? model.Address?.CityId.ToString() : "";

            ViewBag.Cities = new SelectList(_db.Cities, "CityId", "Name", cityId);
            ViewBag.Companies = new SelectList(_db.Companies, "CompanyId", "Name", model?.Event?.Organizer?.CompanyId);
        }

        public ActionResult GetOrganizerAddress(Guid companyId)
        {
            var company = _db.Companies.Include(i => i.Address).FirstOrDefault(f => f.CompanyId == companyId);
            if (company?.Address == null) return PartialView("_FormAddress");

            var cities = _db.Database.SqlQuery<City>("SELECT DISTINCT ci.Name, ci.CityId FROM [City] ci JOIN [Address] ad ON ci.CityId = ad.CityId JOIN [Company] co ON ad.AddressId = co.AddressId").ToList();
            ViewBag.Cities = new SelectList(cities, "CityId", "Name", company.Address?.CityId);
            company.Address.SetCoordinates();
            return PartialView("_Address", AddressForEventVm.GetAddress(company.Address));
        }

        [HttpPost]
        public JsonResult GetPagination(DataTableParameters parameters)
        {
            var search = parameters.Search.Value?.ToLower() ?? string.Empty;
            var repo = new RepositoryBase<Event>(_db);
            var items = repo.GetItemsByExpression(w => w.Name.Contains(search), x => x.Name, parameters.Start, parameters.Length, out var recordsTotal, i => i.Address.City)
                .Select(s =>
                    new
                    {
                        s.EventId,
                        s.Name,
                        s.StartDate,
                        s.EndDate,
                        s.Address.City
                    }).ToList();
            var dtResult = new DataTableResultSet(parameters.Draw, recordsTotal);

            var buttons = new ButtonsEvent();
            foreach (var item in items)
            {
                dtResult.data.Add(new[]
                {
                    item.EventId.ToString(),
                    $"{item.Name}",
                    $"{(item.StartDate.Date == item.EndDate.Date ? item.StartDate.ToShortDateString() : $"{item.StartDate.ToShortDateString()} - {item.EndDate.ToShortDateString()}")}",
                    item.City.Name,
                    buttons.ToPagination(item.EventId)
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