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
            var company = repo.GetItemByExpression(x => x.CompanyId == id);
            if (company == null) return HttpNotFound();
            var model = GetCrudVm(company);

            return View(model);
        }

        public ActionResult Create()
        {
            SetBiewBags(null);
            return View();
        }

        [HttpPost]
        public ActionResult Create(CrudCompanyVm model, Address address)
        {
            try
            {
                if (model.SubCategoryId.HasValue && model.SubCategoryId != 0)
                    model.Company.SubCategoryId = model.SubCategoryId.Value;
                else model.Company.SubCategoryId = model.CategoryId;

                model.Company.Address = address;
                model.Company.Contacts = model.Contacts;
                ModelState.Remove("Company_Logo");
                ModelState.Remove("Company_Cover");
                if (model.Company.Logo == null || model.LogoFile != null)
                    model.Company.Logo = FileUpload.GetBytes(model.LogoFile, "Logo");
                if (model.Company.Cover == null || model.CoverFile != null)
                model.Company.Cover = FileUpload.GetBytes(model.CoverFile, "Capa");

                if (!ModelState.IsValid)
                {
                    SetBiewBags(model);
                    return View(model);
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
            var company = repo.GetItemByExpression(x => x.CompanyId == id, "Address", "SubCategory", "Contacts");
            if (company == null) return HttpNotFound();

            var model = GetCrudVm(company);
            SetBiewBags(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(CrudCompanyVm model, Address address)
        {
            try
            {
                if (model.SubCategoryId.HasValue && model.SubCategoryId != 0)
                    model.Company.SubCategoryId = model.SubCategoryId.Value;
                else model.Company.SubCategoryId = model.CategoryId;

                model.Company.Address = address;
                model.Company.Contacts = model.Contacts;

                ModelState.Remove("Company_Logo");
                ModelState.Remove("Company_Cover");
                if (model.Company.Logo == null || model.LogoFile != null)
                    model.Company.Logo = FileUpload.GetBytes(model.LogoFile, "Logo");
                if (model.Company.Cover == null || model.CoverFile != null)
                    model.Company.Cover = FileUpload.GetBytes(model.CoverFile, "Capa");

                if (!ModelState.IsValid)
                {
                    SetBiewBags(model);
                    return View(model);
                }

                var oldCompany = _db.Companies.Include(c => c.Contacts)
                    .FirstOrDefault(x => x.CompanyId == model.Company.CompanyId);
                BalanceContacts(oldCompany, model.Company);
                _db.Entry(model.Company).State = EntityState.Modified;
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
            var items = repo.GetItemsByExpression(w => w.Name.Contains(search), x => x.Name, parameters.Start, parameters.Length, out var recordsTotal).ToList();
            var dtResult = new DataTableResultSet(parameters.Draw, recordsTotal);

            var buttons = new ButtonsCompany();
            foreach (var item in items)
            {
                dtResult.data.Add(new[]
                {
                    item.CompanyId.ToString(),
                    $"{item.Name}",
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
            var model = new CrudCompanyVm
            {
                Company = company,
                Contacts = new List<Contact>(company.Contacts)
            };
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
            ViewBag.Cities = new SelectList(_db.Cities, "CityId", "Name", model?.Company?.Address.CityId);

            var categories = _db.SubCategories.Where(s => s.CategoryId == null).OrderBy(o => o.Name);
            ViewBag.Categories = new SelectList(categories, "SubCategoryId", "Name", model?.CategoryId);

            if (model == null || model.CategoryId == 0) ViewBag.SubCategories = new SelectList(Enumerable.Empty<SelectListItem>());
            else
            {
                var subCategories = _db.SubCategories.Where(s => s.CategoryId != null).OrderBy(o => o.Name);
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
            var ids = oldCompany.Contacts.Select((t, i) => oldCompany.Contacts.ElementAt(i).ContactId).ToList();
            _db.Contacts.RemoveRange(oldCompany.Contacts.Where(x => ids.Any(e => e == x.ContactId)));

            if (newCompany.Contacts == null) return;
            foreach (var item in newCompany.Contacts)
            {
                if (oldCompany.Contacts == null) continue;
                var x = oldCompany.Contacts.FirstOrDefault(
                        s => s.ContactId == item.ContactId && s.ContactId != Guid.Empty && item.ContactId != Guid.Empty);
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