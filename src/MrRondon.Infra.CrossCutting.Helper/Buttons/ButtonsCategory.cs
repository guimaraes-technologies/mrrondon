using System;
using System.Web.Mvc;

namespace MrRondon.Infra.CrossCutting.Helper.Buttons
{
    public class ButtonsCategory : ButtonsBase
    {
        public string ToPagination(int id, bool showOnApp)
        {
            return $"{Edit(id)} {ShowOnApp(id, showOnApp)} {Details(id)}";
        }

        private MvcHtmlString Details(int id)
        {
            try
            {
                var link = new TagBuilder("a");
                var iconEdit = new TagBuilder("i");
                iconEdit.MergeAttribute("class", IconDetails);
                link.MergeAttribute("href", Url.Action("Details", "Category", new { area = "Admin", id }));
                link.MergeAttribute("class", "ui tiny icon button");
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

        private MvcHtmlString ShowOnApp(int id, bool showOnApp)
        {
            try
            {
                var link = new TagBuilder("a");
                var icon = new TagBuilder("i");
                icon.MergeAttribute("class", showOnApp ? IconActive : IconDisable);
                icon.MergeAttribute("class", IconActive);
                link.MergeAttribute("href", Url.Action("ShowOnApp", "Category", new { area = "Admin", id }));
                link.MergeAttribute("class", "ui tiny icon button");
                link.MergeAttribute("data-loading", "btn");
                link.MergeAttribute("title", $"{(showOnApp ? "Mostrar no app" : "Não mostrar no app")}");
                link.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(null));
                link.InnerHtml = icon.ToString();

                return MvcHtmlString.Create(link.ToString());
            }
            catch { return MvcHtmlString.Empty; }
        }
    }
}