using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MrRondon.Infra.CrossCutting.Message;

namespace MrRondon.Domain.Entities
{
    public class Category
    {
        [Key] public int CategoryId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        [MinLength(4, ErrorMessage = "Mínimo {0} caracteres")]
        [MaxLength(30, ErrorMessage = "Máximo {0} caracteres")]
        public string Name { get; set; }

        [Column(TypeName = "image")]
        [Display(Name = "Imagem")]
        //[Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public byte[] Image { get; set; }

        [Display(Name = "Sub Categoria")] public int? SubCategoryId { get; set; }
        public Category SubCategory { get; set; }

        public void SetImage(byte[] imageBytes)
        {
            Image = imageBytes;
        }
    }
}