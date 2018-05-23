using System.ComponentModel.DataAnnotations;
using MrRondon.Infra.CrossCutting.Helper;
using MrRondon.Infra.CrossCutting.Message;

namespace MrRondon.Presentation.Mvc.ViewModels
{
    public class ChangePasswordVm
    {
        [Display(Name = "Senha antiga")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Display(Name = "Nova Senha")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        [RegularExpression(RegularExpressions.Password, ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "WrongUserNameOrPassword")]
        public string NewPassword { get; set; }

        [Display(Name = "Confirmar Senha")]
        [Compare("NovaSenha", ErrorMessage = "Senhas não correspodem")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        [RegularExpression(RegularExpressions.Password, ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "WrongUserNameOrPassword")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}