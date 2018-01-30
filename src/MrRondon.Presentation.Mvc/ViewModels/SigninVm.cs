using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MrRondon.Infra.CrossCutting.Message;

namespace MrRondon.Presentation.Mvc.ViewModels
{
    public class SigninVm
    {
        [DisplayName("Login")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Error))]
        public string UserName { get; set; }

        [DisplayName("Senha")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Error))]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}