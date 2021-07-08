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
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using thZero.AspNetCore.Mvc.Views.Models;
using thZero.AspNetCore.Results;
using thZero.Responses;

namespace thZero.AspNetCore.Mvc
{
    public abstract partial class BaseController<TController>
    {
        #region Protected Methods
        protected ActionResult DeinitializeDeleteAction<T>(T model, Func<T, bool> methodDelete)
            where T : IEditViewModel
        {
            return DeinitializeDeleteAction<T>(model, null, methodDelete, null);
        }

        protected async Task<ActionResult> DeinitializeDeleteActionAsync<T>(T model, Func<T, Task<bool>> methodDelete)
            where T : IEditViewModel
        {
            return await DeinitializeDeleteActionAsync<T>(model, null, methodDelete, null);
        }

        protected ActionResult DeinitializeDeleteAction<T>(T model, Func<T, bool> methodValidate, Func<T, bool> methodDelete)
            where T : IEditViewModel
        {
            return DeinitializeDeleteAction<T>(model, methodValidate, methodDelete, null);
        }

        protected async Task<ActionResult> DeinitializeDeleteActionAsync<T>(T model, Func<T, Task<bool>> methodValidate, Func<T, Task<bool>> methodDelete)
            where T : IEditViewModel
        {
            return await DeinitializeDeleteActionAsync<T>(model, methodValidate, methodDelete, null);
        }

        protected ActionResult DeinitializeDeleteAction<T>(T model, Func<T, bool> methodValidate, Func<T, bool> methodDelete, string view)
            where T : IEditViewModel
        {
            const string Declaration = "DeinitializeDeleteAction";

            Enforce.AgainstNull(() => methodDelete);

            bool isPost = Request.IsPostRequest();

            try
            {
                if (ModelState.IsValid)
                {
                    if ((methodValidate != null) && !methodValidate(model))
                        return (isPost ? JsonPostFailure() : JsonGetFailure());

                    if (methodDelete(model) && (Request.IsAjaxRequest()))
                        return (isPost ? JsonPost() : JsonGet());

                    if (Request.IsAjaxRequest())
                        return (isPost ? JsonPostFailure() : JsonGetFailure());

                    return View(view);
                }

                TempData.Keep();

                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                if (Request.IsAjaxRequest())
                {
                    if (isPost)
                    {
                        SubmitResult results = new();

                        foreach (var error in allErrors)
                            results.AddError(error.ErrorMessage);

                        return JsonPostFailure(results);
                    }

                    return JsonGetFailure();
                }

                throw new InvalidFailureException(InvalidFailureResult);
            }
            catch (Exception ex)
            {
                Logger?.LogError2(Declaration, ex);
                if (Request.IsAjaxRequest())
                    return (isPost ? JsonPostFailure() : JsonGetFailure());
                throw;
            }
        }

        protected async Task<ActionResult> DeinitializeDeleteActionAsync<T>(T model, Func<T, Task<bool>> methodValidate, Func<T, Task<bool>> methodDelete, string view)
            where T : IEditViewModel
        {
            const string Declaration = "DeinitializeDeleteActionAsync";

            Enforce.AgainstNull(() => methodDelete);

            bool isPost = Request.IsPostRequest();

            try
            {
                if (ModelState.IsValid)
                {
                    if ((methodValidate != null) && !await methodValidate(model))
                        return await Task.FromResult((isPost ? JsonPostFailure() : JsonGetFailure()));

                    if ((await methodDelete(model)) && Request.IsAjaxRequest())
                        return JsonPost();

                    if (Request.IsAjaxRequest())
                        return (isPost ? JsonPostFailure() : JsonGetFailure());

                    return View(view);
                }

                TempData.Keep();

                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                if (Request.IsAjaxRequest())
                {
                    if (isPost)
                    {
                        SubmitResult results = new();

                        foreach (var error in allErrors)
                            results.AddError(error.ErrorMessage);

                        return await Task.FromResult(JsonPostFailure(results));
                    }

                    return await Task.FromResult(JsonPostFailure());
                }

                throw new InvalidFailureException(InvalidFailureResult);
            }
            catch (Exception ex)
            {
                Logger?.LogError2(Declaration, ex);
                if (Request.IsAjaxRequest())
                    return await Task.FromResult((isPost ? JsonPostFailure() : JsonGetFailure()));
                throw;
            }
        }

        protected JsonResult JsonDelete(object data)
        {
            return new JsonResultEx(new SuccessResponse<object>() { Success = true, Data = data });
        }

        protected JsonResult JsonViewDelete(object data)
        {
            return new JsonResultEx(new JsonOutputResponseViewModel<object>() { Success = true, Data = data });
        }

        protected JsonResult JsonDelete<T>(T data)
            where T : class
        {
            return new JsonResultEx(new SuccessResponse<T>() { Success = true, Data = data });
        }

        protected JsonResult JsonViewDelete<T>(T data)
            where T : class
        {
            return new JsonResultEx(new JsonOutputResponseViewModel<T>() { Success = true, Data = data });
        }

        protected virtual JsonResult JsonDeleteFailure(string message)
        {
            return new JsonResultEx((new ErrorResponse()).AddError(message));
        }

        protected virtual JsonResult JsonViewDeleteFailure(string message)
        {
            return new JsonResultEx((new JsonOutputErrorResponseViewModel()).AddError(message));
        }

        protected virtual JsonResult JsonDeleteFailure()
        {
            return new JsonResultEx(new ErrorResponse());
        }

        protected virtual JsonResult JsonViewDeleteFailure()
        {
            return new JsonResultEx(new JsonOutputErrorResponseViewModel());
        }
        #endregion
    }
}