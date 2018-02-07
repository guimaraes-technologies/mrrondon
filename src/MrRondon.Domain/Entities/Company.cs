using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MrRondon.Infra.CrossCutting.Message;

namespace MrRondon.Domain.Entities
{
    public class Company
    {
        [Key]
        public Guid CompanyId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        [MinLength(4, ErrorMessage = "Mínimo {0} caracteres")]
        [MaxLength(60, ErrorMessage = "Máximo {0} caracteres")]
        public string Name { get; set; }

        [Display(Name = "Nome Fantasia")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        [MinLength(4, ErrorMessage = "Mínimo {0} caracteres")]
        [MaxLength(60, ErrorMessage = "Máximo {0} caracteres")]
        public string FancyName { get; set; }

        [Display(Name = "CNPJ")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        [MinLength(14, ErrorMessage = "Mínimo {0} caracteres")]
        [MaxLength(18, ErrorMessage = "Máximo {0} caracteres")]
        public string Cnpj { get; set; }

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
        
        [Display(Name = "Segmento")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public int SegmentId { get; set; }
        public Category Segment { get; set; }

        public ICollection<Contact> Contacts { get; set; }
    }
}