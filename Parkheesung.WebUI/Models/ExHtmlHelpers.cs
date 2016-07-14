using System;
using System.Text;
using System.Web.Mvc;

namespace Parkheesung.WebUI.Models
{
    public static class ExHtmlHelpers
    {
        public static MvcHtmlString TagWrite(this HtmlHelper helper, string tags)
        {
            return MvcHtmlString.Create(tags);
        }

        public static MvcHtmlString EnterWrite(this HtmlHelper helper, string tags)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(tags);
            builder.Replace(Environment.NewLine, "<br />");
            return MvcHtmlString.Create(builder.ToString());
        }
    }
}