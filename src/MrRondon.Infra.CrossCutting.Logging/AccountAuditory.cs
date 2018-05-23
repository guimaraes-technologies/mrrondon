namespace MrRondon.Infra.CrossCutting.Logging
{
    public abstract class AccountAuditory
    {
        public static ProfileManager Current => new ProfileManager();
    }
}