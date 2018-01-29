using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Owin.Security.OAuth;

namespace MrRondon.Infra.Security.Entities
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int AccessFailed { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public DateTime CreateOn { get; set; }

        public virtual ICollection<Claim> Claims { get; set; }
        public virtual ICollection<Login> Logins { get; set; }

        public void AddLogin(string loginProvider, string providerKey)
        {
            Logins = Logins ?? new List<Login>();
            Logins.Add(new Login { LoginProvider = loginProvider, ProviderKey = providerKey });
        }

        public void AddClaim(Claim c)
        {
            Claims = Claims ?? new List<Claim>();
            Claims.Add(c);
        }
        
        public ClaimsIdentity ClaimsToApi(string authenticationType = OAuthDefaults.AuthenticationType)
        {
            var claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(ClaimTypes.NameIdentifier, UserId.ToString()),
                new System.Security.Claims.Claim(ClaimTypes.Name, Email)
            };
            return new ClaimsIdentity(claims, authenticationType);
        }
    }
}