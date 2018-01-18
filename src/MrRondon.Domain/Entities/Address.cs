using System;
using System.ComponentModel.DataAnnotations;

namespace MrRondon.Domain.Entities
{
    public class Address
    {
        [Key]
        public Guid AddressId { get; set; }

        [Display(Name = "Rua/Avenida")]
        [Required(ErrorMessage = "Campo {0} obrigatório")]
        [MinLength(5, ErrorMessage = "Mínimo {0} caracteres")]
        [MaxLength(60, ErrorMessage = "Máximo {0} caracteres")]
        public string Street { get; set; }

        [Display(Name = "Bairro")]
        [Required(ErrorMessage = "Campo {0} obrigatório")]
        [MinLength(4, ErrorMessage = "Mínimo {0} caracteres")]
        [MaxLength(30, ErrorMessage = "Máximo {0} caracteres")]
        public string Neighborhood { get; set; }

        [Display(Name = "Número")]
        [Required(ErrorMessage = "Campo {0} obrigatório")]
        [MinLength(2, ErrorMessage = "Mínimo {0} caracteres")]
        [MaxLength(6, ErrorMessage = "Máximo {0} caracteres")]
        public string Number { get; set; }
        
        [Display(Name = "CEP")]
        [Required(ErrorMessage = "Campo {0} obrigatório")]
        [MaxLength(11, ErrorMessage = "Máximo {0} caracteres")]
        public string ZipCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }
    }
}