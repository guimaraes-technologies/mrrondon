using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MrRondon.Infra.CrossCutting.Message;

namespace MrRondon.Domain.Entities
{
    public class SubCategory
    {
        [Key]
        public int SubCategoryId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        [MinLength(4, ErrorMessage = "Mínimo {0} caracteres")]
        [MaxLength(30, ErrorMessage = "Máximo {0} caracteres")]
        public string Name { get; set; }

        [Column(TypeName = "image")]
        [Display(Name = "Imagem")]
        public byte[] Image { get; set; }

        [Display(Name = "Exibir no Aplicativo?")]
        public bool ShowOnApp { get; set; } = true;

        [Display(Name = "Categoria")]
        public int? CategoryId { get; set; }
        public SubCategory Category { get; set; }

        public ICollection<SubCategory> Categories { get; set; }
        public ICollection<Company> Companies { get; set; }

        public string GetImage
        {
            get
            {
                if (Image != null && Image.Length > 0) return $"data:image/PNG;base64,{Convert.ToBase64String(Image)}";

                return "~/Content/Images/without_image.jpg";
            }
        }

        public void SetImage(byte[] imageBytes)
        {
            Image = imageBytes;
        }
    }
}