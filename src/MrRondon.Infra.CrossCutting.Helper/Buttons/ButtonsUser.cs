using System;
using System.Web.Mvc;

namespace MrRondon.Infra.CrossCutting.Helper.Buttons
{
    public class ButtonsUser : ButtonsBase
    {
        public string ToPagination(Guid id, bool isActive)
        {
            return $"{Edit(id)} {UpdateStatus(id, isActive)} {Details(id)}";
        }

        private MvcHtmlString Details(Guid id)
        {
            try
            {
                var link = new TagBuilder("a");
                var iconEdit = new TagBuilder("i");
                iconEdit.MergeAttribute("class", IconDetails);
                link.MergeAttribute("href", Url.Action("Details", "User", new { area = "Admin", id }));
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
                link.MergeAttribute("href", Url.Action("Edit", "User", new { area = "Admin", id }));
                link.MergeAttribute("class", "ui tiny icon button");
                link.MergeAttribute("data-loading", "btn");
                link.MergeAttribute("title", "Editar");
                link.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(null));
                link.InnerHtml = iconEdit.ToString();

                return MvcHtmlString.Create(link.ToString());
            }
            catch { return MvcHtmlString.Empty; }
        }

        private MvcHtmlString Reset(Guid id)
        {
            try
            {
                var link = new TagBuilder("a");
                var iconEdit = new TagBuilder("i");
                iconEdit.MergeAttribute("class", IconEdit);
                link.MergeAttribute("href", Url.Action("ResetPassword", "User", new { area = "Admin", id }));
                link.MergeAttribute("class", "ui tiny icon button");
                link.MergeAttribute("data-loading", "btn");
                link.MergeAttribute("title", "Resetar Senha");
                link.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(null));
                link.InnerHtml = iconEdit.ToString();

                return MvcHtmlString.Create(link.ToString());
            }
            catch { return MvcHtmlString.Empty; }
        }

        private MvcHtmlString UpdateStatus(Guid id, bool ativo)
        {
            try
            {
                var link = new TagBuilder("a");
                var icon = new TagBuilder("i");

                icon.MergeAttribute("class", ativo ? IconDisable : IconActive);
                link.MergeAttribute("href", Url.Action("UpdateStatus", "User", new { area = "Admin", id }));
                link.MergeAttribute("class", "ui tiny icon button");
                link.MergeAttribute("data-loading", "btn");
                link.MergeAttribute("title", ativo ? "Desativar" : "Ativar");
                link.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(null));
                link.InnerHtml = icon.ToString();

                return MvcHtmlString.Create(link.ToString());
            }
            catch { return MvcHtmlString.Empty; }
        }
    }
}