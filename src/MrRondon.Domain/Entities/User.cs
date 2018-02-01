using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;

namespace MrRondon.Domain.Entities
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
        public string PasswordRecoveryCode { get; private set; }

        public ICollection<Role> Roles { get; set; }
        public ICollection<Login> Logins { get; set; }
        public ICollection<Contact> Contacts { get; set; }

        public ClaimsIdentity GetClaims(string authenticationType)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, UserId.ToString()),
                new Claim(ClaimTypes.Name, Email),
                new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "GT - Guimaraes Tecnologia")
            };
            return new ClaimsIdentity(claims, authenticationType);
        }

        public string GeneratePasswordRecoveryCode()
        {
            var codes = Guid.NewGuid().ToString().ToUpper().Split('-');
            return PasswordRecoveryCode = $"GT-{codes[codes.Length - 1]}";
        }
    }
}