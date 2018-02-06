using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mime;

namespace MrRondon.Domain.Entities
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Campo {0} obrigatório")]
        [MinLength(4, ErrorMessage = "Mínimo {0} caracteres")]
        [MaxLength(30, ErrorMessage = "Máximo {0} caracteres")]
        public string Name { get; set; }

        [Column(TypeName = "image")]
        [Display(Name = "Imagem")]
        //[Required(ErrorMessage = "Campo {0} obrigatório")]
        public byte[] Image { get; set; }

        [Display(Name = "Sub Categoria")]
        public int? SubCategoryId { get; set; }
        public Category SubCategory { get; set; }

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