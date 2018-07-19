using MrRondon.Infra.CrossCutting.Message;
using System.ComponentModel.DataAnnotations;

namespace MrRondon.Presentation.Mvc.ViewModels
{
    public class ForgetPasswordVm
    {
        [MaxLength(14, ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "MaxLength")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public string UserName { get; set; }
    }
}