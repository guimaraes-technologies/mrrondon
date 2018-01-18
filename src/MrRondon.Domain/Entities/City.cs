using System.ComponentModel.DataAnnotations;

namespace MrRondon.Domain.Entities
{
    public class City
    {
        [Key]
        public int CityId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Campo {0} obrigatório")]
        [MinLength(4, ErrorMessage = "Mínimo {0} caracteres")]
        [MaxLength(30, ErrorMessage = "Máximo {0} caracteres")]
        public string Name { get; set; }
    }
}