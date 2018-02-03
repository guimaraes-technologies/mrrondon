using System;
using System.ComponentModel.DataAnnotations;

namespace MrRondon.Domain.Entities
{
    public class Event
    {
        [Key]
        public Guid EventId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Campo {0} obrigatório")]
        [MinLength(4, ErrorMessage = "Mínimo {0} caracteres")]
        [MaxLength(50, ErrorMessage = "Máximo {0} caracteres")]
        public string Name { get; set; }

        [Display(Name = "Valor")]
        [Required(ErrorMessage = "Campo {0} obrigatório")]
        public decimal Value { get; set; }

        [Display(Name = "Início")]
        [Required(ErrorMessage = "Campo {0} obrigatório")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Fim")]
        [Required(ErrorMessage = "Campo {0} obrigatório")]
        public DateTime EndDate { get; set; }

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