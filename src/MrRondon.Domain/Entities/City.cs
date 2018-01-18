using System.ComponentModel.DataAnnotations;

namespace MrRondon.Domain.Entities
{
    public class City
    {
        [Key]
        public int CityId { get; set; }

        [Required(ErrorMessage = "Campo {0} obrigatório")]
        public string Name { get; set; }
    }
}