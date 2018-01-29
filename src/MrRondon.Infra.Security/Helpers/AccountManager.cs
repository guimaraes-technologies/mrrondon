using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace MrRondon.Infra.Security.Helpers
{
    public sealed class AccountManager : ClaimsPrincipal
    {
        public AccountManager() : this((ClaimsPrincipal)HttpContext.Current.User) { }
        public AccountManager(IPrincipal principal) : base(principal)
        {
            if (!IsAuthenticated) return;

            UserId = Guid.Parse(FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        public bool IsAuthenticated => Identity.IsAuthenticated;
        public Guid UserId { get; private set; }
    }
}