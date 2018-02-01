using System.ComponentModel.DataAnnotations;
using MrRondon.Infra.CrossCutting.Message;

namespace MrRondon.Presentation.Mvc.ViewModels
{
    public class RecoveryPasswordVm
    {
        [MaxLength(14, ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "MaxLength")]
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Required")]
        public string UserName { get; set; }
    }
}