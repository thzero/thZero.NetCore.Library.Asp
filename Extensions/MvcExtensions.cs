/* ------------------------------------------------------------------------- *
thZero.NetCore.Library.Asp
Copyright (C) 2016-2022 thZero.com

<development [at] thzero [dot] com>

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

	http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
 * ------------------------------------------------------------------------- */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace thZero
{
    public static class MvcExtensions
    {
        #region Public Methods
        public static HtmlString ClientMarshall(this IHtmlHelper html, string name, IDictionary<string, object> htmlAttributes)
        {
            Enforce.AgainstNull(() => html);
            Enforce.AgainstNullOrEmpty(() => name);

            if (htmlAttributes == null)
                return new HtmlString(string.Empty);

            htmlAttributes.Add(Style, StyleDisplayNone);

            TagBuilder tagBuilder = new(TagDiv);
            tagBuilder.GenerateId(name, string.Empty);
            tagBuilder.MergeAttributes(htmlAttributes, true);

            StringBuilder result = new();
            using (var writer = new StringWriter())
            {
                tagBuilder.WriteTo(writer, HtmlEncoder.Default);
                result.Append(writer.ToString());
            }
            return new HtmlString(result.ToString());
        }

        #region Actions
        public static string ActionUrl(this IUrlHelper urlHelper, string actionName, string controllerName)
        {
            return ActionUrl(urlHelper, actionName, controllerName, true, null);
        }

        public static string ActionUrl(this IUrlHelper urlHelper, string actionName, string controllerName, object routeValues)
        {
            return ActionUrl(urlHelper, actionName, controllerName, true, routeValues);
        }

        public static string ActionUrl(this IUrlHelper urlHelper, string actionName, string controllerName, bool relative, object routeValues)
        {
            Enforce.AgainstNull(() => urlHelper);

            string url = string.Empty;
            string urlBase = string.Concat(urlHelper.ActionContext.HttpContext.Request.Scheme, SeparatorUri, urlHelper.ActionContext.HttpContext.Request.Host.Value, urlHelper.ActionContext.HttpContext.Request.PathBase.Value);
            string urlFull = string.Concat(urlHelper.ActionContext.HttpContext.Request.Scheme, SeparatorUri, urlHelper.ActionContext.HttpContext.Request.Host.Value, urlHelper.ActionContext.HttpContext.Request.Path.Value);

            if (string.IsNullOrEmpty(actionName))
            {
                if (relative)
                    return urlHelper.ActionContext.HttpContext.Request.Path.Value;

                url = urlFull;
            }
            else if (string.IsNullOrEmpty(controllerName))
            {
                Uri uri = new(string.Concat(urlBase, urlHelper.Action(actionName)));
                if (relative)
                    return uri.AbsolutePath;

                url = uri.AbsoluteUri;
            }

            if (string.IsNullOrEmpty(url))
            {
                Uri uri = new(string.Concat(urlBase, urlHelper.Action(actionName, controllerName, routeValues)));
                url = (relative ? uri.AbsolutePath : uri.AbsoluteUri);
            }

            return url;
        }

        public static string ActionUrlWithId(this IUrlHelper urlHelper, string controllerName)
        {
            return ActionUrlWithId(urlHelper, controllerName, -1);
        }

        public static string ActionUrlWithId(this IUrlHelper urlHelper, string controllerName, int id)
        {
            return ActionUrl(urlHelper, Constants.Controller.Action.Index, controllerName, new { id });
        }

        public static string ActionUrlWithId(this IUrlHelper urlHelper, string actionName, string controllerName, int id)
        {
            return ActionUrl(urlHelper, actionName, controllerName, new { id });
        }

        public static string ActionUrlWithId(this IUrlHelper urlHelper, string controllerName, Guid? id)
        {
            id = (id.HasValue ? id : Guid.Empty);
            return ActionUrl(urlHelper, Constants.Controller.Action.Index, controllerName, new { id });
        }

        public static string ActionUrlWithId(this IUrlHelper urlHelper, string actionName, string controllerName, Guid? id)
        {
            id = (id.HasValue ? id : Guid.Empty);
            return ActionUrl(urlHelper, actionName, controllerName, new { id });
        }
        #endregion

        #endregion

        #region Constants
        private const string SeparatorUri = "://";
        private const string Style = "style";
        private const string StyleDisplayNone = "display: none;";
        private const string TagDiv = "div";
        #endregion
    }
}