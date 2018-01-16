using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Infrastructure;
using MrRondon.Infra.Security.Entities;
using MrRondon.Infra.Security.Helpers;
using MrRondon.Services.Api.Context;

namespace MrRondon.Services.Api.Authorization
{
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientid = context.Ticket.Properties.Dictionary["as:client_id"];

            if (string.IsNullOrEmpty(clientid)) return;

            var refreshTokenId = Guid.NewGuid().ToString("n");

            var repo = new MainContext();

            var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");
            var token = new RefreshToken
            {
                Id = PasswordHelper.GetHash(refreshTokenId),
                ClientId = clientid,
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

            var hashedTokenId = PasswordHelper.GetHash(context.Token);

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