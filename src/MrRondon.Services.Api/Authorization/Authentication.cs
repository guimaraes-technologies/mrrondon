using MrRondon.Infra.Security.Helpers;

namespace MrRondon.Services.Api.Authorization
{
    public abstract class Authentication
    {
        public static AccountManager Current => new AccountManager();
    }
}