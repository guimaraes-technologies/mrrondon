using MrRondon.Infra.Security.Helpers;
using System.Linq;
using System.Web.Mvc;


namespace MrRondon.Infra.Security.Extensions
{
    public class HasAnyAttribute : AuthorizeAttribute
    {
        public string[] Permissions { get; set; }

        public HasAnyAttribute(params string[] permissions)
        {
            Permissions = permissions;
        }

        public override void OnAuthorization(AuthorizationContext context)
        {
            var urlHelper = new UrlHelper(context.RequestContext);
            if (Account.Current.IsAuthenticated)
            {
                if (!Permissions.Any() && !string.IsNullOrWhiteSpace(Roles)) Permissions = Roles.Split(',');
                if (Account.Current.HasAny(Permissions)) base.OnAuthorization(context);
                else
                {
                    context.Result = new RedirectResult(urlHelper.Action("Code", "Error", new { area = "Admin", id = 401 }));
                    HandleUnauthorizedRequest(context);
                }
            }
            else
            {
                context.Result = new RedirectResult(urlHelper.Action("Code", "Error", new { area = "Admin", id = 401 }));
                HandleUnauthorizedRequest(context);
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext) { }
    }
}