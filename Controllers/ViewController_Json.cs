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

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using thZero.AspNetCore.Mvc.Views.Models;
using thZero.Responses;

namespace thZero.AspNetCore.Mvc
{
    public abstract partial class ViewController<TController>
    {
    }

    public abstract partial class ViewController
    {
        #region Protected Methods
        protected override JsonResult JsonGet(object data)
        {
            return Json(new JsonOutputResponseViewModel<object>() { Success = true, Data = data });
        }

        protected override JsonResult JsonGet<T>(T data)
            where T : class
        {
            return Json(new SuccessResponse<T>() { Success = true, Results = data });
        }

        protected override JsonResult JsonGetFailure(string message)
        {
            return Json((new JsonOutputErrorResponseViewModel()).AddError(message));
        }

        protected override JsonResult JsonGetFailure()
        {
            return Json(new JsonOutputErrorResponseViewModel());
        }

        protected virtual JsonResult JsonGetSelect(IEnumerable<SelectListItem> list)
        {
            return Json(new JsonOutputSearchResponseViewModel<SelectListItem>() { Success = true, Data = list });
        }

        protected override JsonResult JsonPost(object data)
        {
            return Json(new JsonOutputResponseViewModel<object>() { Success = true, Data = data });
        }

        protected override JsonResult JsonPost<T>(T data)
            where T : class
        {
            return Json(new JsonOutputResponseViewModel<T>() { Success = true, Data = data });
        }

        protected override JsonResult JsonPostFailure()
        {
            return Json(new JsonOutputErrorResponseViewModel());
        }

        protected override JsonResult JsonPostFailure(IEnumerable<string> errors)
        {
            return Json((new JsonOutputErrorResponseViewModel()).AddErrors(errors));
        }

        protected override JsonResult JsonPostFailure(string message)
        {
            return Json((new JsonOutputErrorResponseViewModel()).AddError(message));
        }

        protected override JsonResult JsonPostFailure<T>(T data)
            where T : class
        {
            return Json(new JsonOutputResponseViewModel<T>() { Success = false, Data = data });
        }

        protected override JsonResult JsonPostFailure<T>(T data, string message)
            where T : class
        {
            return Json((new JsonOutputResponseViewModel<T>() { Success = false, Data = data }).AddError(message));
        }
        #endregion
    }
}