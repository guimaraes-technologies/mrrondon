using System;
using System.ComponentModel.DataAnnotations;

namespace MrRondon.Infra.Security.Entities
{
    public class Login
    {
        [Key]
        public Guid LoginId { get; set; }

        [Display(Name = "Provedor")]
        [Required(ErrorMessage = "Campo {0} obrigatório")]
        public string LoginProvider { get; set; }

        [Display(Name = "Chave")]
        [Required(ErrorMessage = "Campo {0} obrigatório")]
        public string ProviderKey { get; set; }

        [Display(Name = "Usuário")]
        [Required(ErrorMessage = "Campo {0} obrigatório")]
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}