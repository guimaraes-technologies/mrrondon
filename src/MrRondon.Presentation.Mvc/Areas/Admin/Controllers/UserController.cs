using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MrRondon.Domain.Entities;
using MrRondon.Infra.CrossCutting.Helper;
using MrRondon.Infra.CrossCutting.Helper.Buttons;
using MrRondon.Infra.CrossCutting.Message;
using MrRondon.Infra.Data.Context;
using MrRondon.Infra.Data.Repositories;
using MrRondon.Presentation.Mvc.Extensions;
using MrRondon.Presentation.Mvc.ViewModels;

namespace MrRondon.Presentation.Mvc.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private readonly MainContext _db = new MainContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(Guid id)
        {
            var user = _db.Users
                .Include(a => a.Contacts)
                .Include(a => a.Roles)
                .FirstOrDefault(f => f.UserId == id);

            if (user == null) return RedirectToAction("Index").Error("Usuário não encontrado");


            return View(user);
        }

        public ActionResult Create()
        {
            GetDrops();
            return View();
        }

        [HttpPost]
        public ActionResult Create(UserContactVm userContact)
        {
            try
            {
                if (!ModelState.IsValid) return View(userContact).Error(ModelState);

                var user = userContact.GetUser();

                if (userContact.Contacts != null && userContact.Contacts.Any(a => a.ContactType == ContactType.Email))
                {
                    var contatos = user.Contacts.Where(f => f.ContactType == ContactType.Email);
                    foreach (var contato in contatos)
                    {
                        var emailIsInUse = _db.Contacts.Any(x => x.Description.Equals(contato.Description));
                        if (emailIsInUse) throw new Exception($"Este Email '{contato.Description}' já está em uso");
                    }
                }

                var cpfIsInUse = _db.Users.Any(x => x.Cpf.Equals(user.Cpf));
                if (cpfIsInUse) throw new Exception($"Este CPF '{user.Cpf}' já está em uso");

                user.EncryptPassword(user.Cpf.Replace(".", "").Replace("-", ""));


                user.Roles = new List<Role>();

                var roles = _db.Roles.Where(x => userContact.RolesIds.Any(r => r == x.RoleId));
                foreach (var role in roles)
                {
                    if (userContact.RolesIds != null && userContact.RolesIds.Any(x => x.Equals(role.RoleId)))
                        user.Roles.Add(role);
                }

                _db.Users.Add(user);
                _db.SaveChanges();

                return RedirectToAction("Index").Success(Success.Saved);
            }
            catch (Exception e)
            {
                GetDrops(userContact.RolesIds?.FirstOrDefault(), userContact.CompanyId);
                return View(userContact).Error(e.Message);
            }
        }

        [AllowAnonymous]
        public ActionResult Contacts(UserContactVm userContact)
        {
            UrlsContact();
            userContact = userContact ?? new UserContactVm();
            userContact.Contacts = userContact.Contacts ?? new List<Contact>();
            return PartialView("_Contacts", userContact.Contacts);
        }

        [AllowAnonymous, HttpPost]
        public ActionResult AddContact(UserContactVm userContact)
        {
            userContact.Contacts = userContact.Contacts ?? new List<Contact>();
            userContact.Contacts.Add(new Contact { Description = userContact.Description, ContactType = userContact.ContactType });
            userContact.Description = string.Empty;
            userContact.ContactType = 0;
            UrlsContact();
            return PartialView("_Contacts", userContact.Contacts);
        }

        [AllowAnonymous, HttpPost]
        public ActionResult RemoveContact(UserContactVm userContact, int index)
        {
            UrlsContact();
            userContact.Contacts?.RemoveAt(index);
            return PartialView("_Contacts", userContact.Contacts);
        }

        [AllowAnonymous]
        public JsonResult CpfIsInUse(string value)
        {
            var repo = new RepositoryBase<User>();
            return Json(!repo.IsTrue(x => x.Cpf.Equals(value)), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPagination(DataTableParameters parameters)
        {
            var search = parameters.Search.Value?.ToLower() ?? string.Empty;
            var repo = new RepositoryBase<User>(_db);
            var items = repo.GetItemsByExpression(w => w.FullName.Contains(search) || w.Cpf.Replace(".", "").Replace("-", "").Contains(search), x => x.FullName, parameters.Start, parameters.Length, out var recordsTotal)
                .Select(s => new
                {
                    s.UserId,
                    s.FullName,
                    s.Cpf,
                    s.IsActive
                }).ToList();
            var dtResult = new DataTableResultSet(parameters.Draw, recordsTotal);

            var buttonsEntidade = new ButtonsUser();
            foreach (var item in items)
            {
                dtResult.data.Add(new[]
                {
                    item.IsActive.ToString(),
                    item.FullName,
                    item.Cpf,
                    buttonsEntidade.ToPagination(item.UserId, item.IsActive)
                });
            }
            return Json(dtResult, JsonRequestBehavior.AllowGet);
        }

        public void UrlsContact()
        {
            ViewBag.UrlAdd = Url.Action("AddContact", "User", new { area = "Admin" });
            ViewBag.UrlRemove = Url.Action("RemoveContact", "User", new { area = "Admin" });
        }
        private void GetDrops(int? roleId = null, Guid? companyId = null)
        {
            var roles = _db.Roles;
            ViewBag.Roles = new SelectList(roles, "RoleId", "Name", roleId);

            var companies = _db.Companies;//todo Apenas ativos?
            ViewBag.EntidadeId = new SelectList(companies, "CompanyId", "Name", companyId);
        }
    }
}