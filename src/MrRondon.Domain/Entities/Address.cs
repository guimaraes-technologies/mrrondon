using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MrRondon.Infra.CrossCutting.Message;

namespace MrRondon.Domain.Entities
{
    public class Address
    {
        [Key] public Guid AddressId { get; set; } = Guid.NewGuid();

        [Display(Name = "Rua/Avenida")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required", AllowEmptyStrings = true)]
        [MaxLength(60, ErrorMessage = "Máximo {1} caracteres")]
        public string Street { get; set; }

        [Display(Name = "Bairro")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required", AllowEmptyStrings = true)]
        [MaxLength(30, ErrorMessage = "Máximo {1} caracteres")]
        public string Neighborhood { get; set; }

        [Display(Name = "Número")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required", AllowEmptyStrings = true)]
        [MaxLength(6, ErrorMessage = "Máximo {1} caracteres")]
        public string Number { get; set; }

        [DisplayName("Complemento")] public string AdditionalInformation { get; set; }

        [Display(Name = "CEP")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required", AllowEmptyStrings = true)]
        [MaxLength(10, ErrorMessage = "Máximo {1} caracteres")]
        public string ZipCode { get; set; }

        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public double Latitude { get; set; }

        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public double Longitude { get; set; }

        [NotMapped]
        [Display(Name = "Latitude")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required", AllowEmptyStrings = true)]
        public string LatitudeString { get; set; }

        [NotMapped]
        [Display(Name = "Longitude")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public string LongitudeString { get; set; }

        [DisplayName("Cidade")]
        public int CityId { get; set; }
        public City City { get; set; }

        public void UpdateAddress(Address newAddress)
        {
            Street = newAddress.Street;
            Neighborhood = newAddress.Neighborhood;
            Number = newAddress.Number;
            AdditionalInformation = newAddress.AdditionalInformation;
            ZipCode = newAddress.ZipCode;
            Latitude = newAddress.Latitude;
            LatitudeString = newAddress.Latitude.ToString();
            Longitude = newAddress.Longitude;
            LongitudeString = newAddress.Longitude.ToString();
            CityId = newAddress.CityId;
        }

        public void SetCoordinates()
        {
            LatitudeString = Latitude.ToString().Replace(",", ".");
            LongitudeString = Longitude.ToString().Replace(",", ".");
        }

        public Address SetCoordinates(string latitude, string longitude)
        {
            if (string.IsNullOrWhiteSpace(latitude) || latitude.Equals("0")) return this;

            if (string.IsNullOrWhiteSpace(longitude) || longitude.Equals("0")) return this;

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