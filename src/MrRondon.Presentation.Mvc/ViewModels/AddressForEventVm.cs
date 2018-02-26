using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MrRondon.Domain.Entities;
using MrRondon.Infra.CrossCutting.Message;

namespace MrRondon.Presentation.Mvc.ViewModels
{
    public class AddressForEventVm
    {
        public Guid AddressId { get; set; } = Guid.NewGuid();

        [Display(Name = "Rua/Avenida")]
        [MaxLength(60, ErrorMessage = "Máximo {1} caracteres")]
        public string Street { get; set; }

        [Display(Name = "Bairro")]
        [MaxLength(30, ErrorMessage = "Máximo {1} caracteres")]
        public string Neighborhood { get; set; }

        [Display(Name = "Número")]
        [MaxLength(6, ErrorMessage = "Máximo {1} caracteres")]
        public string Number { get; set; }

        [DisplayName("Complemento")] public string AdditionalInformation { get; set; }

        [Display(Name = "CEP")]
        [MaxLength(10, ErrorMessage = "Máximo {1} caracteres")]
        public string ZipCode { get; set; }

        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public double Latitude { get; set; }

        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public double Longitude { get; set; }

        [NotMapped]
        [Display(Name = "Latitude")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public string LatitudeString { get; set; }

        [NotMapped]
        [Display(Name = "Longitude")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public string LongitudeString { get; set; }

        [DisplayName("Cidade")] public int CityId { get; set; }
        public City City { get; set; }

        public static AddressForEventVm GetAddress(Address address)
        {
            return new AddressForEventVm
            {
                Latitude = address.Latitude,
                Longitude = address.Longitude,
                Number = address.Number,
                Street = address.Street,
                ZipCode = address.ZipCode,
                Neighborhood = address.Neighborhood,
                AdditionalInformation = address.AdditionalInformation,
                City = address.City,
                CityId = address.CityId,
                LatitudeString = address.Latitude.ToString(),
                LongitudeString = address.Longitude.ToString()
            };
        }
    }
}