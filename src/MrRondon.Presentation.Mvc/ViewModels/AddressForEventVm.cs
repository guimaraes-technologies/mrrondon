using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MrRondon.Domain.Entities;

namespace MrRondon.Presentation.Mvc.ViewModels
{
    public class AddressForEventVm
    {
        public Guid AddressId { get; set; } = Guid.NewGuid();

        [Display(Name = "Rua/Avenida")]
        [MaxLength(60, ErrorMessage = "Máximo {0} caracteres")]
        public string Street { get; set; }

        [Display(Name = "Bairro")]
        [MaxLength(30, ErrorMessage = "Máximo {0} caracteres")]
        public string Neighborhood { get; set; }

        [Display(Name = "Número")]
        [MaxLength(6, ErrorMessage = "Máximo {0} caracteres")]
        public string Number { get; set; }

        [DisplayName("Complemento")] public string AdditionalInformation { get; set; }

        [Display(Name = "CEP")]
        [MaxLength(10, ErrorMessage = "Máximo {0} caracteres")]
        public string ZipCode { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [NotMapped]
        public string LatitudeString
        {
            get => Latitude.ToString().Replace(",", ".");
            set => LatitudeString = value;
        }

        [NotMapped]
        public string LongitudeString
        {
            get => Longitude.ToString().Replace(",", ".");
            set => LongitudeString = value;
        }

        [DisplayName("Cidade")] public int CityId { get; set; }
        public City City { get; set; }

        public static AddressForEventVm GetAddress(Address address)
        {
            return new AddressForEventVm
            {
                AddressId = address.AddressId,
                Latitude = address.Latitude,
                Longitude = address.Longitude,
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