using System;
using System.Web.Mvc;

namespace MrRondon.Infra.CrossCutting.Helper.Buttons
{
    public class ButtonsCategory : ButtonsBase
    {
        public string ToPagination(int id)
        {
            return $"{Edit(id)} {Details(id)}";
        }

        public MvcHtmlString Image(byte[] array)
        {
            try
            {
                var link = new TagBuilder("img");
                link.MergeAttribute("class", "ui tiny icon");
                link.MergeAttribute("style", "height: 60px; width: 60px;");
                link.MergeAttribute("src", array != null && array.Length > 0 ? $"data:image/PNG;base64,{Convert.ToBase64String(array)}" : "/Content/Images/without_image.jpg");
                link.MergeAttribute("title", "Imagem");
                link.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(null));

                return MvcHtmlString.Create(link.ToString());
            }
            catch { return MvcHtmlString.Empty; }
        }

        private MvcHtmlString Details(int id)
        {
            try
            {
                var link = new TagBuilder("a");
                var iconEdit = new TagBuilder("i");
                iconEdit.MergeAttribute("class", IconDetails);
                link.MergeAttribute("href", Url.Action("Details", "Category", new { area = "Admin", id }));
                link.MergeAttribute("class", "ui small image");
                link.MergeAttribute("data-loading", "btn");
                link.MergeAttribute("title", "Detalhes");
                link.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(null));
                link.InnerHtml = iconEdit.ToString();

                return MvcHtmlString.Create(link.ToString());
            }
            catch { return MvcHtmlString.Empty; }
        }

        private MvcHtmlString Edit(int id)
        {
            try
            {
                var link = new TagBuilder("a");
                var iconEdit = new TagBuilder("i");
                iconEdit.MergeAttribute("class", IconEdit);
                link.MergeAttribute("href", Url.Action("Edit", "Category", new { area = "Admin", id }));
                link.MergeAttribute("class", "ui tiny icon button");
                link.MergeAttribute("data-loading", "btn");
                link.MergeAttribute("title", "Editar");
                link.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(null));
                link.InnerHtml = iconEdit.ToString();

                return MvcHtmlString.Create(link.ToString());
            }
            catch { return MvcHtmlString.Empty; }
        }
    }
}