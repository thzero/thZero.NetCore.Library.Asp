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
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using thZero.AspNetCore.Mvc.Views.Models;
using thZero.Responses;

namespace thZero.AspNetCore.Mvc
{
    public abstract partial class BaseController<TController>
    {
        #region Protected Methods
        protected IActionResult InitializeJsonGetResult<T>(T model, Func<T, IActionResult> methodLoad)
            where T : class
        {
            return InitializeJsonGetResult(model, null, methodLoad);
        }

        protected IActionResult InitializeJsonGetResult<T>(T model, Func<T, bool> methodValidate, Func<T, IActionResult> methodLoad)
            where T : class
        {
            const string Declaration = "InitializeJsonGetResult";

            Enforce.AgainstNull(() => model);
            Enforce.AgainstNull(() => methodLoad);

            try
            {
                if (methodValidate != null)
                {
                    if (!methodValidate(model))
                        return JsonGetFailure();
                }

                if (ModelState.IsValid)
                    return methodLoad(model);

                throw new InvalidFailureException();
            }
            catch (Exception ex)
            {
                Logger?.LogError2(Declaration, ex);
                throw;
            }
        }

        protected async Task<IActionResult> InitializeJsonGetResultAsync<T>(T model, Func<T, Task<IActionResult>> methodLoad)
            where T : class
        {
            return await InitializeJsonGetResultAsync<T>(model, null, methodLoad);
        }

        protected async Task<IActionResult> InitializeJsonGetResultAsync<T>(T model, Func<T, Task<bool>> methodValidate, Func<T, Task<IActionResult>> methodLoad)
            where T : class
        {
            const string Declaration = "InitializeJsonGetResultAsync";

            Enforce.AgainstNull(() => model);
            Enforce.AgainstNull(() => methodLoad);

            try
            {
                if (methodValidate != null)
                {
                    if (!(await methodValidate(model)))
                        return await Task.FromResult(JsonGetFailure());
                }

                return await Task.FromResult(await methodLoad(model));

                throw new InvalidFailureException();
            }
            catch (Exception ex)
            {
                Logger?.LogError2(Declaration, ex);
                throw;
            }
        }

        protected IActionResult InitializeJsonPostResult<T>(T model, Func<T, IActionResult> methodPerform)
            where T : class
        {
            return InitializeJsonPostResult(model, null, methodPerform);
        }

        protected IActionResult InitializeJsonPostResult<T>(T model, Func<T, bool> methodValidate, Func<T, IActionResult> methodPerform)
            where T : class
        {
            const string Declaration = "InitializeJsonPostResult";

            Enforce.AgainstNull(() => model);
            Enforce.AgainstNull(() => methodPerform);

            try
            {
                if (methodValidate != null)
                {
                    if (!methodValidate(model))
                        return JsonPostFailure();
                }

                if (ModelState.IsValid)
                    return methodPerform(model);

                throw new InvalidFailureException();
            }
            catch (Exception ex)
            {
                Logger?.LogError2(Declaration, ex);
                throw;
            }
        }

        protected async Task<IActionResult> InitializeJsonPostResultAsync<T>(T model, Func<T, Task<IActionResult>> methodPerform)
            where T : class
        {
            return await InitializeJsonPostResultAsync<T>(model, null, methodPerform);
        }

        protected async Task<IActionResult> InitializeJsonPostResultAsync<T>(T model, Func<T, Task<bool>> methodValidate, Func<T, Task<IActionResult>> methodPerform)
            where T : class
        {
            const string Declaration = "InitializeJsonPostResultAsync";

            Enforce.AgainstNull(() => model);
            Enforce.AgainstNull(() => methodPerform);

            try
            {
                if (methodValidate != null)
                {
                    if (!(await methodValidate(model)))
                        return await Task.FromResult(JsonPostFailure());
                }

                return await Task.FromResult(await methodPerform(model));

                throw new InvalidFailureException();
            }
            catch (Exception ex)
            {
                Logger?.LogError2(Declaration, ex);
                throw;
            }
        }
        #endregion
    }

    public abstract partial class BaseController
    {
        #region Protected Methods
        protected virtual JsonResult JsonGet()
        {
            return JsonGet(null);
        }

        protected virtual JsonResult JsonGet(object data)
        {
            return new JsonResultEx(new SuccessResponse<object>() { Success = true, Data = data });
        }

        protected virtual JsonResult JsonGet<T>(T data)
            where T : class
        {
            return new JsonResultEx(new SuccessResponse<T>() { Success = true, Data = data });
        }

        protected virtual JsonResult JsonGetFailure(string message)
        {
            return new JsonResultEx((new ErrorResponse()).AddError(message));
        }

        protected virtual JsonResult JsonGetFailure()
        {
            return new JsonResultEx(new ErrorResponse());
        }

        protected virtual JsonResult JsonVGetSelect(IEnumerable<SelectListItem> list)
        {
            return new JsonResultEx(new SearchSuccessResponse<SelectListItem>() { Success = true, Data = list });
        }

        protected virtual JsonResult JsonPost()
        {
            return JsonPost(null);
        }

        protected virtual JsonResult JsonPost(object data)
        {
            return new JsonResultEx(new SuccessResponse<object>() { Success = true, Data = data });
        }

        protected virtual JsonResult JsonPost<T>(T data)
            where T : class
        {
            return new JsonResultEx(new SuccessResponse<T>() { Success = true, Data = data });
        }

        protected virtual JsonResult JsonPostFailure()
        {
            return new JsonResultEx(new ErrorResponse());
        }

        protected virtual JsonResult JsonPostFailure(IEnumerable<string> errors)
        {
            return new JsonResultEx((new ErrorResponse()).AddErrors(errors));
        }

        protected virtual JsonResult JsonPostFailure(string message)
        {
            return new JsonResultEx((new ErrorResponse()).AddError(message));
        }

        protected virtual JsonResult JsonPostFailure<T>(T data)
            where T : class
        {
            return new JsonResultEx(new SuccessResponse<T>() { Success = false, Data = data });
        }

        protected virtual JsonResult JsonPostFailure<T>(T data, string message)
            where T : class
        {
            return new JsonResultEx((new SuccessResponse<T>() { Success = false, Data = data }).AddError(message));
        }
        #endregion
    }
}