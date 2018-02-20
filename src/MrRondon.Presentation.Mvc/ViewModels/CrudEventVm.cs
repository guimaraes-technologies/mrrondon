using System.Web;
using MrRondon.Domain.Entities;

namespace MrRondon.Presentation.Mvc.ViewModels
{
    public class CrudEventVm
    {
        public Event Event { get; set; }
        public AddressForEventVm Address { get; set; }

        public HttpPostedFileBase CoverFile { get; set; }
        public HttpPostedFileBase LogoFile { get; set; }
    }
}