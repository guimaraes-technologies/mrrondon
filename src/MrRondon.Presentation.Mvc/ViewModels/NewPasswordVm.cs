using System.ComponentModel.DataAnnotations;
using MrRondon.Infra.CrossCutting.Helper;
using MrRondon.Infra.CrossCutting.Message;

namespace MrRondon.Presentation.Mvc.ViewModels
{
    public class NewPasswordVm
    {
        [Display(Name = "Nova Senha")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        [RegularExpression(RegularExpressions.LetterAndNumber, ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "PasswordRequirements")]
        public string NewPassword { get; set; }
        
        [Display(Name = "Confirmar Senha")]
        [Compare(nameof(NewPassword), ErrorMessage = "Senhas não correspodem")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        [RegularExpression(RegularExpressions.LetterAndNumber, ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "PasswordRequirements")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}