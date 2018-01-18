using System;
using System.ComponentModel.DataAnnotations;

namespace MrRondon.Domain.Entities
{
    public class Company
    {
        [Key]
        public Guid CompanyId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Campo {0} obrigatório")]
        [MinLength(4, ErrorMessage = "Mínimo {0} caracteres")]
        [MaxLength(60, ErrorMessage = "Máximo {0} caracteres")]
        public string Name { get; set; }

        [Display(Name = "Nome Fantasia")]
        [Required(ErrorMessage = "Campo {0} obrigatório")]
        [MinLength(4, ErrorMessage = "Mínimo {0} caracteres")]
        [MaxLength(60, ErrorMessage = "Máximo {0} caracteres")]
        public string FancyName { get; set; }

        [Display(Name = "CNPJ")]
        [Required(ErrorMessage = "Campo {0} obrigatório")]
        [MinLength(14, ErrorMessage = "Mínimo {0} caracteres")]
        [MaxLength(14, ErrorMessage = "Máximo {0} caracteres")]
        public string Cnpj { get; set; }
        
        [Display(Name = "Endereço")]
        [Required(ErrorMessage = "Campo {0} obrigatório")]
        public Guid AddressId { get; set; }
        public Address Address { get; set; }
        
        [Display(Name = "Sub Categoria")]
        [Required(ErrorMessage = "Campo {0} obrigatório")]
        public int SubCategoryId { get; set; }
        public Category SubCategory { get; set; }
    }
}