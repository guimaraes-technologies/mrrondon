using System;
using System.Web.Mvc;

namespace MrRondon.Infra.CrossCutting.Helper.Buttons
{
    public class ButtonsEvent : ButtonsBase
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
                link.MergeAttribute("href", Url.Action("Details", "Event", new { area = "Admin", id }));
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
                link.MergeAttribute("href", Url.Action("Edit", "Event", new { area = "Admin", id }));
                link.MergeAttribute("class", "ui tiny icon button");
                link.MergeAttribute("data-loading", "btn");
                link.MergeAttribute("title", "Editar");
                link.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(null));
                link.InnerHtml = $"{iconEdit}";

                return MvcHtmlString.Create(link.ToString());
            }
            catch { return MvcHtmlString.Empty; }
        }
    }
}