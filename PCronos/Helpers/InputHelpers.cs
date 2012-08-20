using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace PCronos.Helpers
{
    public static class InputHelpers
    {
        public static MvcHtmlString TypeaheadFor<TModel, TValue>(
               this HtmlHelper<TModel> htmlHelper,
               Expression<Func<TModel, TValue>> expression,
               IEnumerable<string> source,
               int items = 8)
        {
             var builder = new TagBuilder("div");
             
            if (expression == null)
                throw new ArgumentNullException("expression");

            if (source == null)
                throw new ArgumentNullException("source");

            var jsonString = new JavaScriptSerializer().Serialize(source);

            
            return htmlHelper.TextBoxFor(
                expression,
                new
                {
                    autocomplete = "off",
                    data_provide = "typeahead",
                    data_items = items,
                    data_source = jsonString
                }
            );
        }
        
        public static MvcHtmlString FormLineEditorFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string templateName = null, string labelText = null, string customHelpText = null, object htmlAttributes = null)
        {
            var editor = helper.EditorFor(expression, templateName, htmlAttributes);
            if (htmlAttributes == null)
                htmlAttributes = new { @class = "input-xlarge date" };
            else
            {
                dynamic htmlAttributesd = htmlAttributes as dynamic;
                if (htmlAttributesd.@class == null)
                    htmlAttributesd.@class = "input-xlarge date";
            }
            return FormLine(
                helper.LabelFor(expression, labelText, new { @class = "control-label" }).ToString(),
                helper.EditorFor(expression, templateName, htmlAttributes).ToString(),
                helper.HelpTextFor(expression, customHelpText).ToString(),
                helper.ValidationMessageFor(expression));
        }
        public static MvcHtmlString HelpTextFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string customText = null)
        {
            var helpertag = new TagBuilder("span");
            helpertag.AddCssClass("help-inline");
            
            return MvcHtmlString.Create(helpertag.ToString());
            // Can do all sorts of things here -- eg: reflect over attributes and add hints, etc...
        }  
        private static MvcHtmlString FormLine(string labelContent, string fieldContent, string helperContent, object htmlAttributes = null)
        {
            
            var container = new TagBuilder("div");
            if (htmlAttributes != null)
                container.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            container.AddCssClass("control-group");
            container.InnerHtml += labelContent;
            container.InnerHtml += new TagBuilder("div") { Attributes = { { "class", "controls" } }, InnerHtml = fieldContent };
            container.InnerHtml += helperContent;
            return MvcHtmlString.Create(container.ToString());
        }
    }
}