using System;
using System.ComponentModel.DataAnnotations;

namespace MrRondon.Infra.Security.Entities
{
    public class Claim
    {
        [Key]
        public Guid ClaimId { get; set; }
        public string ClaimType { get; set; }

        public Guid ClaimValue { get; set; }
        public string RoleName { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}