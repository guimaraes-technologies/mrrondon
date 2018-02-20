using System;
using System.Web.Mvc;

namespace MrRondon.Infra.CrossCutting.Helper.Buttons
{
    public class ButtonsCompany : ButtonsBase
    {
        public string ToPagination(Guid id)
        {
            return $"{Edit(id)} {Details(id)}";
        }

        private MvcHtmlString Details(Guid id)
        {
            try
            {
                var link = new TagBuilder("a");
                var iconEdit = new TagBuilder("i");
                iconEdit.MergeAttribute("class", IconDetails);
                link.MergeAttribute("href", Url.Action("Details", "Company", new { area = "Admin", id }));
                link.MergeAttribute("class", "ui tiny icon button");
                link.MergeAttribute("data-loading", "btn");
                link.MergeAttribute("title", "Detalhes");
                link.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(null));
                link.InnerHtml = iconEdit.ToString();

                return MvcHtmlString.Create(link.ToString());
            }
            catch { return MvcHtmlString.Empty; }
        }

        private MvcHtmlString Edit(Guid id)
        {
            try
            {
                var link = new TagBuilder("a");
                var iconEdit = new TagBuilder("i");
                iconEdit.MergeAttribute("class", IconEdit);
                link.MergeAttribute("href", Url.Action("Edit", "Company", new { area = "Admin", id }));
                link.MergeAttribute("class", "ui tiny icon button");
                link.MergeAttribute("data-loading", "btn");
                link.MergeAttribute("title", "Editar");
                link.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(null));
                link.InnerHtml = iconEdit.ToString();

                return MvcHtmlString.Create(link.ToString());
            }
            catch { return MvcHtmlString.Empty; }
        }

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