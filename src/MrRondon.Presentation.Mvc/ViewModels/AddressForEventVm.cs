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
        [MaxLength(60, ErrorMessage = "Máximo {0} caracteres")]
        public string Street { get; set; }

        [Display(Name = "Bairro")]
        [MaxLength(30, ErrorMessage = "Máximo {0} caracteres")]
        public string Neighborhood { get; set; }

        [Display(Name = "Número")]
        [MaxLength(6, ErrorMessage = "Máximo {0} caracteres")]
        public string Number { get; set; }

        [DisplayName("Complemento")]
        public string AdditionalInformation { get; set; }

        [Display(Name = "CEP")]
        [MaxLength(10, ErrorMessage = "Máximo {0} caracteres")]
        public string ZipCode { get; set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

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

        [DisplayName("Cidade")]
        public int CityId { get; set; }
        public City City { get; set; }

        public Address GetAddress()
        {
            return new Address
            {
                AddressId = AddressId,
                LongitudeString = LongitudeString,
                Number = Number,
                Street = Street,
                ZipCode = ZipCode,
                LatitudeString = LatitudeString,
                Neighborhood = Neighborhood,
                City = City,
                CityId = CityId,
                AdditionalInformation = AdditionalInformation
           
            };
        }

        public AddressForEventVm SetCoordinates(string latitude, string longitude)
        {
            if (double.TryParse(latitude.Replace(".", ","), out var latitudeResult))
                Latitude = latitudeResult;
            else throw new Exception("Latitude em formato inválido");

            if (double.TryParse(longitude.Replace(".", ","), out var longitudeResult))
                Longitude = longitudeResult;
            else throw new Exception("Longitude em formato inválido");

            return this;
        }
    }
}