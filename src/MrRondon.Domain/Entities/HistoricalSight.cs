using System;
using System.ComponentModel.DataAnnotations;
using MrRondon.Infra.CrossCutting.Message;

namespace MrRondon.Domain.Entities
{
    public class HistoricalSight
    {
        [Key]
        public int HistoricalSightId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        [MinLength(4, ErrorMessage = "Mínimo {0} caracteres")]
        [MaxLength(30, ErrorMessage = "Máximo {0} caracteres")]
        public string Name { get; set; }

        [Display(Name = "História")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        [MinLength(50, ErrorMessage = "Mínimo {0} caracteres")]
        [MaxLength(500, ErrorMessage = "Máximo {0} caracteres")]
        public string SightHistory { get; set; }

        [Display(Name = "Imagem da Logo")]
        //[Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public byte[] Logo { get; set; }

        [Display(Name = "Imagem da Capa")]
        //[Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public byte[] Cover { get; set; }

        [Display(Name = "Endereço")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public Guid AddressId { get; set; }
        public Address Address { get; set; }
    }
}