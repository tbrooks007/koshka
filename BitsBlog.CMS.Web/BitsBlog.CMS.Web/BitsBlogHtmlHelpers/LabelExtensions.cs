namespace System.Web.Mvc.Html
{
    using System;
    using Collections.Generic;
    using Linq;
    using Linq.Expressions;
    using Mvc;

    public static class LabelExtensions
    {
        /// <summary>
        /// Overloads the LabelFor html helper so that we can add html attributes.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            return html.LabelFor(expression, null, htmlAttributes);
        }

        /// <summary>
        /// Overloads the LabelFor html helper so that we can add html attributes as well as the lable text.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText, object htmlAttributes)
        {
            return html.LabelHelper(
                ModelMetadata.FromLambdaExpression(expression, html.ViewData),
                ExpressionHelper.GetExpressionText(expression),
                HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes),
                labelText);
        }

        private static MvcHtmlString LabelHelper(this HtmlHelper html, ModelMetadata metadata, string htmlFieldName, IDictionary<string, object> htmlAttributes, string labelText = null)
        {
            var str = labelText
                ?? (metadata.DisplayName
                ?? (metadata.PropertyName
                ?? htmlFieldName.Split(new[] { '.' }).Last()));

            if (string.IsNullOrEmpty(str))
                return MvcHtmlString.Empty;

            var tagBuilder = new TagBuilder("label");
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.Attributes.Add("for", TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName)));
            tagBuilder.SetInnerText(str);

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }

        //private static MvcHtmlString ToMvcHtmlString(this TagBuilder tagBuilder, TagRenderMode renderMode)
        //{
        //    return new MvcHtmlString(tagBuilder.ToString(renderMode));
        //}
    }
}