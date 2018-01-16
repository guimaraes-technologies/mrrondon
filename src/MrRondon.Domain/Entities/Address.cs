using System;

namespace MrRondon.Domain.Entities
{
    public class Address
    {
        public Guid AddressId { get; set; }
        public string Street { get; set; }
        public string Neighborhood { get; set; }
        public string Number { get; set; }
        public string ZipCode { get; set; }
        public int Latitude { get; set; }
        public int Longitude { get; set; }
    }
}