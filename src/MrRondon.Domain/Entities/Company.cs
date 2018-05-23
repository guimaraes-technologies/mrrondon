using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MrRondon.Infra.CrossCutting.Message;
using Newtonsoft.Json;

namespace MrRondon.Domain.Entities
{
    public class Company
    {
        [Key]
        public Guid CompanyId { get; set; } = Guid.NewGuid();

        [Display(Name = "Nome")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        [MinLength(4, ErrorMessage = "Mínimo {1} caracteres")]
        [MaxLength(60, ErrorMessage = "Máximo {1} caracteres")]
        public string Name { get; set; }

        [Display(Name = "Nome Fantasia")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        [MinLength(4, ErrorMessage = "Mínimo {1} caracteres")]
        [MaxLength(60, ErrorMessage = "Máximo {1} caracteres")]
        public string FancyName { get; set; }

        [Display(Name = "CNPJ")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        [MaxLength(18, ErrorMessage = "Máximo {1} caracteres")]
        public string Cnpj { get; set; }

        [Display(Name = "Imagem da Logo")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public byte[] Logo { get; set; }

        [Display(Name = "Imagem da Capa")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public byte[] Cover { get; set; }

        [Display(Name = "Endereço")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public Guid AddressId { get; set; }

        public Address Address { get; set; }

        [Display(Name = "Segmento")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }

        public ICollection<Contact> Contacts { get; set; }

        [JsonIgnore]
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

        [JsonIgnore]
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

        [JsonIgnore]
        public string GetSegment => SubCategory.Category == null ? SubCategory.Name : $"{SubCategory.Category.Name} - {SubCategory.Name}";
    }
}