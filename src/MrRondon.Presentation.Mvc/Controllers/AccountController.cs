using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MrRondon.Infra.CrossCutting.Message;
using MrRondon.Infra.Data.Context;
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
            return AccountManager.IsAuthenticated ? RedirectToArea() : View(new SigninVm { ReturnUrl = returnUrl });
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signin(SigninVm signin)
        {
            if (!ModelState.IsValid) return View(signin).Error(Error.Default);
            var user = _db.Users.FirstOrDefault(f => f.Email.Equals(signin.UserName));
            
            AccountManager.Signin(user, signin.Password);

            if (!_usuarioAppService.VerificarSenha(signin.Senha, user.UsuarioId)) return View(signin).Error(Error.WrongUserNameOrPassword);

            return !string.IsNullOrEmpty(signin.ReturnUrl) ? RedirectToLocal(signin.ReturnUrl) : RedirectToArea(user);
        }

        public ActionResult NovaSenha()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NovaSenha(SenhaVm senhaVm, string captcha)
        {
            try
            {
                if (captcha != HttpContext.Session["captcha"].ToString())
                    return View(senhaVm).Error("O código informado não está correto!");

                if (!ModelState.IsValid) return View().Error(Error.ModelState);
                var user = _usuarioAppService.ObterPorId(Account.UsuarioId);

                if (!_usuarioAppService.VerificarSenha(senhaVm.SenhaAntiga, user.UsuarioId))
                    throw new Exception("Senha antiga não confere.");
                _usuarioAppService.AlterarSenha(user.UsuarioId, senhaVm.ConfirmarSenha);
                return RedirectToAction("Detalhes").Success(Success.Saved);
            }
            catch (Exception e)
            {
                return View().Error(e.Message);
            }
        }

        [AllowAnonymous]
        public ActionResult AlterarSenha(Guid id)
        {
            return View(new AlterarSenha { UsuarioId = id });
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AlterarSenha(AlterarSenha changePassword)
        {
            if (ModelState.IsValid)
            {
                _usuarioAppService.AlterarSenha(changePassword.UsuarioId, changePassword.ConfirmarSenha);
                return RedirectToAction("Signin").Success(Success.ChangePassword);
            }
            return View().Error(Error.ModelState);
        }

        public ActionResult Detalhes()
        {
            return View(_usuarioAppService.ObterPorId(Account.UsuarioId));
        }

        public ActionResult Editar()
        {
            return View(_usuarioAppService.ObterPorIdCustom(Account.UsuarioId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(UsuarioContatoVm model)
        {
            try
            {
                ModelState.Remove("Cpf");
                ModelState.Remove("RolesIds");
                if (!ModelState.IsValid) return View(model).Error(Error.ModelState);
                if (Account.EntidadeId > 0) model.EntidadeId = Account.EntidadeId;
                var result = _usuarioAppService.AtualizarInfo(model);
                if (result.IsValid) return RedirectToAction("Detalhes").Success(Success.Saved);

                ViewBag.Erros = result.Erros.Select(validationError => validationError.Message);

                throw new Exception(Error.ModelState);
            }
            catch (Exception e)
            {
                return View(model).Error(e.Message);
            }
        }

        public ActionResult Signout()
        {
            Authentication.Signout("ApplicationCookie");
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        [AllowAnonymous]
        public ActionResult RecuperarSenha()
        {
            return View();
        }

        [HttpPost, AllowAnonymous]
        public async Task<ActionResult> RecuperarSenha(RecuperarSenhaVm model)
        {
            try
            {
                if (!ModelState.IsValid) return View(model);

                var user = await _usuarioAppService.RecuperarSenha(model.UserName);

                var email = user.ListaContatos.FirstOrDefault(f => f.TipoContato == TipoContatoVm.Email)?.Descricao;

                return string.IsNullOrWhiteSpace(email) ? View(model).Error("Não foi possível enviar um email com o seu código de recuperação, pois não existe nenhum Email para contato") : View(model).Success($"Um código para redefinição da sua senha foi enviado para o seu email: '{email}'");
            }
            catch (Exception e)
            {
                return View(model).Error(e.Message);
            }
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
        }

        #region APOIO

        [AllowAnonymous]
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        [AllowAnonymous]
        private ActionResult RedirectToArea()
        {
            if (User.IsInRole("Coordenador"))
            {
                return RedirectToAction("Index", "Home", new { area = "Coordenador" });
            }
            if (User.IsInRole("Atleta"))
            {
                return RedirectToAction("Index", "Home", new { area = "Atleta" });
            }
            if (User.IsInRole("Entidade"))
            {
                return RedirectToAction("Index", "Home", new { area = "Entidade" });
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        private ActionResult RedirectToArea(UsuarioVm user)
        {
            if (user.ListaRole.Any(x => x.Nome.Equals("Coordenador")))
            {
                return RedirectToAction("Index", "Processo", new { area = "Coordenador" });
            }
            if (user.ListaRole.Any(x => x.Nome.Equals("Atleta")))
            {
                return RedirectToAction("Index", "Processo", new { area = "Atleta" });
            }
            if (user.ListaRole.Any(x => x.Nome.Equals("Entidade")))
            {
                return RedirectToAction("Index", "Atleta", new { area = "Entidade" });
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        #endregion

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