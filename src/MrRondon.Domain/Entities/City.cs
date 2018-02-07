using System.ComponentModel.DataAnnotations;
using MrRondon.Infra.CrossCutting.Message;

namespace MrRondon.Domain.Entities
{
    public class City
    {
        [Key]
        public int CityId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        [MinLength(4, ErrorMessage = "Mínimo {0} caracteres")]
        [MaxLength(30, ErrorMessage = "Máximo {0} caracteres")]
        public string Name { get; set; }
    }
}