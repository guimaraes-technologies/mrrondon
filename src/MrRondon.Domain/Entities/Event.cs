using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MrRondon.Infra.CrossCutting.Message;

namespace MrRondon.Domain.Entities
{
    public class Event
    {
        [Key]
        public Guid EventId { get; set; } = Guid.NewGuid();

        [Display(Name = "Nome")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        [MinLength(4, ErrorMessage = "Mínimo {1} caracteres")]
        [MaxLength(50, ErrorMessage = "Máximo {1} caracteres")]
        public string Name { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        [MaxLength(200, ErrorMessage = "Máximo {1} caracteres")]
        public string Description { get; set; }

        [Display(Name = "Valor")]
        public decimal? Value { get; set; }

        [Display(Name = "Início")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Fim")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Imagem da Logo")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public byte[] Logo { get; set; }

        [Display(Name = "Imagem da Capa")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public byte[] Cover { get; set; }

        [Display(Name = "Organizador")]
        public Guid? OrganizerId { get; set; }
        public Company Organizer { get; set; }

        public bool SameAddressAsOganizer { get; set; } = true;

        [Display(Name = "Endereço")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public Guid AddressId { get; set; }
        public Address Address { get; set; }

        public string GetLogo
        {
            get
            {
                if (Logo != null && Logo.Length > 0)
                {
                    return $"data:image/PNG;base64,{Convert.ToBase64String(Logo)}";
                }

                return "~/Content/Images/without_image.jpg";
            }
        }

        public string GetCover
        {
            get
            {
                if (Cover != null && Cover.Length > 0)
                {
                    return $"data:image/PNG;base64,{Convert.ToBase64String(Cover)}";
                }

                return "~/Content/Images/without_image.jpg";
            }
        }

        public ICollection<Contact> Contacts { get; set; }
    }
}