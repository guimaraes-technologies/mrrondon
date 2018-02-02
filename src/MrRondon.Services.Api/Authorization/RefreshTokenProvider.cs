using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Infrastructure;
using MrRondon.Domain;
using MrRondon.Domain.Entities;
using MrRondon.Infra.Data.Context;
using MrRondon.Infra.Security.Helpers;

namespace MrRondon.Services.Api.Authorization
{
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientName = context.Ticket.Properties.Dictionary["as:client_id"];

            if (string.IsNullOrEmpty(clientName)) return;

            var refreshTokenId = Guid.NewGuid().ToString("n");

            var repo = new MainContext();
            var client = await repo.Clients.FindAsync(clientName);
            if (client == null) return;

            var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");
            var token = new RefreshToken
            {
                RefreshTokenId = PasswordAssertionConcern.GetHash(refreshTokenId),
                ApplicationClientId = client.ApplicationClientId,
                Subject = context.Ticket.Identity.Name,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
            };
            context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
            context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;
            token.ProtectedTicket = context.SerializeTicket();

            repo.RefreshTokens.Add(token);
            var result = await repo.SaveChangesAsync() > 0;

            if (result) context.SetToken(refreshTokenId);
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            var hashedTokenId = PasswordAssertionConcern.GetHash(context.Token);

            var repo = new MainContext();
            var refreshToken = await repo.RefreshTokens.FindAsync(hashedTokenId);

            if (refreshToken != null)
            {
                //Get protectedTicket from refreshToken class
                context.DeserializeTicket(refreshToken.ProtectedTicket);
                repo.RefreshTokens.Remove(refreshToken);
                await repo.SaveChangesAsync();
            }
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }
    }
}