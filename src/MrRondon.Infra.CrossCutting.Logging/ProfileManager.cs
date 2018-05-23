using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace MrRondon.Infra.CrossCutting.Logging
{
    public sealed class ProfileManager : ClaimsPrincipal
    {
        public ProfileManager() : this((ClaimsPrincipal)HttpContext.Current.User) { }
        public ProfileManager(IPrincipal principal) : base(principal)
        {
            if (!IsAuthenticated) return;
            UserId = Guid.Parse(FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        public bool IsAuthenticated => Identity.IsAuthenticated;
        public Guid UserId { get; }
    }
}