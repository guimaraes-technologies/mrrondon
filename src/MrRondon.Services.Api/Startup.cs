using System;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using MrRondon.Services.Api;
using MrRondon.Services.Api.Authorization;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace MrRondon.Services.Api
{
    public class Startup
    {
        public static HttpConfiguration Config;

        public void Configuration(IAppBuilder app)
        {
            Config = new HttpConfiguration();
            
            //https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/individual-accounts-in-web-api
            //The SuppressDefaultHostAuthentication method tells Web API to ignore any authentication that happens before the request reaches the Web API pipeline, either by IIS or by OWIN middleware. That way, we can restrict Web API to authenticate only using bearer tokens
            Config.SuppressDefaultHostAuthentication();

            Config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            WebApiConfig.Register(Config);                        

            var authAuthorizationServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/security/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1825),
                Provider = new TokenProvider(),
                RefreshTokenProvider = new RefreshTokenProvider()
            };
            
            app.UseOAuthAuthorizationServer(authAuthorizationServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(Config);
        }
    }
}