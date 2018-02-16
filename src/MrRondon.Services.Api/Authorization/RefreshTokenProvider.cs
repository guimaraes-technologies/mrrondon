using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Infrastructure;
using MrRondon.Domain;
using MrRondon.Domain.Entities;
using MrRondon.Infra.Data.Context;
using MrRondon.Infra.Data.Repositories;

namespace MrRondon.Services.Api.Authorization
{
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientName = context.Ticket.Properties.Dictionary["as:client_id"];

            if (string.IsNullOrEmpty(clientName)) return;

            var refreshTokenId = Guid.NewGuid().ToString("n");
            var db = new MainContext();
            var repo = new RepositoryBase<ApplicationClient>(db);
            var applicationClientId = repo.GetItemByExpression(f => f.Name.Equals(clientName))?.ApplicationClientId;
            if (!applicationClientId.HasValue) return;

            var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");
            var token = new RefreshToken
            {
                RefreshTokenId = PasswordAssertionConcern.GetHash(refreshTokenId),
                ApplicationClientId = applicationClientId.Value,
                Subject = context.Ticket.Identity.Name,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
            };
            context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
            context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;
            token.ProtectedTicket = context.SerializeTicket();

            var refreshTokenRepo = new RepositoryRefreshToken(db);
            var result = await refreshTokenRepo.AddAsync(token);

            if (result) context.SetToken(refreshTokenId);
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            var hashedTokenId = PasswordAssertionConcern.GetHash(context.Token);

            var repo = new RepositoryRefreshToken(new MainContext());
            var refreshToken = repo.Find(hashedTokenId);

            if (refreshToken != null)
            {
                //Get protectedTicket from refreshToken class
                context.DeserializeTicket(refreshToken.ProtectedTicket);
                await repo.RemoveAsync(refreshToken);
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