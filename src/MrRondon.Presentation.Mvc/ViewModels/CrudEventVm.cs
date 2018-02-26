using System;
using System.Collections.Generic;
using System.Web;
using MrRondon.Domain.Entities;

namespace MrRondon.Presentation.Mvc.ViewModels
{
    public class CrudEventVm
    {
        public Guid ContactId { get; set; }
        public string Description { get; set; }
        public ContactType ContactType { get; set; }

        public Event Event { get; set; }
        public AddressForEventVm Address { get; set; }

        public HttpPostedFileBase CoverFile { get; set; }
        public HttpPostedFileBase LogoFile { get; set; }
        public List<Contact> Contacts { get; set; }

        public Address GetAddress(AddressForEventVm address)
        {
            return new Address
            {
                Latitude = address.Latitude,
                Longitude = address.Longitude,
                LatitudeString = address.LatitudeString,
                LongitudeString = address.LongitudeString,
                Number = address.Number ?? string.Empty,
                Street = address.Street ?? string.Empty,
                ZipCode = address.ZipCode ?? string.Empty,
                Neighborhood = address.Neighborhood ?? string.Empty,
                AdditionalInformation = address.AdditionalInformation ?? string.Empty,
                City = address.City,
                CityId = address.CityId
            };
        }
    }
}