using System;
using System.ComponentModel.DataAnnotations;

namespace MrRondon.Domain.Entities
{
    public class HistoricalSight
    {
        [Key]
        public int HistoricalSightId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Campo {0} obrigatório")]
        [MinLength(4, ErrorMessage = "Mínimo {0} caracteres")]
        [MaxLength(30, ErrorMessage = "Máximo {0} caracteres")]
        public string Name { get; set; }

        [Display(Name = "História")]
        [Required(ErrorMessage = "Campo {0} obrigatório")]
        [MinLength(50, ErrorMessage = "Mínimo {0} caracteres")]
        [MaxLength(500, ErrorMessage = "Máximo {0} caracteres")]
        public string SightHistory { get; set; }

        [Display(Name = "Imagem da Logo")]
        //[Required(ErrorMessage = "Campo {0} obrigatório")]
        public byte[] Logo { get; set; }

        [Display(Name = "Imagem da Capa")]
        //[Required(ErrorMessage = "Campo {0} obrigatório")]
        public byte[] Cover { get; set; }

        [Display(Name = "Endereço")]
        [Required(ErrorMessage = "Campo {0} obrigatório")]
        public Guid AddressId { get; set; }
        public Address Address { get; set; }
    }
}