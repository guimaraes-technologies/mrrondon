using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MrRondon.Domain.Entities;
using MrRondon.Infra.CrossCutting.Message;

namespace MrRondon.Presentation.Mvc.ViewModels
{
    public class AddressForHistoricalSightVm
    {
        public Guid AddressId { get; set; } = Guid.NewGuid();

        [Display(Name = "Rua/Avenida")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        [MinLength(5, ErrorMessage = "Mínimo {0} caracteres")]
        [MaxLength(60, ErrorMessage = "Máximo {0} caracteres")]
        public string Street { get; set; }

        [Display(Name = "Bairro")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        [MinLength(4, ErrorMessage = "Mínimo {0} caracteres")]
        [MaxLength(30, ErrorMessage = "Máximo {0} caracteres")]
        public string Neighborhood { get; set; }

        [Display(Name = "Número")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        [MinLength(2, ErrorMessage = "Mínimo {0} caracteres")]
        [MaxLength(6, ErrorMessage = "Máximo {0} caracteres")]
        public string Number { get; set; }

        [DisplayName("Complemento")]
        public string AdditionalInformation { get; set; }

        [Display(Name = "CEP")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        [MaxLength(10, ErrorMessage = "Máximo {0} caracteres")]
        public string ZipCode { get; set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        [DisplayName("Cidade")]
        public int CityId { get; set; }
        public City City { get; set; }

        public Address GetAddress()
        {
            return new Address
            {
                AddressId = AddressId,
                Longitude = Longitude,
                Number = Number,
                Street = Street,
                ZipCode = ZipCode,
                Latitude = Latitude,
                Neighborhood = Neighborhood,
                City = City,
                CityId = CityId,
                AdditionalInformation = AdditionalInformation
            };
        }

        public static AddressForHistoricalSightVm  GetAddress(Address address)
        {
            return new AddressForHistoricalSightVm
            {
                AddressId = address.AddressId,
                Longitude = address.Longitude,
                Number = address.Number,
                Street = address.Street,
                ZipCode = address.ZipCode,
                Latitude = address.Latitude,
                Neighborhood = address.Neighborhood,
                City = address.City,
                CityId = address.CityId,
                AdditionalInformation = address.AdditionalInformation
            };
        }
    }
}