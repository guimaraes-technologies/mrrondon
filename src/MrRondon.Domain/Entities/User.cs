using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace MrRondon.Domain.Entities
{
    public class User
    {
        public User()
        {
            UserId = UserId == Guid.Empty ? Guid.NewGuid() : UserId;
        }

        [Key]
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string Cpf { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [MaxLength(100)]
        public string Password { get; private set; }
        public int AccessFailed { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public DateTime CreateOn { get; set; } = DateTime.Now;
        public string PasswordRecoveryCode { get; private set; }
        public bool IsActive { get; set; } = true;

        public ICollection<Role> Roles { get; set; }
        public ICollection<Login> Logins { get; set; }
        public ICollection<Contact> Contacts { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public ClaimsIdentity GetClaims(string authenticationType)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, UserId.ToString()),
                new Claim(ClaimTypes.Name, FullName),
                new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "GT - Guimaraes Technology")
            };

            if (Roles != null) claims.AddRange(Roles.Select(x => new Claim(ClaimTypes.Role, x.Name)));

            return new ClaimsIdentity(claims, authenticationType);
        }

        public string SetPasswordRecoveryCode()
        {
            var codes = Guid.NewGuid().ToString().ToUpper().Split('-');
            return PasswordRecoveryCode = $"GTech_{codes[1]}";
        }

        public string EncryptPassword(string password)
        {
            return PasswordAssertionConcern.ComputeHash(password);
        }

        public void Update(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public void UpdateStatus()
        {
            IsActive = !IsActive;
        }

        public string ResetPassword()
        {
            var digitsRegex = new Regex(@"[^\d]");
            var password = digitsRegex.Replace(Cpf, "");
            return SetNewPassword(password);
        }

        public string SetNewPassword(string newPassword)
        {
            return Password = EncryptPassword(newPassword);
        }

        public void SetPassword(string oldUserPassword)
        {
            Password = oldUserPassword;
        }

        public void SetInfo(User oldUser)
        {
            Cpf = oldUser.Cpf;
            IsActive = oldUser.IsActive;
            LockoutEnd = oldUser.LockoutEnd;
            LastLogin = oldUser.LastLogin;
            AccessFailed = oldUser.AccessFailed;
            CreateOn = oldUser.CreateOn;
            Password = oldUser.Password;
            PasswordRecoveryCode = oldUser.PasswordRecoveryCode;
        }
    }
}