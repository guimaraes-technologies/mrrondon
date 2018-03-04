namespace MrRondon.Infra.Security.Helpers
{
    public sealed class Account
    {
        public static AccountManager Current => new AccountManager();
    }
}