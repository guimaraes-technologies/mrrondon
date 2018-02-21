using System.Web;
using MrRondon.Domain.Entities;

namespace MrRondon.Presentation.Mvc.ViewModels
{
    public class CrudHistoricalSightVm
    {
        public HistoricalSight HistoricalSight { get; set; }

        public HttpPostedFileBase CoverFile { get; set; }
        public HttpPostedFileBase LogoFile { get; set; }
    }
}