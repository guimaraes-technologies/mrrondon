using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using MrRondon.Infra.CrossCutting.Message;
using MrRondon.Infra.Security.Entities;

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

        public static bool IsAuthenticated => Current.Identity.IsAuthenticated;
        public Guid UserId { get; private set; }
        public string FullName { get; private set; }

        public static void Signin(User user, string password)
        {
            if (user == null) throw new Exception(Error.WrongUserNameOrPassword);

            if (user.LockoutEnd.HasValue && DateTime.Now < user.LockoutEnd)
                throw new Exception("Sua conta foi temporariamente bloqueada por exceder o número de tentativas inválidas, tente novamente mais tarde.");

            var passwordHash = PasswordHelper.ComputeHash(password);
            if (PasswordHelper.VerifyHash(passwordHash, user.Password))
            {
                user.AccessFailed = 0;
                user.LastLogin = DateTime.Now;
                user.LockoutEnd = null;
                Auth.SignIn(user.GetClaims(OAuthDefaults.AuthenticationType));
            }
            else
            {
                if (user.AccessFailed == 5)
                {
                    if (!user.LockoutEnd.HasValue) user.LockoutEnd = DateTime.Now.AddMinutes(2);
                }
                else user.AccessFailed = user.AccessFailed + 1;
            }
            
            if (user.AccessFailed > 0) throw new Exception(Error.WrongUserNameOrPassword);
        }
    }
}