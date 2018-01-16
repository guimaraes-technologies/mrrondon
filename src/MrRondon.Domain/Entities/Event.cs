using System;

namespace MrRondon.Domain.Entities
{
    public class Event
    {
        public Guid EventId { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //todo insert cover image and event image

        public Guid AddressId { get; set; }
        public Address Address { get; set; }
    }
}