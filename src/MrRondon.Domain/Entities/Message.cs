using System;
using System.ComponentModel.DataAnnotations;

namespace MrRondon.Domain.Entities
{
    public class Message
    {
        [Key]
        public Guid MessageId { get; set; }

        [Display(Name = "Titulo")]
        [Required(ErrorMessage = "Campo {0} obrigatório")]
        [MinLength(4, ErrorMessage = "Mínimo {0} caracteres")]
        [MaxLength(50, ErrorMessage = "Máximo {0} caracteres")]
        public string Title { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Campo {0} obrigatório")]
        [MinLength(10, ErrorMessage = "Mínimo {0} caracteres")]
        [MaxLength(250, ErrorMessage = "Máximo {0} caracteres")]
        public string Description { get; set; }

        [Display(Name = "Celular")]
        [Required(ErrorMessage = "Campo {0} obrigatório")]
        [MinLength(13, ErrorMessage = "Mínimo {0} caracteres")]
        [MaxLength(14, ErrorMessage = "Máximo {0} caracteres")]
        public string CellPhone { get; set; }

        [Display(Name = "Telefone")]
        [Required(ErrorMessage = "Campo {0} obrigatório")]
        [MinLength(13, ErrorMessage = "Mínimo {0} caracteres")]
        [MaxLength(14, ErrorMessage = "Máximo {0} caracteres")]
        public string Telephone { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}