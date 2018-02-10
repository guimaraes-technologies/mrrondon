using System;
using System.ComponentModel.DataAnnotations;
using MrRondon.Infra.CrossCutting.Message;

namespace MrRondon.Domain.Entities
{
    public class Login
    {
        [Key]
        public Guid LoginId { get; set; } = Guid.NewGuid();

        [Display(Name = "Provedor")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public string LoginProvider { get; set; }

        [Display(Name = "Chave")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public string ProviderKey { get; set; }

        [Display(Name = "Usuário")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}