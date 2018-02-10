using System;
using System.ComponentModel.DataAnnotations;
using MrRondon.Infra.CrossCutting.Message;

namespace MrRondon.Domain.Entities
{
    public class Message
    {
        [Key]
        public Guid MessageId { get; set; } = Guid.NewGuid();

        [Display(Name = "Titulo")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        [MinLength(4, ErrorMessage = "Mínimo {0} caracteres")]
        [MaxLength(50, ErrorMessage = "Máximo {0} caracteres")]
        public string Title { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        [MinLength(10, ErrorMessage = "Mínimo {0} caracteres")]
        [MaxLength(250, ErrorMessage = "Máximo {0} caracteres")]
        public string Description { get; set; }

        [Display(Name = "Celular")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        [MinLength(14, ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "MinLength")]
        [MaxLength(15, ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "MaxLength")]
        public string CellPhone { get; set; }

        [Display(Name = "Telefone")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        [MinLength(13, ErrorMessage = "Mínimo {0} caracteres")]
        [MaxLength(14, ErrorMessage = "Máximo {0} caracteres")]
        public string Telephone { get; set; }

        [Display(Name = "Assunto")]
        [EnumDataType(typeof(MessageSubject), ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public MessageSubject Subject { get; set; }

        [EnumDataType(typeof(MessageStatus), ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public MessageStatus Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}