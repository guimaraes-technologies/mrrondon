using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MrRondon.Domain.Entities;
using MrRondon.Infra.CrossCutting.Helper;
using MrRondon.Infra.CrossCutting.Message;
using MrRondon.Infra.Data.Context;
using MrRondon.Infra.Data.Repositories;
using MrRondon.Infra.Security.Helpers;
using MrRondon.Presentation.Mvc.Extensions;
using MrRondon.Presentation.Mvc.ViewModels;

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
                .FirstOrDefault(f => f.UserId == Account.Current.UserId);
            return View(user);
        }

        public ActionResult Edit()
        {
            var user = _db.Users
                .Include(i => i.Contacts)
                .FirstOrDefault(f => f.UserId == Account.Current.UserId);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserContactVm model)
        {
            try
            {
                ModelState.Remove("Cpf");
                ModelState.Remove("RolesIds");
                if (!ModelState.IsValid) return View(model).Error(ModelState);

                var result = _usuarioAppService.AtualizarInfo(model);
                if (result.IsValid) return RedirectToAction("Details").Success(Success.Saved);

                ViewBag.Erros = result.Erros.Select(validationError => validationError.Message);

                throw new Exception(Error.ModelState);
            }
            catch (Exception e)
            {
                return View(model).Error(e.Message);
            }
        }

        [AllowAnonymous]
        public ActionResult RecoveryPassword()
        {
            return View();
        }

        [HttpPost, AllowAnonymous]
        public async Task<ActionResult> RecoveryPassword(RecoveryPasswordVm model)
        {
            try
            {
                if (!ModelState.IsValid) return View(model);

                var repo = new RepositoryBase<User>(_db);
                var user = repo.GetItemByExpression(x => x.Cpf.Equals(model.UserName), x => x.Contacts);
                if (user == null) return View(model).Success("Um código para redefinição da sua senha foi enviado para o seu email");
                user.GeneratePasswordRecoveryCode();

                var query = $"UPDATE [{nameof(Domain.Entities.User)}] SET {nameof(user.PasswordRecoveryCode)}='{user.PasswordRecoveryCode}' WHERE {nameof(user.UserId)}='{user.UserId}'";
                await _db.Database.ExecuteSqlCommandAsync(query);

                var email = user.Contacts.FirstOrDefault(f => f.ContactType == ContactType.Email)?.Description;

                if (string.IsNullOrWhiteSpace(email))
                    return View(model).Error("Não foi possível enviar um email com o seu código de recuperação, pois não existe nenhum Email para contato");

                var emailManager = new EmailManager(new ArrayList { email });

                emailManager.ForgotPassword(user.FullName, $"{Request.Url.Authority}/account/newpassword/{user.PasswordRecoveryCode}");
                await emailManager.SendAsync(emailManager.Sender, "Sistema Mr Rondon Turismo");

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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult NewPassword(NewPasswordVm model)
        //{
        //    try
        //    {
        //        if (captcha != HttpContext.Session["captcha"].ToString())
        //            return View(model).Error("O código informado não está correto!");

        //        if (!ModelState.IsValid) return View().Error(Error.Default);
        //        var user = _db.Users.Find(Account.Current.UserId);
        //        if (user == null)
        //            if (!_usuarioAppService.VerificarSenha(model.SenhaAntiga, user.UserId)) throw new Exception("Senha antiga não confere.");
        //        _usuarioAppService.AlterarSenha(user.UserId, model.ConfirmarSenha);
        //        return RedirectToAction("Detalhes").Success(Success.Saved);
        //    }
        //    catch (Exception e)
        //    {
        //        return View().Error(e.Message);
        //    }
        //}

        /*
        [AllowAnonymous]
        public ActionResult ChangePassword(Guid id)
        {
            return View(new ChangePasswordVm { UserId = id });
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordVm changePassword)
        {
            if (ModelState.IsValid)
            {
                _usuarioAppService.AlterarSenha(changePassword.UserId, changePassword.ConfirmarSenha);
                return RedirectToAction("Signin").Success(Success.ChangePassword);
            }
            return View().Error(Error.ModelState);
        }

        [AllowAnonymous]
        public ActionResult AlterarSenhaComCodigo(string codigo)
        {
            return View(new AlterarSenhaComCodigoVm { Codigo = codigo });
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult AlterarSenhaComCodigo(AlterarSenhaComCodigoVm model)
        {
            try
            {
                if (!ModelState.IsValid) return View(model).Error(Error.ModelState);

                _usuarioAppService.AlterarSenha(model);
                return RedirectToAction("Signin").Success(Success.ChangePassword);
            }
            catch (Exception ex)
            {
                return View(model).Error(ex.Message);
            }
        }*/

        [AllowAnonymous]
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (!Account.Current.IsAuthenticated) return RedirectToAction("Signin", "Account", new { area = "" });

            if (!string.IsNullOrWhiteSpace(returnUrl) && Account.Current.IsAuthenticated)
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Category", new { area = "Admin" });
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