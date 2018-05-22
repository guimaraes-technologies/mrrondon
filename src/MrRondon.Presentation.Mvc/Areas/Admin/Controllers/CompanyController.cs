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
using WebGrease.Css.Extensions;
using Address = MrRondon.Domain.Entities.Address;

namespace MrRondon.Presentation.Mvc.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CompanyController : Controller
    {
        private readonly MainContext _db = new MainContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(Guid id)
        {
            var repo = new RepositoryBase<Company>(_db);
            var company = repo.GetItemByExpression(x => x.CompanyId == id, i => i.Address.City, i => i.SubCategory.Category, i => i.Contacts);
            if (company == null) return HttpNotFound();
            return View(company);
        }

        public ActionResult Create()
        {
            SetBiewBags(null);
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(CrudCompanyVm model, Address address)
        {
            try
            {
                if (model.SubCategoryId.HasValue && model.SubCategoryId != 0)
                    model.Company.SubCategoryId = model.SubCategoryId.Value;
                else model.Company.SubCategoryId = model.CategoryId;

                address.SetCoordinates(address.LatitudeString, address.LongitudeString);
                model.Company.Address = address;
                model.Company.Contacts = model.Contacts;

                if (model.Company.Logo == null || model.LogoFile != null)
                    model.Company.Logo = FileUpload.GetBytes(model.LogoFile, "Logo");
                if (model.Company.Cover == null || model.CoverFile != null)
                    model.Company.Cover = FileUpload.GetBytes(model.CoverFile, "Capa");

                ModelState.Remove("Company.Logo");
                ModelState.Remove("Company.Cover");
                if (!ModelState.IsValid)
                {
                    SetBiewBags(model);
                    return View(model).Error(ModelState);
                }

                _db.Companies.Add(model.Company);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                SetBiewBags(model);
                return View(model).Error(ex.Message);
            }
        }

        public ActionResult Edit(Guid id)
        {
            var repo = new RepositoryBase<Company>(_db);
            var company = repo.GetItemByExpression(x => x.CompanyId == id, x => x.Address, x => x.SubCategory, x => x.Contacts);
            if (company == null) return HttpNotFound();

            var crud = GetCrudVm(company);
            crud.Company.Address.SetCoordinates();

            SetBiewBags(crud);

            return View(crud);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(CrudCompanyVm model, Address address)
        {
            try
            {
                if (model.SubCategoryId.HasValue && model.SubCategoryId != 0)
                    model.Company.SubCategoryId = model.SubCategoryId.Value;
                else model.Company.SubCategoryId = model.CategoryId;

                address.SetCoordinates(address.LatitudeString, address.LongitudeString);
                model.Company.Address = address;
                model.Company.Contacts = model.Contacts;

                if (model.Company.Logo == null || model.LogoFile != null)
                    model.Company.Logo = FileUpload.GetBytes(model.LogoFile, "Logo");
                if (model.Company.Cover == null || model.CoverFile != null)
                    model.Company.Cover = FileUpload.GetBytes(model.CoverFile, "Capa");

                ModelState.Remove("Company.Logo");
                ModelState.Remove("Company.Cover");
                if (!ModelState.IsValid)
                {
                    SetBiewBags(model);
                    return View(model).Error(ModelState);
                }

                var oldCompany = _db.Companies
                    .Include(c => c.Address)
                    .Include(c => c.SubCategory)
                    .Include(c => c.Contacts)
                    .FirstOrDefault(x => x.CompanyId == model.Company.CompanyId);
                if (oldCompany == null) return RedirectToAction("Index").Success("Empresa atualizada com sucesso");

                // Update parent
                _db.Entry(oldCompany).CurrentValues.SetValues(model.Company);
                oldCompany.Address.UpdateAddress(model.Company.Address);
                _db.Entry(oldCompany.Address).State = EntityState.Modified;

                // Delete children
                foreach (var existingContact in oldCompany.Contacts.ToList())
                {
                    if (model.Contacts.All(c => c.ContactId != existingContact.ContactId))
                        _db.Contacts.Remove(existingContact);
                }

                // Update and Insert children
                foreach (var childContact in model.Contacts)
                {
                    var existingContact = oldCompany.Contacts
                        .FirstOrDefault(c => c.ContactId == childContact.ContactId);

                    childContact.CompanyId = model.Company.CompanyId;
                    if (existingContact != null)
                        // Update child
                        _db.Entry(existingContact).CurrentValues.SetValues(childContact);
                    else
                    {
                        // Insert child
                        oldCompany.Contacts.Add(childContact);
                        _db.Contacts.Add(childContact);
                    }
                }

                _db.SaveChanges();
                return RedirectToAction("Index").Success("Empresa atualizada com sucesso");
            }
            catch (Exception ex)
            {
                SetBiewBags(model);

                return View(model).Error(ex.Message);
            }
        }

        [AllowAnonymous]
        public ActionResult Contacts(CrudCompanyVm companyContact)
        {
            UrlsContact();
            companyContact = companyContact ?? new CrudCompanyVm();
            companyContact.Contacts = companyContact.Contacts ?? new List<Contact>();
            return PartialView("_Contacts", companyContact.Contacts);
        }

        [AllowAnonymous, HttpPost]
        public ActionResult AddContact(CrudCompanyVm companyContact)
        {
            companyContact.Contacts = companyContact.Contacts ?? new List<Contact>();
            companyContact.Contacts.Add(new Contact { Description = companyContact.Description, ContactType = companyContact.ContactType });
            companyContact.Description = string.Empty;
            companyContact.ContactType = 0;
            UrlsContact();
            return PartialView("_Contacts", companyContact.Contacts);
        }

        [AllowAnonymous, HttpPost]
        public ActionResult RemoveContact(CrudCompanyVm companyContact, int index)
        {
            UrlsContact();
            companyContact.Contacts?.RemoveAt(index);
            return PartialView("_Contacts", companyContact.Contacts);
        }

        [HttpPost]
        public JsonResult GetPagination(DataTableParameters parameters)
        {
            var search = parameters.Search.Value?.ToLower() ?? string.Empty;
            var repo = new RepositoryBase<Company>(_db);
            var items = repo.GetItemsByExpression(w => w.Name.Contains(search), x => x.Name, parameters.Start, parameters.Length, out var recordsTotal)
                .Select(s=> new 
                {
                    s.CompanyId,
                    s.Logo,
                    s.Name,
                    s.Cnpj
                }).ToList();
            var dtResult = new DataTableResultSet(parameters.Draw, recordsTotal);
            
            var buttons = new ButtonsCompany();
            foreach (var item in items)
            {
                dtResult.data.Add(new[]
                {
                    item.CompanyId.ToString(),
                    item.Name,
                    item.Cnpj,
                    buttons.ToPagination(item.CompanyId)
                });
            }
            return Json(dtResult, JsonRequestBehavior.AllowGet);
        }

        public void UrlsContact()
        {
            ViewBag.UrlAdd = Url.Action("AddContact", "Company", new { area = "Admin" });
            ViewBag.UrlRemove = Url.Action("RemoveContact", "Company", new { area = "Admin" });
        }

        private static CrudCompanyVm GetCrudVm(Company company)
        {
            var model = new CrudCompanyVm { Company = company };

            if (company.Contacts != null) model.Contacts = new List<Contact>(company.Contacts);
            if (company.SubCategory?.CategoryId != null)
            {
                model.SubCategoryId = company.SubCategoryId;
                model.CategoryId = company.SubCategory.CategoryId.Value;
            }
            else model.CategoryId = company.SubCategoryId;

            return model;
        }

        private void SetBiewBags(CrudCompanyVm model)
        {
            ViewBag.Cities = new SelectList(EventController.GetCities(_db), "CityId", "Name", model?.Company?.Address?.CityId);

            var categories = _db.SubCategories.Where(s => s.CategoryId == null).OrderBy(o => o.Name);
            ViewBag.Categories = new SelectList(categories, "SubCategoryId", "Name", model?.CategoryId);

            if (model == null || model.CategoryId == 0) ViewBag.SubCategories = new SelectList(Enumerable.Empty<SelectListItem>());
            else
            {
                var subCategories = _db.SubCategories.Where(s => s.CategoryId != null && model.CategoryId == s.SubCategoryId).OrderBy(o => o.Name);
                ViewBag.SubCategories = new SelectList(subCategories, "SubCategoryId", "Name", model.SubCategoryId);
            }
        }

        public void BalanceRoles(User oldUser, User newUser, int[] rolesIds)
        {
            newUser.Roles = new List<Role>();
            oldUser.Roles = oldUser.Roles ?? new List<Role>();

            var rolesArray = _db.Roles.ToList();
            foreach (var role in rolesArray)
            {
                if (rolesIds != null && rolesIds.Any(x => x.Equals(role.RoleId)))
                {
                    newUser.Roles.Add(_db.Roles.FirstOrDefault(x => x.RoleId == role.RoleId));
                }
            }

            foreach (var role in rolesArray)
            {
                var userRole = oldUser.Roles.FirstOrDefault(x => x.RoleId == role.RoleId);
                if (rolesIds != null && rolesIds.Contains(role.RoleId) && userRole == null)
                {
                    oldUser.Roles.Add(_db.Roles.FirstOrDefault(x => x.RoleId == role.RoleId));
                }
                else
                {
                    if (userRole == null) continue;
                    if (rolesIds != null && rolesIds.Contains(userRole.RoleId)) continue;
                    oldUser.Roles.Remove(userRole);
                }
            }
        }

        public void BalanceContacts(Company oldCompany, Company newCompany)
        {
            var ids = oldCompany.Contacts.Select(x => x.ContactId).ToList();

            oldCompany.Contacts.Where(w => ids.Any(e => e == w.ContactId)).ForEach(x => _db.Entry(x).State = EntityState.Deleted);

            if (newCompany.Contacts == null) return;
            foreach (var item in newCompany.Contacts)
            {
                if (oldCompany.Contacts == null) continue;
                var x = oldCompany.Contacts.FirstOrDefault(s => s.ContactId == item.ContactId && s.ContactId != Guid.Empty && item.ContactId != Guid.Empty);
                if (x != null)
                {
                    x.Atualizar(item);
                    _db.Entry(x).CurrentValues.SetValues(item);
                }
                else oldCompany.Contacts.Add(item);
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