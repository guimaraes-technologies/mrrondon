using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using Microsoft.Owin.Security;
using MrRondon.Domain;
using MrRondon.Domain.Entities;
using MrRondon.Infra.CrossCutting.Message;

namespace MrRondon.Infra.Security.Helpers
{
    public sealed class AccountManager : ClaimsPrincipal
    {
        private static IAuthenticationManager Auth => HttpContext.Current.Request.GetOwinContext().Authentication;
        public AccountManager() : this((ClaimsPrincipal)HttpContext.Current.User) { }
        public AccountManager(IPrincipal principal) : base(principal)
        {
            if (!IsAuthenticated) return;

            UserId = Guid.Parse(FindFirst(ClaimTypes.NameIdentifier).Value);
            FullName = FindFirst(ClaimTypes.Name).Value;
        }

        public bool IsAuthenticated => Current.Identity.IsAuthenticated;
        public Guid UserId { get; private set; }
        public string FullName { get; }

        public static void Signin(User user, string password)
        {
            var claims = ValidateLogin(user, password, AuthenticationType.Mvc);
            Auth.SignIn(claims);
        }

        public static void Signout()
        {
            Auth.SignOut("ApplicationCookie");
        }

        public static ClaimsIdentity ValidateLogin(User user, string password, string authenticationType)
        {
            if (user == null) throw new Exception(Error.WrongUserNameOrPassword);

            if (user.LockoutEnd.HasValue && DateTime.Now < user.LockoutEnd)
                throw new Exception("Sua conta foi temporariamente bloqueada por exceder o número de tentativas inválidas, tente novamente mais tarde.");
            if (!user.IsActive) throw new Exception("O seu usuário foi desativado");
            if (PasswordAssertionConcern.VerifyHash(password, user.Password))
            {
                user.AccessFailed = 0;
                user.LastLogin = DateTime.Now;
                user.LockoutEnd = null;
                return user.GetClaims(authenticationType);
            }

            if (user.AccessFailed == 5)
            {
                if (!user.LockoutEnd.HasValue) user.LockoutEnd = DateTime.Now.AddMinutes(2);
            }
            else user.AccessFailed = user.AccessFailed + 1;

            if (user.AccessFailed > 0) throw new Exception(Error.WrongUserNameOrPassword);

            throw new Exception(Error.WrongUserNameOrPassword);
        }
    }
}