using System;
using System.ComponentModel.DataAnnotations;

namespace MrRondon.Infra.Security.Entities
{
    public class Client
    {
        [Key]
        public Guid ClientId { get; set; }
        public string Secret { get; set; }
        public string Name { get; set; }
        public ApplicationTypes ApplicationType { get; set; }
        public int RefreshTokenLifeTime { get; set; }
        public string AllowedOrigin { get; set; }
        public bool Active { get; set; }
    }

    public enum ApplicationTypes
    {
        JavaScript = 0,
        NativeConfidential = 1
    }
}