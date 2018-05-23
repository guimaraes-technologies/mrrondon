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
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (!context.TryGetBasicCredentials(out _, out var clientSecret))
                context.TryGetFormCredentials(out _, out clientSecret);

            if (context.ClientId == null)
            {
                //Remove the comments from the below line context.SetError, and invalidate context 
                //if you want to force sending clientId/secrects once obtain access tokens. 
                context.Rejected();
                context.SetError("invalid_clientId", "Chave do cliente deve ser enviada.");
                await Task.FromResult<object>(null);
                return;
            }

            var repo = new MainContext();
            var client = await repo.ApplicationClients.FirstOrDefaultAsync(f => f.Name.Equals(context.ClientId));

            if (client == null)
            {
                context.SetError("invalid_clientId", $"Cliente '{context.ClientId}' não está registrado no sistema.");
                await Task.FromResult<object>(null);
                return;
            }

            if (client.ApplicationType == ApplicationTypes.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("invalid_clientId", "Chave do cliente deveria ser enviada.");
                    await Task.FromResult<object>(null);
                    return;
                }
                if (client.Secret != clientSecret)
                {
                    context.SetError("invalid_clientId", "Chave do cliente é inválida.");
                    await Task.FromResult<object>(null);
                    return;
                }
            }

            if (!client.Active)
            {
                context.SetError("invalid_clientId", "Cliente está desativado.");
                await Task.FromResult<object>(null);
                return;
            }

            context.OwinContext.Set("as:clientAllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());

            context.Validated();
            await Task.FromResult<object>(null);
            return;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin") ?? "*";

                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

                var identity = ValidateAccess(context.UserName, context.Password, context.Options.AuthenticationType);

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

        private static ClaimsIdentity ValidateAccess(string username, string password, string authenticationType = OAuthDefaults.AuthenticationType)
        {
            var repo = new MainContext();
            var user = repo.Users.Include(i => i.Roles).FirstOrDefault(f => f.Cpf == username);
            var claims = AccountManager.ValidateLogin(user, password, authenticationType);

            repo.Entry(user).State = EntityState.Modified;
            repo.SaveChanges();
            return claims;
        }
    }
}