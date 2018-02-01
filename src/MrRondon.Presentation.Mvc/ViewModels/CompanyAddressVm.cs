using System.ComponentModel.DataAnnotations;
using MrRondon.Domain.Entities;

namespace MrRondon.Presentation.Mvc.ViewModels
{
    public class CompanyAddressVm
    {
        public Company Company { get; set; }

        [Display(Name = "Categoria")]
        public int CategoryId { get; set; }

        [Display(Name = "Sub Categoria")]
        public int? SubCategoryId { get; set; }
    }
}