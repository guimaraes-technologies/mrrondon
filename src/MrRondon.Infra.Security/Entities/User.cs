using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Owin.Security;
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
        
        public ClaimsIdentity GetClaims(string authenticationType = OAuthDefaults.AuthenticationType)
        {
            var claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(ClaimTypes.NameIdentifier, UserId.ToString()),
                new System.Security.Claims.Claim(ClaimTypes.Name, Email),
                new System.Security.Claims.Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "GT - Guimaraes Tecnologia")
            };
            return new ClaimsIdentity(claims, authenticationType);
        }
    }
}