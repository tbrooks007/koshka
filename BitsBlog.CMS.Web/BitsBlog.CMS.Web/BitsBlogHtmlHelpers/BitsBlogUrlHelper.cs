using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace BitsBlog.CMS.Web.BitsBlogHtmlHelpers
{
    public static class BitsBlogUrlHelper
    {
        /// <summary>
        /// This method extension method returns the corresponding page header html for the current route.
        /// </summary>
        /// <param name="urlHelper">this UrlHelper</param>
        /// <returns>HtmlString</returns>
        public static MvcHtmlString CurrentPageHeader(this UrlHelper urlHelper)
        {
            //TODO RFACTOR: MAY USE TAG BUILDER?
            StringBuilder sbHeader = new StringBuilder();

            var routeValueDictionary = urlHelper.RequestContext.RouteData.Values;
            string currentRoute = string.Format("{0}/{1}", routeValueDictionary["controller"].ToString(), routeValueDictionary["action"].ToString());

            if (currentRoute.ToLower().Equals("dashboard/index"))
            {
                sbHeader.Append("<div id=\"page-header\"><h2 id=\"dashboard-header\">Dashboard</h2></div>");
            }
            else if (currentRoute.ToLower().Equals("useraccount/createuser"))
            {
                sbHeader.Append("<div id=\"page-header\"><h2 id=\"manage-users-header\">Manage Users</h2><h2 id=\"create-user-header\">Create User</h2></div>");
            }
            else if (currentRoute.ToLower().Equals("useraccount/updateuser"))
            {
                sbHeader.Append("<div id=\"page-header\"><h2 id=\"manage-users-header\">Manage Users</h2><h2 id=\"edit-user-profile-header\">Edit Profile</h2></div>");
            }
            else if (currentRoute.ToLower().Equals("useraccount/changepassword"))
            {
                sbHeader.Append("<div id=\"page-header\"><h2 id=\"manage-users-header\">Manage Users</h2><h2 id=\"change-password-header\">Change Password</h2></div>");
            }
            else if (currentRoute.ToLower().Equals("blogsettings/update"))
            {
                sbHeader.Append("<div id=\"page-header\"><h2 id=\"manage-documents-header\">Manage Documents</h2><h2 id=\"update-blog-settings-header\">Edit Blog Settings</h2></div>");
            }
            else if (currentRoute.ToLower().Equals("blogsettings/create"))
            {
                sbHeader.Append("<div id=\"page-header\"><h2 id=\"manage-documents-header\">Manage Documents</h2><h2 id=\"create-blog-settings-header\">Create Blog</h2></div>");
            }
            else if (currentRoute.ToLower().Equals("systemsettings/update"))
            {
                sbHeader.Append("<div id=\"page-header\"><h2 id=\"system-settings-header\">System Settings</h2><h2 id=\"manage-system-settings-header\">Manage System Settings</h2></div>");
            }
            else if (currentRoute.ToLower().Equals("blogpost/update"))
            {
                sbHeader.Append("<div id=\"page-header\"><h2 id=\"manage-blog-post-header\">Manage Blogs</h2><h2 id=\"edit-post-header\">Edit Post</h2></div>");
            }
            else if (currentRoute.ToLower().Equals("blogpost/create"))
            {
                sbHeader.Append("<div id=\"page-header\"><h2 id=\"manage-blog-post-header\">Manage Blogs</h2><h2 id=\"create-post-header\">Create Post</h2></div>");
            }
            else if (currentRoute.ToLower().Equals("blogpostlibrary/index"))
            {
                sbHeader.Append("<div id=\"page-header\"><h2 id=\"manage-blog-post-header\">Manage Blogs</h2><h2 id=\"create-post-header\">Create Post</h2></div>");
            }
            else
            {
                sbHeader.Append(string.Empty);
            }

            return new MvcHtmlString(sbHeader.ToString());
        }

        public static MvcHtmlString GetAuthorDisplayName(this HtmlHelper htmlHelper, BitsBlog.Core.Structs.Author authorModel, string classAttributeValue)
        {
            StringBuilder sb = new StringBuilder();

            var builder = new TagBuilder("label");
            builder.AddCssClass(classAttributeValue);

            if (!string.IsNullOrWhiteSpace(authorModel.DisplayName))
            {
                builder.InnerHtml = authorModel.DisplayName;
            }
            else
            {
                sb.Append(authorModel.FirstName);
                sb.Append("&nbsp;");

                if (!string.IsNullOrWhiteSpace(authorModel.MiddleName))
                {
                    sb.Append(authorModel.MiddleName);
                    sb.Append("&nbsp;");
                }
                sb.Append(authorModel.LastName);

                builder.InnerHtml = sb.ToString();
            }

            return new MvcHtmlString(builder.ToString(TagRenderMode.Normal));
        }
    }
}