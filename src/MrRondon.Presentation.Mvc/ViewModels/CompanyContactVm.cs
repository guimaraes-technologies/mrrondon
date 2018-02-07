using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MrRondon.Domain.Entities;

namespace MrRondon.Presentation.Mvc.ViewModels
{
    public class CompanyContactVm
    {
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
        [MaxLength(18, ErrorMessage = "Máximo {0} caracteres")]
        public string Cnpj { get; set; }

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

        [Display(Name = "Segmento")]
        [Required(ErrorMessage = "Campo {0} obrigatório")]
        public int SegmentId { get; set; }
        public Category Segment { get; set; }

        public Guid ContactId { get; set; }
        public string Description { get; set; }
        public ContactType ContactType { get; set; }

        public Guid? UserId { get; set; }
        public User User { get; set; }

        public List<Contact> Contacts { get; set; }
    }
}