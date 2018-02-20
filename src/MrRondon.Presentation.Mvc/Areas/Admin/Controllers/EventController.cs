using System;
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
        public ActionResult Create(CrudEventVm model)
        {
            try
            {
                ModelState.Remove("Event_Logo");
                ModelState.Remove("Event_Cover");
                RemoveAddressValidation(model);

                if (!ModelState.IsValid)
                {
                    SetBiewBags(model);
                    return View(model);
                }

                if (model.Event.Logo == null || model.LogoFile != null)
                    model.Event.Logo = FileUpload.GetBytes(model.LogoFile, "Logo");
                if (model.Event.Cover == null || model.CoverFile != null)
                    model.Event.Cover = FileUpload.GetBytes(model.CoverFile, "Capa");

                model.Address.SetCoordinates(model.Address.LatitudeString, model.Address.LongitudeString);
                model.Event.Address = model.Address.GetAddress();
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
      
        /*

        public ActionResult Edit(Guid id)
        {
            var repo = new RepositoryBase<Event>(_db);
            var company = repo.GetItemByExpression(x => x.EventId == id, "Address", "Segment");
            if (company == null) return HttpNotFound();

            var model = GetCrudVm(company);
            SetBiewBags(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Event company)
        {
            var model = new CrudEventVm { Event = company };
            try
            {
                ModelState.Remove(nameof(company.SubCategoryId));
                if (!ModelState.IsValid)
                {
                    SetBiewBags(model);
                    return View(model);
                }

                _db.Entry(company).State = EntityState.Modified;
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
        public ActionResult Contacts(CrudEventVm companyContact)
        {
            UrlsContact();
            companyContact = companyContact ?? new CrudEventVm();
            companyContact.Contacts = companyContact.Contacts ?? new List<Contact>();
            return PartialView("_Contacts", companyContact.Contacts);
        }

        [AllowAnonymous, HttpPost]
        public ActionResult AddContact(CrudEventVm companyContact)
        {
            companyContact.Contacts = companyContact.Contacts ?? new List<Contact>();
            companyContact.Contacts.Add(new Contact { Description = companyContact.Description, ContactType = companyContact.ContactType });
            companyContact.Description = string.Empty;
            companyContact.ContactType = 0;
            UrlsContact();
            return PartialView("_Contacts", companyContact.Contacts);
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
        */

        private void SetBiewBags(CrudEventVm model)
        {
            string cityId;
            if (model?.Event == null) cityId = string.Empty;
            else cityId = model.Event.SameAddressAsOganizer ? model.Address?.CityId.ToString() : "";

            ViewBag.Cities = new SelectList(_db.Cities, "CityId", "Name", cityId);
            ViewBag.Companies = new SelectList(_db.Companies, "CompanyId", "Name", model?.Event?.Organizer?.CompanyId);
        }

        [HttpPost]
        public JsonResult GetPagination(DataTableParameters parameters)
        {
            var search = parameters.Search.Value?.ToLower() ?? string.Empty;
            var repo = new RepositoryBase<Event>(_db);
            var items = repo.GetItemsByExpression(w => w.Name.Contains(search), x => x.Name, parameters.Start, parameters.Length, out var recordsTotal).ToList();
            var dtResult = new DataTableResultSet(parameters.Draw, recordsTotal);

            var buttons = new ButtonsEvent();
            foreach (var item in items)
            {
                dtResult.data.Add(new[]
                {
                    item.EventId.ToString(),
                    $"{item.Name}",
                    buttons.ToPagination(item.EventId)
                });
            }
            return Json(dtResult, JsonRequestBehavior.AllowGet);
        }

        private void RemoveAddressValidation(CrudEventVm model)
        {
            ModelState.Remove(nameof(model.Address.ZipCode));
            ModelState.Remove(nameof(model.Address.AdditionalInformation));
            ModelState.Remove(nameof(model.Address.Latitude));
            ModelState.Remove(nameof(model.Address.Longitude));
            ModelState.Remove(nameof(model.Address.Number));
            ModelState.Remove(nameof(model.Address.Street));
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