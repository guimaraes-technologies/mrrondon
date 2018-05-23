using System;
using System.ComponentModel.DataAnnotations;

namespace MrRondon.Domain.Entities
{
    public class FavoriteEvent
    {
        [Key]
        public Guid FavoriteEventId { get; set; } = Guid.NewGuid();

        public Guid EventId { get; set; }
        public Event Event { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}