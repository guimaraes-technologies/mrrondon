using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Owin.Security.OAuth;

namespace MrRondon.Infra.Security.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int AccessFailed { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LockoutEndDate { get; set; }
        public DateTime CreateonDate { get; set; }

        public virtual ICollection<Claims> Claims { get; set; }
        public virtual ICollection<Login> Logins { get; set; }

        public void AddLogin(string loginProvider, string providerKey)
        {
            Logins = Logins ?? new List<Login>();
            Logins.Add(new Login { LoginProvider = loginProvider, ProviderKey = providerKey });
        }

        public void AddClaim(Claims c)
        {
            Claims = Claims ?? new List<Claims>();
            Claims.Add(c);
        }
        
        public ClaimsIdentity ClaimsToApi(string authenticationType = OAuthDefaults.AuthenticationType)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, UserId.ToString()),
                new Claim(ClaimTypes.Name, Email)
            };
            return new ClaimsIdentity(claims, authenticationType);
        }
    }
}