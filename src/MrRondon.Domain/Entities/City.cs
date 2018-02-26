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
        [MinLength(4, ErrorMessage = "Mínimo {1} caracteres")]
        [MaxLength(30, ErrorMessage = "Máximo {1} caracteres")]
        public string Name { get; set; }
    }
}