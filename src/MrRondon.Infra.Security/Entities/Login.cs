using System;

namespace MrRondon.Infra.Security.Entities
{
    public class Login
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}