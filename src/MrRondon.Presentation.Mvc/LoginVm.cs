using System.ComponentModel.DataAnnotations;

namespace MrRondon.Presentation.Mvc
{
    public class LoginVm
    {
        [Display(Name = "Login")]
        [Required(ErrorMessage = "Campo {0} é obrigatório")]
        public string UserName { get; set; }

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "Campo {0} é obrigatório")]
        public string Password { get; set; }
        public string RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}