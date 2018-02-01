using System;
using System.ComponentModel.DataAnnotations;

namespace MrRondon.Domain.Entities
{
    public class RefreshToken
    {
        [Key]
        public string RefreshTokenId { get; set; }
        public string Subject { get; set; }
        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiresUtc { get; set; }
        public string ProtectedTicket { get; set; }

        public Guid ApplicationClientId { get; set; }
        public ApplicationClient ApplicationClient { get; set; }
    }
}