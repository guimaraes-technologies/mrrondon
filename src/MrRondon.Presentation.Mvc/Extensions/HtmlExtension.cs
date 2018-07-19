using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using MrRondon.Infra.CrossCutting.Helper;
using MrRondon.Infra.CrossCutting.Helper.Buttons;
using MrRondon.Presentation.Mvc.ViewModels;
using Newtonsoft.Json;

namespace MrRondon.Presentation.Mvc.Extensions
{
    public static class HtmlExtension
    {
        public static MvcHtmlString Back(this HtmlHelper html, string textoLink = "Voltar")
        {
            return new MvcHtmlString($"<a href=\"javascript:history.go(-1);\">{textoLink}</a>");
        }

        public static MvcHtmlString SemanticTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, dynamic htmlAttributes)
        {
            var fullBindingName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var validations = html.GetUnobtrusiveValidationAttributes(metadata.PropertyName, metadata);
            var tag = new TagBuilder("input");

            InsertValidateAttribute(tag, validations);

            if (htmlAttributes != null) tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            tag.Attributes.Add("type", "text");
            tag.Attributes.Add("id", TagBuilder.CreateSanitizedId(fullBindingName));
            tag.Attributes.Add("name", fullBindingName);
            tag.Attributes.Add("value", metadata.Model?.ToString() ?? string.Empty);

            return new MvcHtmlString(tag.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString SemanticNumericBoxFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, dynamic htmlAttributes)
        {
            var fullBindingName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var validations = html.GetUnobtrusiveValidationAttributes(metadata.PropertyName, metadata);
            var tag = new TagBuilder("input");

            InsertValidateAttribute(tag, validations);

            if (htmlAttributes != null) tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            tag.Attributes.Add("type", "number");
            tag.Attributes.Add("id", TagBuilder.CreateSanitizedId(fullBindingName));
            tag.Attributes.Add("name", fullBindingName);
            tag.Attributes.Add("value", metadata.Model?.ToString() ?? string.Empty);

            return new MvcHtmlString(tag.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString SemanticPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var fullBindingName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var validations = html.GetUnobtrusiveValidationAttributes(metadata.PropertyName, metadata);
            var tag = new TagBuilder("input");

            InsertValidateAttribute(tag, validations);

            if (htmlAttributes != null) tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            tag.Attributes.Add("type", "password");
            tag.Attributes.Add("id", TagBuilder.CreateSanitizedId(fullBindingName));
            tag.Attributes.Add("name", fullBindingName);
            tag.Attributes.Add("value", metadata.Model?.ToString() ?? string.Empty);

            return new MvcHtmlString(tag.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString SemanticTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, dynamic htmlAttributes = null)
        {
            var fullBindingName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var validations = html.GetUnobtrusiveValidationAttributes(metadata.PropertyName, metadata);
            var tag = new TagBuilder("textarea");

            InsertValidateAttribute(tag, validations);

            if (htmlAttributes != null) tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            tag.Attributes.Add("id", TagBuilder.CreateSanitizedId(fullBindingName));
            tag.Attributes.Add("name", fullBindingName);
            tag.SetInnerText(metadata.Model?.ToString() ?? string.Empty);

            return new MvcHtmlString(tag.ToString());
        }

        public static MvcHtmlString SemanticTextEmailFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, dynamic htmlAttributes)
        {
            var fullBindingName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var validations = html.GetUnobtrusiveValidationAttributes(metadata.PropertyName, metadata);
            var tag = new TagBuilder("input");

            InsertValidateAttribute(tag, validations);

            if (htmlAttributes != null) tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            tag.Attributes.Add("type", "email");
            tag.Attributes.Add("id", TagBuilder.CreateSanitizedId(fullBindingName));
            tag.Attributes.Add("name", fullBindingName);
            tag.Attributes.Add("value", metadata.Model?.ToString() ?? string.Empty);

            return new MvcHtmlString(tag.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString SemanticTextCpfFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, dynamic htmlAttributes)
        {
            var fullBindingName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var validations = html.GetUnobtrusiveValidationAttributes(metadata.PropertyName, metadata);
            var tag = new TagBuilder("input");

            InsertValidateAttribute(tag, validations);

            if (htmlAttributes != null) tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            tag.Attributes.Add("type", "text");
            tag.Attributes.Add("id", TagBuilder.CreateSanitizedId(fullBindingName));
            tag.Attributes.Add("name", fullBindingName);
            tag.Attributes.Add("data-gtmask", "cpf");
            //tag.Attributes.Add("placeholder", "Ex.: 123.456.789-00");
            tag.Attributes.Add("value", metadata.Model?.ToString() ?? string.Empty);

            return new MvcHtmlString(tag.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString SemanticTextTelephoneFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, dynamic htmlAttributes)
        {
            var fullBindingName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var validations = html.GetUnobtrusiveValidationAttributes(metadata.PropertyName, metadata);
            var tag = new TagBuilder("input");

            InsertValidateAttribute(tag, validations);

            if (htmlAttributes != null) tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            tag.Attributes.Add("type", "text");
            tag.Attributes.Add("id", TagBuilder.CreateSanitizedId(fullBindingName));
            tag.Attributes.Add("name", fullBindingName);
            tag.Attributes.Add("data-gtmask", "telephone");
            tag.Attributes.Add("value", metadata.Model?.ToString() ?? string.Empty);

            return new MvcHtmlString(tag.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString SemanticTextCnpjFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, dynamic htmlAttributes)
        {
            var fullBindingName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var validations = html.GetUnobtrusiveValidationAttributes(metadata.PropertyName, metadata);
            var tag = new TagBuilder("input");

            InsertValidateAttribute(tag, validations);

            if (htmlAttributes != null) tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            tag.Attributes.Add("type", "text");
            tag.Attributes.Add("id", TagBuilder.CreateSanitizedId(fullBindingName));
            tag.Attributes.Add("name", fullBindingName);
            tag.Attributes.Add("data-gtmask", "cnpj");
            tag.Attributes.Add("value", metadata.Model?.ToString() ?? string.Empty);

            return new MvcHtmlString(tag.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString SemanticTextDateTimeFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, dynamic htmlAttributes)
        {
            var fullBindingName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var validations = html.GetUnobtrusiveValidationAttributes(metadata.PropertyName, metadata);
            var tag = new TagBuilder("input");

            InsertValidateAttribute(tag, validations);

            if (htmlAttributes != null) tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            tag.Attributes.Add("type", "text");
            tag.Attributes.Add("id", TagBuilder.CreateSanitizedId(fullBindingName));
            tag.Attributes.Add("name", fullBindingName);
            tag.Attributes.Add("data-gtmask", "datetime");
            tag.Attributes.Add("value", metadata.Model?.ToString() ?? string.Empty);

            return new MvcHtmlString(tag.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString SemanticTextZipCodeFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, dynamic htmlAttributes)
        {
            var fullBindingName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var validations = html.GetUnobtrusiveValidationAttributes(metadata.PropertyName, metadata);
            var tag = new TagBuilder("input");

            InsertValidateAttribute(tag, validations);

            if (htmlAttributes != null) tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            tag.Attributes.Add("type", "text");
            tag.Attributes.Add("id", TagBuilder.CreateSanitizedId(fullBindingName));
            tag.Attributes.Add("name", fullBindingName);
            tag.Attributes.Add("data-gtmask", "cep");
            tag.Attributes.Add("value", metadata.Model?.ToString() ?? string.Empty);

            return new MvcHtmlString(tag.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString SemanticTextDateFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, dynamic htmlAttributes)
        {
            var fullBindingName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var validations = html.GetUnobtrusiveValidationAttributes(metadata.PropertyName, metadata);
            var tag = new TagBuilder("input");

            InsertValidateAttribute(tag, validations);

            if (htmlAttributes != null) tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            tag.Attributes.Add("type", "text");
            tag.Attributes.Add("id", TagBuilder.CreateSanitizedId(fullBindingName));
            tag.Attributes.Add("name", fullBindingName);
            tag.Attributes.Add("data-gtmask", "datetime");

            var valorInput = metadata.Model?.ToString();
            if (string.IsNullOrWhiteSpace(valorInput)) tag.Attributes.Add("value", string.Empty);
            else tag.Attributes.Add("value", DateTime.Parse(valorInput) != DateTime.MinValue ? Convert.ToDateTime(valorInput).ToShortDateString() : string.Empty);

            return new MvcHtmlString(tag.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString SemanticEnumDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, string optionLabel = "Selecione...", object htmlAttributes = null)
        {
            var fullBindingName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var validations = html.GetUnobtrusiveValidationAttributes(metadata.PropertyName, metadata);
            var tag = new TagBuilder("select");

            InsertValidateAttribute(tag, validations);

            if (htmlAttributes != null) tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            if (tag.Attributes.ContainsKey("class"))
            {
                tag.Attributes["class"] = $"ui fluid search dropdown {tag.Attributes["class"]}";
            }
            else
            {
                tag.Attributes.Add("class", "ui fluid search dropdown");
            }

            tag.Attributes.Add("id", TagBuilder.CreateSanitizedId(fullBindingName));
            tag.Attributes.Add("name", fullBindingName);
            tag.InnerHtml += $"<option value=''>{optionLabel}</option>";

            foreach (Enum item in Enum.GetValues(Nullable.GetUnderlyingType(typeof(TProperty)) ?? typeof(TProperty)))
            {
                tag.InnerHtml += $"<option value='{item}' {(Equals(item, metadata.Model) ? "selected" : string.Empty)}>{EnumDescription.Get(item)}</option>";
            }

            return new MvcHtmlString(tag.ToString());
        }

        public static MvcHtmlString SemanticEnumDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes)
        {
            var fullBindingName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var validations = html.GetUnobtrusiveValidationAttributes(metadata.PropertyName, metadata);
            var tag = new TagBuilder("select");

            InsertValidateAttribute(tag, validations);

            if (htmlAttributes != null) tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            tag.Attributes.Add("id", TagBuilder.CreateSanitizedId(fullBindingName));
            tag.Attributes.Add("name", fullBindingName);
            tag.AddCssClass("ui search dropdown");

            tag.InnerHtml += $"<option value=''>{optionLabel}</option>";

            foreach (var item in selectList)
            {
                tag.InnerHtml += $"<option value='{item.Value}' {(item.Selected ? "selected" : string.Empty)}>{item.Text}</option>";
            }

            return new MvcHtmlString(tag.ToString());
        }

        public static MvcHtmlString SemanticDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel = "Selecione...", object htmlAttributes = null)
        {
            var fullBindingName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var validations = html.GetUnobtrusiveValidationAttributes(metadata.PropertyName, metadata);
            var tag = new TagBuilder("div");

            InsertValidateAttribute(tag, validations);

            if (htmlAttributes != null) tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            tag.Attributes.Add("id", TagBuilder.CreateSanitizedId(fullBindingName));
            tag.AddCssClass("ui selection dropdown");
            var selectListItems = selectList as SelectListItem[] ?? selectList.ToArray();
            tag.InnerHtml = $"<input name='{fullBindingName}' type='hidden' value='{selectListItems.FirstOrDefault(f => f.Selected)?.Value}'>";

            tag.InnerHtml += $"<div class='default text'>{optionLabel}</div>";
            tag.InnerHtml += "<div class='menu'>";

            foreach (var item in selectListItems)
            {
                tag.InnerHtml += $"<div class='item' data-value='{item.Value}' {(item.Selected ? "selected" : string.Empty)}>{item.Text}</div>";
            }

            tag.InnerHtml += "</div>";//close menu div
            return new MvcHtmlString(tag.ToString());
        }

        public static MvcHtmlString SemanticDropDownListMultipleFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, IList<SelectListItemVm> selectList = null, string optionLabel = "Selecione...", object htmlAttributes = null)
        {
            var fullBindingName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var validations = html.GetUnobtrusiveValidationAttributes(metadata.PropertyName, metadata);
            var tag = new TagBuilder("div");
            InsertValidateAttribute(tag, validations);

            if (htmlAttributes != null) tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            tag.Attributes.Add("id", TagBuilder.CreateSanitizedId(fullBindingName));
            tag.AddCssClass("ui fluid multiple search selection dropdown");
            var value = selectList?
                .Where(s => s.Selected)
                .Aggregate(string.Empty, (current, item) => $"{(string.IsNullOrWhiteSpace(current) ? "" : $"{current},")}{item.Value}");

            tag.InnerHtml = $"<input type='hidden' name='{fullBindingName}' value='{value}'>";
            tag.InnerHtml += "<i class='dropdown icon'></i>";
            tag.InnerHtml += $"<div class='default text'>{optionLabel}</div>";
            tag.InnerHtml += "<div class='menu'>";

            if (selectList != null)
                foreach (var item in selectList)
                {
                    tag.InnerHtml += $"<div class='item' data-value='{item.Value}' {(item.Selected ? "selected" : string.Empty)}>{item.Text}</div>";
                }

            tag.InnerHtml += "</div>";
            var result = new MvcHtmlString(tag.ToString());
            return result;
        }

        public static MvcHtmlString SemanticDropDownListOptionalFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel = "Selecione...", object htmlAttributes = null)
        {
            var fullBindingName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var validations = html.GetUnobtrusiveValidationAttributes(metadata.PropertyName, metadata);
            var tag = new TagBuilder("div");

            InsertValidateAttribute(tag, validations);

            if (htmlAttributes != null) tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            var removeIcon = new TagBuilder("i");
            removeIcon.MergeAttribute("class", ButtonsBase.IconRemove);
            var dropIcon = new TagBuilder("i");
            dropIcon.MergeAttribute("class", "dropdown icon");

            tag.Attributes.Add("id", TagBuilder.CreateSanitizedId(fullBindingName));
            tag.AddCssClass("ui selection dropdown");
            var selectListItems = selectList as SelectListItem[] ?? selectList.ToArray();
            var selectedValue = selectListItems.FirstOrDefault(f => f.Selected)?.Value;
            tag.InnerHtml = $"<input name='{fullBindingName}' type='hidden' value='{selectedValue}'>";
            tag.InnerHtml += $"{removeIcon} {dropIcon}";

            tag.InnerHtml += $"<div class='default text'>{optionLabel}</div>";
            tag.InnerHtml += "<div class='menu'>";

            foreach (var item in selectListItems)
            {
                tag.InnerHtml += $"<div class='item' data-value='{item.Value}' {(item.Selected ? "selected" : string.Empty)}>{item.Text}</div>";
            }

            tag.InnerHtml += "</div>";//close menu div
            return new MvcHtmlString(tag.ToString());
        }

        public static MvcHtmlString SemanticDropDownBoolFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, string optionLabel, object htmlAttributes)
        {
            var fullBindingName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var validations = html.GetUnobtrusiveValidationAttributes(metadata.PropertyName, metadata);
            var tag = new TagBuilder("select");
            InsertValidateAttribute(tag, validations);
            if (htmlAttributes != null) tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            if (tag.Attributes.ContainsKey("class"))
            {
                tag.Attributes["class"] = $"ui fluid dropdown {tag.Attributes["class"]}";
            }
            else
            {
                tag.Attributes.Add("class", "ui fluid dropdown");
            }
            tag.Attributes.Add("id", TagBuilder.CreateSanitizedId(fullBindingName));
            tag.Attributes.Add("name", fullBindingName);
            tag.InnerHtml += $"<option value=''>{optionLabel}</option>";
            tag.InnerHtml += $"<option value='true'>Sim</option>";
            tag.InnerHtml += $"<option value='false'>Não</option>";
            return new MvcHtmlString(tag.ToString());
        }

        public static MvcHtmlString SemanticDropDownBoolFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, string optionLabel, object htmlAttributes, bool value)
        {
            var fullBindingName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var validations = html.GetUnobtrusiveValidationAttributes(metadata.PropertyName, metadata);
            var tag = new TagBuilder("select");
            InsertValidateAttribute(tag, validations);
            if (htmlAttributes != null) tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            if (tag.Attributes.ContainsKey("class"))
            {
                tag.Attributes["class"] = $"ui fluid dropdown {tag.Attributes["class"]}";
            }
            else
            {
                tag.Attributes.Add("class", "ui fluid dropdown");
            }
            tag.Attributes.Add("id", TagBuilder.CreateSanitizedId(fullBindingName));
            tag.Attributes.Add("name", fullBindingName);
            tag.InnerHtml += $"<option value=''>{optionLabel}</option>";
            if (value)
            {
                tag.InnerHtml += "<option value='true' selected>Sim</option>";
                tag.InnerHtml += "<option value='false'>Não</option>";
            }
            else
            {
                tag.InnerHtml += "<option value='true'>Sim</option>";
                tag.InnerHtml += "<option value='false' selected>Não</option>";
            }
            return new MvcHtmlString(tag.ToString());
        }

        public static MvcHtmlString SemanticRadioBoxFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, string value, bool ischecked = false)
        {
            var fullBindingName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            var tag = new TagBuilder("input");

            tag.Attributes.Add("type", "radio");
            tag.Attributes.Add("id", TagBuilder.CreateSanitizedId(fullBindingName));
            tag.Attributes.Add("name", fullBindingName);
            tag.Attributes.Add("value", !string.IsNullOrEmpty(value) ? value : string.Empty);

            if (ischecked) { tag.Attributes.Add("checked", "checked"); }

            return new MvcHtmlString(tag.ToString(TagRenderMode.SelfClosing));
        }

        private static void InsertValidateAttribute(TagBuilder tag, IDictionary<string, object> validations)
        {
            if (!validations.Any()) return;

            var rules = new List<dynamic>();

            foreach (var validation in validations)
            {
                switch (validation.Key)
                {
                    case "data-val-required":
                        rules.Add(new { type = "empty", prompt = validations["data-val-required"]?.ToString() ?? string.Empty });
                        break;
                    case "data-val-maxlength-max":
                        rules.Add(new { type = $"maxLength[{validations["data-val-maxlength-max"]}]", prompt = validations["data-val-maxlength"]?.ToString() ?? string.Empty });
                        tag.Attributes.Add("maxlength", validations["data-val-maxlength-max"].ToString());
                        break;
                    case "data-val-regex":
                        rules.Add(new { type = $"regExp[{validations["data-val-regex-pattern"]}]", prompt = validations["data-val-regex"]?.ToString() ?? string.Empty });
                        break;
                    case "data-val-email":
                        rules.Add(new { type = "email", prompt = validations["data-val-email"]?.ToString() ?? string.Empty });
                        break;
                    case "data-val-remote-url":
                        rules.Add(new { type = "remote", prompt = validations["data-val-remote"]?.ToString() ?? string.Empty });
                        tag.Attributes.Add("data-val-remote-url", validations["data-val-remote-url"].ToString());
                        break;
                    case "data-val-equalto":
                        rules.Add(new { type = $"match[{validations["data-val-equalto-other"].ToString().Replace("*.", "")}]", prompt = validations["data-val-equalto"]?.ToString() ?? string.Empty });
                        break;
                }
            }

            tag.Attributes.Add("data-rules", JsonConvert.SerializeObject(rules));
        }
    }
}