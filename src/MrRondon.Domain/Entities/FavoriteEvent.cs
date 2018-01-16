using System;

namespace MrRondon.Domain.Entities
{
    public class FavoriteEvent
    {
        public Guid FavoriteEventId { get; set; }

        public Guid EventId { get; set; }
        public Event Event { get; set; }

        public Guid UserId { get; set; }
        public Person User { get; set; }
    }
}