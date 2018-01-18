﻿using System.ComponentModel.DataAnnotations;

namespace MrRondon.Domain.Entities
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Campo {0} obrigatório")]
        [MinLength(4, ErrorMessage = "Mínimo {0} caracteres")]
        [MaxLength(20, ErrorMessage = "Máximo {0} caracteres")]
        public string Name { get; set; }

        [Display(Name = "Sub Categoria")]
        public int? SubCategoryId { get; set; }
        public Category SubCategory { get; set; }
    }
}