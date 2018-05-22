using System;
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
        protected string IconReset => "sync icon";

        public MvcHtmlString Image(byte[] imageArray)
        {
            try
            {
                var link = new TagBuilder("img");
                link.MergeAttribute("class", "ui medium circular image");
                link.MergeAttribute("style", "height: 70px; width: 70px;border: #045e55 3px solid");
                link.MergeAttribute("src", imageArray != null && imageArray.Length > 0 ? $"data:image/PNG;base64,{Convert.ToBase64String(imageArray)}" : "/Content/Images/without_image.jpg");
                link.MergeAttribute("title", "Imagem");
                link.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(null));

                return MvcHtmlString.Create(link.ToString());
            }
            catch { return MvcHtmlString.Empty; }
        }
    }
}