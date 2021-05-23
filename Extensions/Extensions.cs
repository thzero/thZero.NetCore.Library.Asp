/* ------------------------------------------------------------------------- *
thZero.NetCore.Library.Asp
Copyright (C) 2016-2021 thZero.com

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

using Microsoft.AspNetCore.Http;

namespace thZero.AspNetCore
{
    public static class Extensions
    {
        #region Public Methods
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            Enforce.AgainstNull(() => request);
            Enforce.AgainstNull(() => request.Headers);

            return RequestAjax.EqualsIgnore(request.Headers[RequestAjaxValue]);
        }

        public static bool IsPostRequest(this HttpRequest request)
        {
            Enforce.AgainstNull(() => request);

            return RequestPost.EqualsIgnore(request.Method);
        }
        #endregion

        #region Constants
        private const string RequestAjax = "XMLHttpRequest";
        private const string RequestAjaxValue = "X-Requested-With";
        private const string RequestPost = "POST";
        #endregion
    }
}