using System.Web;
using System.Web.Mvc;

namespace MrRondon.Infra.CrossCutting.Helper.Buttons
{
    public abstract class ButtonsBase
    {
        protected UrlHelper Url => new UrlHelper(HttpContext.Current.Request.RequestContext);
        protected string IconAdd => "add-circle icon";
        protected string IconEdit => "edit icon";
        protected string IconDelete => "trash icon";
        public static string IconRemove => "remove icon";
        protected string IconSearch => "search icon";
        protected string IconActive => "checkmark icon";
        protected string IconDisable => "ban icon";
        protected string IconDetails => "list icon";
    }
}