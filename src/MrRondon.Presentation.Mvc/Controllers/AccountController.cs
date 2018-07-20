using MrRondon.Domain.Entities;
using MrRondon.Infra.CrossCutting.Helper;
using MrRondon.Infra.CrossCutting.Message;
using MrRondon.Infra.Data.Context;
using MrRondon.Infra.Data.Repositories;
using MrRondon.Infra.Security.Helpers;
using MrRondon.Presentation.Mvc.Areas.Admin.Controllers;
using MrRondon.Presentation.Mvc.Extensions;
using MrRondon.Presentation.Mvc.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MrRondon.Presentation.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly MainContext _db;

        public AccountController()
        {
            _db = new MainContext();
        }

        [AllowAnonymous]
        public ActionResult Signin(string returnUrl)
        {
            return Account.Current.IsAuthenticated ? RedirectToLocal(returnUrl) : View(new SigninVm { ReturnUrl = returnUrl });
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Signin(SigninVm model)
        {
            try
            {
                if (!ModelState.IsValid) return View(model).Error(Error.Default);
                var user = _db.Users.Include(i => i.Roles).FirstOrDefault(f => f.Cpf.Equals(model.UserName));

                AccountManager.Signin(user, model.Password);
                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChanges();

                return RedirectToLocal(model.ReturnUrl);
            }
            catch (Exception ex)
            {
                return View(model).Error(ex.Message);
            }
        }

        public ActionResult Signout()
        {
            AccountManager.Signout();
            return RedirectToAction("Signin", "Account", new { area = "" });
        }

        public ActionResult Details()
        {
            var user = _db.Users
                .Include(i => i.Contacts)
                .Include(i => i.Roles)
                .FirstOrDefault(f => f.UserId == Account.Current.UserId);
            return View(user);
        }

        public ActionResult Edit()
        {
            var user = _db.Users
                .Include(i => i.Contacts)
                .FirstOrDefault(f => f.UserId == Account.Current.UserId);
            if (user == null) return RedirectToAction("Index", "Home").Error("Usuário não encontrado");
            var userContact = new UserContactVm
            {
                UserId = user.UserId,
                Cpf = user.Cpf,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                Contacts = user.Contacts.ToList()
            };
            return View(userContact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserContactVm model)
        {
            try
            {
                ModelState.Remove("Cpf");
                ModelState.Remove("RolesIds");
                if (!ModelState.IsValid)
                {
                    ViewBag.Erros = ErrorController.Get(ModelState);
                    return View(model).Error(ModelState);
                }

                var oldUser = _db.Users
                    .Include(c => c.Roles)
                    .Include(c => c.Contacts)
                    .FirstOrDefault(x => x.UserId == model.UserId);
                if (oldUser == null) return RedirectToAction("Details").Success("Conta atualizada com sucesso");
                oldUser.Update(model.FirstName, model.LastName);

                var newUser = model.GetUser(_db);
                newUser.SetPassword(oldUser.Password);
                newUser.SetInfo(oldUser);
                BalanceContacts(oldUser, newUser);
                BalanceRoles(oldUser, newUser, model.RolesIds);

                _db.Entry(oldUser).CurrentValues.SetValues(newUser);
                _db.SaveChanges();
                return RedirectToAction("Details").Success("Conta atualizada com sucesso");
            }
            catch (Exception e)
            {
                return View(model).Error(e.Message);
            }
        }

        [AllowAnonymous]
        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost, AllowAnonymous]
        public async Task<ActionResult> ForgetPassword(ForgetPasswordVm model)
        {
            try
            {
                if (!ModelState.IsValid) return View(model);

                var repo = new RepositoryBase<User>(_db);
                var user = repo.GetItemByExpression(x => x.Cpf.Equals(model.UserName), x => x.Contacts);
                if (user == null) return View(model).Success("Um código para redefinição da sua senha foi enviado para o seu email");
                user.SetPasswordRecoveryCode();

                var query = $"UPDATE [{nameof(Domain.Entities.User)}] SET {nameof(user.PasswordRecoveryCode)}='{user.PasswordRecoveryCode}' WHERE {nameof(user.UserId)}='{user.UserId}'";
                await _db.Database.ExecuteSqlCommandAsync(query);

                var email = user.Contacts.FirstOrDefault(f => f.ContactType == ContactType.Email)?.Description;

                if (string.IsNullOrWhiteSpace(email))
                    return View(model).Error("Não foi possível enviar um email com o seu código de recuperação, pois não existe nenhum Email para contato");

                var emailManager = new EmailManager(new ArrayList { email });
                var actualUrl = Request.Url.OriginalString;
                var url = $"{actualUrl.Replace(Request.Url.Segments[Request.Url.Segments.Length - 1], "recoverypassword")}/{user.PasswordRecoveryCode}";

                emailManager.ForgotPassword(user.FullName, url);
                await emailManager.SendAsync(emailManager.Sender, $"Sistema {Constants.SystemName}");

                return View(model).Success($"Um código para redefinição da sua senha foi enviado para o seu email: '{email}'");
            }
            catch (Exception e)
            {
                return View(model).Error(e.Message);
            }
        }

        public ActionResult NewPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewPassword(NewPasswordVm model)
        {
            try
            {
                if (!ModelState.IsValid) return View().Error(Error.Default);
                if (!string.Equals(model.NewPassword, model.ConfirmPassword)) return View(model).Error(Error.PasswordDoesNotMatch);
                var user = _db.Users.Find(Account.Current.UserId);
                if (user == null) return RedirectToAction("Details").Success(Success.ChangedPassword);
                _db.Entry(user).Property(u => u.Password).CurrentValue = user.EncryptPassword(model.NewPassword);
                _db.SaveChanges();

                return RedirectToAction("Details").Success(Success.ChangedPassword);
            }
            catch (Exception e)
            {
                return View().Error(e.Message);
            }
        }

        [AllowAnonymous]
        public ActionResult RecoveryPassword(string code)
        {
            return View(new RecoveryPasswordVm { Code = code });
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecoveryPassword(RecoveryPasswordVm model)
        {
            try
            {
                if (!ModelState.IsValid) return View().Error(Error.Default);

                if (!string.Equals(model.NewPassword, model.ConfirmPassword)) return View(model).Error(Error.PasswordDoesNotMatch);

                var user = _db.Users.FirstOrDefault(x => x.PasswordRecoveryCode == model.Code);
                if (user == null) return RedirectToAction("Signin").Success(Success.ChangedPassword);

                _db.Entry(user).Property(u => u.Password).CurrentValue = user.EncryptPassword(model.NewPassword);
                _db.SaveChanges();

                return RedirectToAction("Signin").Success(Success.ChangedPassword);
            }
            catch (Exception e)
            {
                return View().Error(e.Message);
            }
        }

        [AllowAnonymous]
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (!Account.Current.IsAuthenticated) return RedirectToAction("Signin", "Account", new { area = "" });

            if (Account.Current.HasAny(Constants.Roles.ReadOnly))
                return RedirectToAction("Index", "Company", new { area = "Admin" });

            if (Account.Current.HasAny(Constants.Roles.GeneralAdministrator))
                return RedirectToAction("Index", "Company", new { area = "Admin" });

            if (Account.Current.HasAny(Constants.Roles.CategoryAdministrator))
                return RedirectToAction("Index", "Category", new { area = "Admin" });

            if (Account.Current.HasAny(Constants.Roles.CityAdministrator))
                return RedirectToAction("Index", "City", new { area = "Admin" });

            if (Account.Current.HasAny(Constants.Roles.CompanyAdministrator))
                return RedirectToAction("Index", "Company", new { area = "Admin" });

            if (Account.Current.HasAny(Constants.Roles.EventAdministrator))
                return RedirectToAction("Index", "Event", new { area = "Admin" });

            if (Account.Current.HasAny(Constants.Roles.HistoricalSightAdministrator))
                return RedirectToAction("Index", "HistoricalSight", new { area = "Admin" });

            if (Account.Current.HasAny(Constants.Roles.SubCategoryAdministrator))
                return RedirectToAction("Index", "SubCategory", new { area = "Admin" });

            if (Account.Current.HasAny(Constants.Roles.UserAdministrator))
                return RedirectToAction("Index", "User", new { area = "Admin" });

            return RedirectToAction("Code", "Error", new { id = Constants.Codes.Unauthorized });
        }

        public void BalanceRoles(User oldUser, User newUser, int[] rolesIds, MainContext ctx)
        {
            newUser.Roles = new List<Role>();
            oldUser.Roles = oldUser.Roles ?? new List<Role>();

            var rolesArray = ctx.Roles.ToList();
            foreach (var role in rolesArray)
            {
                if (rolesIds != null && rolesIds.Any(x => x.Equals(role.RoleId)))
                {
                    newUser.Roles.Add(ctx.Roles.FirstOrDefault(x => x.RoleId == role.RoleId));
                }
            }

            foreach (var role in rolesArray)
            {
                var userRole = oldUser.Roles.FirstOrDefault(x => x.RoleId == role.RoleId);
                if (rolesIds != null && rolesIds.Contains(role.RoleId) && userRole == null)
                {
                    oldUser.Roles.Add(ctx.Roles.FirstOrDefault(x => x.RoleId == role.RoleId));
                }
                else
                {
                    if (userRole == null) continue;
                    if (rolesIds != null && rolesIds.Contains(userRole.RoleId)) continue;
                    oldUser.Roles.Remove(userRole);
                }
            }
        }

        public void BalanceContacts(User oldUser, User newUser, MainContext ctx)
        {
            var ids = oldUser.Contacts.Select((t, i) => oldUser.Contacts.ElementAt(i).ContactId).ToList();
            ctx.Contacts.RemoveRange(oldUser.Contacts.Where(x => ids.Any(e => e == x.ContactId)));

            if (newUser.Contacts == null) return;
            foreach (var item in newUser.Contacts)
            {
                if (oldUser.Contacts == null) continue;
                var x = oldUser.Contacts.FirstOrDefault(s => s.ContactId == item.ContactId && s.ContactId != Guid.Empty && item.ContactId != Guid.Empty);
                if (x != null)
                {
                    x.Atualizar(item);
                    ctx.Entry(x).CurrentValues.SetValues(item);
                }
                else
                {
                    oldUser.Contacts.Add(item);
                }
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

        public void BalanceContacts(User oldUser, User newUser)
        {
            var ids = oldUser.Contacts.Select((t, i) => oldUser.Contacts.ElementAt(i).ContactId).ToList();
            _db.Contacts.RemoveRange(oldUser.Contacts.Where(x => ids.Any(e => e == x.ContactId)));

            if (newUser.Contacts == null) return;
            foreach (var item in newUser.Contacts)
            {
                if (oldUser.Contacts == null) continue;
                var x = oldUser.Contacts.FirstOrDefault(s => s.ContactId == item.ContactId && s.ContactId != Guid.Empty && item.ContactId != Guid.Empty);
                if (x != null)
                {
                    x.Atualizar(item);
                    _db.Entry(x).CurrentValues.SetValues(item);
                }
                else
                {
                    oldUser.Contacts.Add(item);
                }
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