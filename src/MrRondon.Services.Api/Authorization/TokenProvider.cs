using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using MrRondon.Domain.Entities;
using MrRondon.Infra.Data.Context;
using MrRondon.Infra.Security.Helpers;

namespace MrRondon.Services.Api.Authorization
{
    public class TokenProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            var clientId = string.Empty;
            var clientSecret = string.Empty;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                //Remove the comments from the below line context.SetError, and invalidate context 
                //if you want to force sending clientId/secrects once obtain access tokens. 
                context.Rejected();
                context.SetError("invalid_clientId", "Chave do cliente deve ser enviada.");
                return Task.FromResult<object>(null);
            }

            var repo = new MainContext();
            var client = repo.Clients.Find(context.ClientId);

            if (client == null)
            {
                context.SetError("invalid_clientId", $"Cliente '{context.ClientId}' não está registrado no sistema.");
                return Task.FromResult<object>(null);
            }

            if (client.ApplicationType == ApplicationTypes.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("invalid_clientId", "Chave do cliente deveria ser enviada.");
                    return Task.FromResult<object>(null);
                }
                if (client.Secret != clientSecret)
                {
                    context.SetError("invalid_clientId", "Chave do cliente é inválida.");
                    return Task.FromResult<object>(null);
                }
            }

            if (!client.Active)
            {
                context.SetError("invalid_clientId", "Cliente está desativado.");
                return Task.FromResult<object>(null);
            }

            context.OwinContext.Set("as:clientAllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());

            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin") ?? "*";

                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

                var identity = ValidAccess(context.UserName, context.Password, context.Options.AuthenticationType);

                var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "as:client_id", context.ClientId ?? string.Empty
                    },
                    {
                        "UserName", context.UserName
                    }
                });

                var ticket = new AuthenticationTicket(identity, props);
                context.Validated(ticket);
            }
            catch (Exception ex)
            {
                context.SetError("invalid_grant", ex.Message);
            }
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            var newClaim = newIdentity.Claims.FirstOrDefault(c => c.Type == "newClaim");
            if (newClaim != null)
            {
                newIdentity.RemoveClaim(newClaim);
            }

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (var property in context.Properties.Dictionary)
                context.AdditionalResponseParameters.Add(property.Key, property.Value);

            return Task.FromResult<object>(null);
        }

        private static ClaimsIdentity ValidAccess(string username, string password, string authenticationType = OAuthDefaults.AuthenticationType)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                throw new Exception("Login ou senha incorreta");

            var repo = new MainContext();
            var user = repo.Users.FirstOrDefault(f => f.Email == username);

            if (user == null) throw new Exception("Login ou senha incorreta");
            if (user.LockoutEnd.HasValue && DateTime.Now < user.LockoutEnd)
                throw new Exception("Sua conta foi temporariamente bloqueada por exceder o número de tentativas inválidas, tente novamente mais tarde.");

            var hashedPassword = user.Password;

            if (hashedPassword != null && PasswordHelper.VerifyHash(password, "SHA512", hashedPassword))
            {
                user.AccessFailed = 0;
                user.LastLogin = DateTime.Now;
                user.LockoutEnd = null;
            }
            else
            {
                if (user.AccessFailed == 5 && !user.LockoutEnd.HasValue)
                    user.LockoutEnd = DateTime.Now.AddMinutes(2);
                else user.AccessFailed = user.AccessFailed + 1;
            }

            repo.Entry(user).State = EntityState.Modified;
            repo.SaveChanges();

            if (user.AccessFailed > 0) throw new Exception("Login ou senha incorreta");

            return user.GetClaims(authenticationType);
        }
    }
}