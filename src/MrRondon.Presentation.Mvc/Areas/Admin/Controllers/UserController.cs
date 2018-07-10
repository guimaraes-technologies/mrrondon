using MrRondon.Domain.Entities;
using MrRondon.Infra.CrossCutting.Helper;
using MrRondon.Infra.CrossCutting.Helper.Buttons;
using MrRondon.Infra.CrossCutting.Message;
using MrRondon.Infra.Data.Context;
using MrRondon.Infra.Data.Repositories;
using MrRondon.Infra.Security.Extensions;
using MrRondon.Presentation.Mvc.Extensions;
using MrRondon.Presentation.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using MrRondon.Infra.Security.Helpers;

namespace MrRondon.Presentation.Mvc.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private readonly MainContext _db = new MainContext();

        [HasAny(Constants.Roles.GeneralAdministrator, Constants.Roles.UserAdministrator, Constants.Roles.ReadOnly)]
        public ActionResult Index()
        {
            return View();
        }

        [HasAny(Constants.Roles.GeneralAdministrator, Constants.Roles.UserAdministrator, Constants.Roles.ReadOnly)]
        public ActionResult Details(Guid id)
        {
            var user = _db.Users
                .Include(a => a.Contacts)
                .Include(a => a.Roles)
                .FirstOrDefault(f => f.UserId == id);

            if (user == null) return RedirectToAction("Index").Error("Usuário não encontrado");

            return View(user);
        }

        [HasAny(Constants.Roles.GeneralAdministrator, Constants.Roles.UserAdministrator)]
        public ActionResult Create()
        {
            SetViewBags();
            return View();
        }

        [HttpPost]
        [HasAny(Constants.Roles.GeneralAdministrator, Constants.Roles.UserAdministrator)]
        public ActionResult Create(UserContactVm model)
        {
            try
            {
                if (!ModelState.IsValid || !model.IsValid())
                {
                    SetViewBags(model);
                    return View(model).Error(ModelState);
                }

                var user = model.GetUser();

                if (model.Contacts != null && model.Contacts.Any(a => a.ContactType == ContactType.Email))
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
                user.ResetPassword();

                user.Roles = new List<Role>();

                var roles = _db.Roles.Where(x => model.RolesIds.Any(r => r == x.RoleId));
                foreach (var role in roles)
                {
                    if (model.RolesIds != null && model.RolesIds.Any(x => x.Equals(role.RoleId)))
                        user.Roles.Add(role);
                }

                user.UserId = Guid.NewGuid();
                _db.Users.Add(user);
                _db.SaveChanges();

                return RedirectToAction("Index").Success(Success.Saved);
            }
            catch (Exception e)
            {
                SetViewBags(model);
                return View(model).Error(e.Message);
            }
        }

        [HasAny(Constants.Roles.GeneralAdministrator, Constants.Roles.UserAdministrator)]
        public ActionResult Edit(Guid id)
        {
            var repo = new RepositoryBase<User>(_db);
            var user = repo.GetItemByExpression(x => x.UserId == id, x => x.Roles, x => x.Contacts);
            if (user == null) return HttpNotFound();

            var crud = GetCrudVm(user);

            SetViewBags(crud);

            return View(crud);
        }

        [HttpPost]
        [HasAny(Constants.Roles.GeneralAdministrator, Constants.Roles.UserAdministrator)]
        public ActionResult Edit(UserContactVm model, int[] rolesIds)
        {
            try
            {
                ModelState.Remove(nameof(model.RolesIds));
                if (!ModelState.IsValid || !model.IsValid())
                {
                    SetViewBags(model);
                    return View(model).Error(ModelState);
                }

                var oldUser = _db.Users
                    .Include(c => c.Roles)
                    .Include(c => c.Contacts)
                    .FirstOrDefault(x => x.UserId == model.UserId);

                if (oldUser == null) return RedirectToAction("Index").Success("Usuário atualizado com sucesso");

                //oldUser.Update(model.FirstName, model.LastName);
                var newUser = model.GetUser();
                //newUser.SetInfo(oldUser);

                BalanceRoles(oldUser, newUser, rolesIds);
                BalanceContacts(oldUser, newUser);

                _db.Entry(oldUser).CurrentValues.SetValues(newUser);
                _db.SaveChanges();
                return RedirectToAction("Index").Success("Usuário atualizado com sucesso");
            }
            catch (Exception ex)
            {
                SetViewBags(model);
                return View(model).Error(ex.Message);
            }
        }

        [HasAny(Constants.Roles.GeneralAdministrator, Constants.Roles.UserAdministrator)]
        public ActionResult UpdateStatus(Guid id)
        {
            try
            {
                var user = _db.Users.FirstOrDefault(f => f.UserId == id);

                if (user == null) throw new Exception("Usuário não encontrado");

                _db.Entry(user).Property(u => u.IsActive).CurrentValue = !user.IsActive;
                _db.SaveChanges();

                return RedirectToAction("Index").Success("Operação realizada com sucesso");
            }
            catch (Exception e)
            {
                return RedirectToAction("Index").Error(e.Message);
            }
        }

        [HasAny(Constants.Roles.GeneralAdministrator, Constants.Roles.UserAdministrator)]
        public ActionResult ResetPassword(Guid id)
        {
            try
            {
                var user = _db.Users.FirstOrDefault(f => f.UserId == id);
                if (user == null) throw new Exception("Usuário não encontrado");
                user.ResetPassword();
                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChanges();

                return RedirectToAction("Index").Success(Success.ChangedPassword);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index").Error(e.Message);
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
        [HasAny(Constants.Roles.GeneralAdministrator, Constants.Roles.UserAdministrator, Constants.Roles.ReadOnly)]
        public JsonResult GetPagination(DataTableParameters parameters)
        {
            var search = parameters.Search.Value?.ToLower() ?? string.Empty;
            var repo = new RepositoryBase<User>(_db);
            var items = repo.GetItemsByExpression(w => string.Concat(w.FirstName, " ", w.LastName).Contains(search), x => x.FirstName, parameters.Start, parameters.Length, out var recordsTotal)
                .Select(s => new
                {
                    s.UserId,
                    s.FullName,
                    s.Cpf,
                    s.IsActive
                }).ToList();
            var dtResult = new DataTableResultSet(parameters.Draw, recordsTotal);

            var buttonsUser = new ButtonsUser();
            foreach (var item in items)
            {
                dtResult.data.Add(new object[]
                {
                    item.IsActive,
                    item.FullName,
                    item.Cpf,
                    $"{(item.IsActive ? "Ativado" : "Desativado")}",
                    buttonsUser.ToPagination(item.UserId, item.IsActive, Account.Current.Roles)
                });
            }
            return Json(dtResult, JsonRequestBehavior.AllowGet);
        }

        public void UrlsContact()
        {
            ViewBag.UrlAdd = Url.Action("AddContact", "User", new { area = "Admin" });
            ViewBag.UrlRemove = Url.Action("RemoveContact", "User", new { area = "Admin" });
        }

        private void SetViewBags(UserContactVm model = null)
        {
            var roles = _db.Roles
                .OrderBy(x => x.Name)
                .AsNoTracking();

            model = model ?? new UserContactVm();
            foreach (var role in roles)
            {
                var item = new SelectListItemVm(
                    role.RoleId,
                    role.Name.Replace("_", " "),
                    model.RolesIds?.Any(x => x == role.RoleId) ?? false);
                model.SelectListRole.Add(item);
            }

            model.RolesIds = model.Roles?.Select(s => s.RoleId).ToArray();
            ViewBag.Roles = model.SelectListRole;
        }

        private static UserContactVm GetCrudVm(User user)
        {
            var model = new UserContactVm
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Contacts = user.Contacts.ToList(),
                AccessFailed = user.AccessFailed,
                Cpf = user.Cpf,
                IsActive = user.IsActive,
                CreateOn = user.CreateOn,
                UserId = user.UserId,
                Roles = user.Roles,
                RolesIds = user.Roles.Select(s => s.RoleId).ToArray()
            };

            if (user.Contacts != null) model.Contacts = new List<Contact>(user.Contacts);

            return model;
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

        public void BalanceContacts(User oldUser, User newUser)
        {
            // Delete children
            foreach (var existingContact in oldUser.Contacts.ToList())
            {
                if (newUser.Contacts.All(c => c.ContactId != existingContact.ContactId))
                    _db.Contacts.Remove(existingContact);
            }

            // Update and Insert children
            foreach (var childContact in newUser.Contacts)
            {
                var existingContact = oldUser.Contacts
                    .FirstOrDefault(c => c.ContactId == childContact.ContactId);

                childContact.UserId = newUser.UserId;
                if (existingContact != null)
                    // Update child
                    _db.Entry(existingContact).CurrentValues.SetValues(childContact);
                else
                {
                    // Insert child
                    oldUser.Contacts.Add(childContact);
                    _db.Contacts.Add(childContact);
                }
            }
        }
    }
}