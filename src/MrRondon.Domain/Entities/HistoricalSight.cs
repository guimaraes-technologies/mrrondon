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
        [MinLength(4, ErrorMessage = "Mínimo {1} caracteres")]
        [MaxLength(30, ErrorMessage = "Máximo {1} caracteres")]
        public string Name { get; set; }

        [Display(Name = "História")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        [MaxLength(800, ErrorMessage = "Máximo {1} caracteres")]
        public string SightHistory { get; set; }

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
    }
}