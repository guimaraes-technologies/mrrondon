using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using MrRondon.Presentation.Mvc;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace MrRondon.Presentation.Mvc
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/account/signin")
            });
        }
    }
}