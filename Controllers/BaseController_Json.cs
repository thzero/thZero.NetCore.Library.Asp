﻿/* ------------------------------------------------------------------------- *
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
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using thZero.AspNetCore.Results;
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

                if (!ModelState.IsValid)
                {
                    SubmitResult results = new();

                    var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                    if (Request.IsAjaxRequest())
                        return JsonPostFailure(allErrors.Select(l => l.ErrorMessage));

                    foreach (var error in results.Errors)
                        ModelState.AddModelError(error.InputElement, error.Message);

                    throw new InvalidFailureException(InvalidFailureResult);
                }

                return methodLoad(model);
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

        protected async Task<IActionResult> InitializeJsonGetResultAsync<T, U>(T model, Func<T, Task<U>> methodLoad)
            where T : class
        {
            return await InitializeJsonGetResultAsync<T, U>(model, null, methodLoad);
        }

        protected async Task<IActionResult> InitializeJsonGetResultAsync<T>(T model, Func<T, Task<bool>> methodValidate, Func<T, Task<IActionResult>> methodLoad)
        {
            const string Declaration = "InitializeJsonGetResultAsync";

            Enforce.AgainstNull(() => methodLoad);

            try
            {
                if (methodValidate != null)
                {
                    if (!(await methodValidate(model)))
                        return JsonGetFailure();
                }

                if (!ModelState.IsValid)
                {
                    SubmitResult results = new();

                    var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                    if (Request.IsAjaxRequest())
                        return JsonPostFailure(allErrors.Select(l => l.ErrorMessage));

                    foreach (var error in results.Errors)
                        ModelState.AddModelError(error.InputElement, error.Message);

                    throw new InvalidFailureException(InvalidFailureResult);
                }

                return Json(await methodLoad(model));
            }
            catch (Exception ex)
            {
                Logger?.LogError2(Declaration, ex);
                throw;
            }
        }

        protected async Task<IActionResult> InitializeJsonGetResultAsync<T, U>(T model, Func<T, Task<bool>> methodValidate, Func<T, Task<U>> methodLoad)
        {
            const string Declaration = "InitializeJsonGetResultAsync";

            Enforce.AgainstNull(() => methodLoad);

            try
            {
                if (methodValidate != null)
                {
                    if (!(await methodValidate(model)))
                        return JsonGetFailure();
                }

                if (!ModelState.IsValid)
                {
                    SubmitResult results = new();

                    var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                    if (Request.IsAjaxRequest())
                        return JsonPostFailure(allErrors.Select(l => l.ErrorMessage));

                    foreach (var error in results.Errors)
                        ModelState.AddModelError(error.InputElement, error.Message);

                    throw new InvalidFailureException(InvalidFailureResult);
                }

                return Json(await methodLoad(model));
            }
            catch (Exception ex)
            {
                Logger?.LogError2(Declaration, ex);
                throw;
            }
        }

        protected async Task<IActionResult> InitializeJsonGetResultAsync<TInput, TResponse, TOutput>(TInput model, Func<TInput, Task<bool>> methodValidate, Func<TInput, Task<TResponse>> methodLoad)
            where TResponse : SuccessResponse<TOutput>
            where TOutput : class
        {
            const string Declaration = "InitializeJsonGetResultAsync";

            Enforce.AgainstNull(() => methodLoad);

            try
            {
                if (methodValidate != null)
                {
                    if (!(await methodValidate(model)))
                        return JsonGetFailure();
                }

                if (!ModelState.IsValid)
                {
                    SubmitResult results = new();

                    var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                    if (Request.IsAjaxRequest())
                        return JsonPostFailure(allErrors.Select(l => l.ErrorMessage));

                    foreach (var error in results.Errors)
                        ModelState.AddModelError(error.InputElement, error.Message);

                    throw new InvalidFailureException(InvalidFailureResult);
                }

                return Json(await methodLoad(model));
            }
            catch (Exception ex)
            {
                Logger?.LogError2(Declaration, ex);
                throw;
            }
        }

        protected IActionResult InitializeJsonPostResult<T>(T model, Func<T, IActionResult> methodPerform)
        {
            return InitializeJsonPostResult(model, null, methodPerform);
        }

        protected IActionResult InitializeJsonPostResult<T>(T model, Func<T, bool> methodValidate, Func<T, IActionResult> methodPerform)
        {
            const string Declaration = "InitializeJsonPostResult";

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

                SubmitResult results = new();

                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                if (Request.IsAjaxRequest())
                    return JsonPostFailure(allErrors.Select(l => l.ErrorMessage));

                foreach (var error in results.Errors)
                    ModelState.AddModelError(error.InputElement, error.Message);

                throw new InvalidFailureException(InvalidFailureResult);
            }
            catch (Exception ex)
            {
                Logger?.LogError2(Declaration, ex);
                throw;
            }
        }

        protected async Task<IActionResult> InitializeJsonPostResultAsync<T>(T model, Func<T, Task<IActionResult>> methodPerform)
        {
            return await InitializeJsonPostResultAsync<T>(model, null, methodPerform);
        }

        protected async Task<IActionResult> InitializeJsonPostResultAsync<T>(T model, Func<T, Task<bool>> methodValidate, Func<T, Task<IActionResult>> methodPerform)
        {
            const string Declaration = "InitializeJsonPostResultAsync";

            Enforce.AgainstNull(() => methodPerform);

            try
            {
                if (methodValidate != null)
                {
                    if (!(await methodValidate(model)))
                        return JsonPostFailure();
                }

                if (!ModelState.IsValid)
                {
                    SubmitResult results = new();

                    var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                    if (Request.IsAjaxRequest())
                        return JsonPostFailure(allErrors.Select(l => l.ErrorMessage));

                    foreach (var error in results.Errors)
                        ModelState.AddModelError(error.InputElement, error.Message);

                    throw new InvalidFailureException(InvalidFailureResult);
                }

                return Json(await methodPerform(model));
            }
            catch (Exception ex)
            {
                Logger?.LogError2(Declaration, ex);
                throw;
            }
        }

        protected async Task<IActionResult> InitializeJsonPostResultAsync<T, U>(T model, Func<T, Task<bool>> methodValidate, Func<T, Task<U>> methodLoad)
        {
            const string Declaration = "InitializeJsonPostResultAsync";

            Enforce.AgainstNull(() => methodLoad);

            try
            {
                if (methodValidate != null)
                {
                    if (!(await methodValidate(model)))
                        return JsonPostFailure();
                }

                if (!ModelState.IsValid)
                {
                    SubmitResult results = new();

                    var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                    if (Request.IsAjaxRequest())
                        return JsonPostFailure(allErrors.Select(l => l.ErrorMessage));

                    foreach (var error in results.Errors)
                        ModelState.AddModelError(error.InputElement, error.Message);

                    throw new InvalidFailureException(InvalidFailureResult);
                }

                return Json(await methodLoad(model));
            }
            catch (Exception ex)
            {
                Logger?.LogError2(Declaration, ex);
                throw;
            }
        }

        protected async Task<IActionResult> InitializeJsonPostResultAsync<TInput, TResponse, TOutput>(TInput model, Func<TInput, Task<bool>> methodValidate, Func<TInput, Task<TResponse>> methodLoad)
            where TResponse : SuccessResponse<TOutput>
            where TOutput : class
        {
            const string Declaration = "InitializeJsonPostResultAsync";

            Enforce.AgainstNull(() => methodLoad);

            try
            {
                if (methodValidate != null)
                {
                    if (!(await methodValidate(model)))
                        return JsonPostFailure();
                }

                if (!ModelState.IsValid)
                {
                    SubmitResult results = new();

                    var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                    if (Request.IsAjaxRequest())
                        return JsonPostFailure(allErrors.Select(l => l.ErrorMessage));

                    foreach (var error in results.Errors)
                        ModelState.AddModelError(error.InputElement, error.Message);

                    throw new InvalidFailureException(InvalidFailureResult);
                }

                return Json(await methodLoad(model));
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
            return JsonGet(new SuccessResponse());
        }

        protected virtual JsonResult JsonGet(object data)
        {
            return Json(new SuccessResponse<object>() { Results = data });
        }

        protected virtual JsonResult JsonGet<T>(T data)
            where T : class
        {
            return Json(new SuccessResponse<T>() { Results = data });
        }

        protected virtual JsonResult JsonGetFailure(string message)
        {
            return Json((new ErrorResponse()).AddError(message));
        }

        protected virtual JsonResult JsonGetFailure()
        {
            return Json(new ErrorResponse());
        }

        protected virtual JsonResult JsonVGetSelect(IEnumerable<SelectListItem> list)
        {
            return Json(new SearchSuccessResponse<SelectListItem>() { Data = list });
        }

        protected virtual JsonResult JsonPost()
        {
            return Json(new SuccessResponse());
        }

        protected virtual JsonResult JsonPost(object data)
        {
            return Json(new SuccessResponse<object>() { Results = data });
        }

        protected virtual JsonResult JsonPost<T>(T data)
            where T : class
        {
            return Json(new SuccessResponse<T>() { Results = data });
        }

        protected virtual JsonResult JsonPostFailure()
        {
            return Json(new ErrorResponse());
        }

        protected virtual JsonResult JsonPostFailure(IEnumerable<string> errors)
        {
            return Json((new ErrorResponse()).AddErrors(errors));
        }

        protected virtual JsonResult JsonPostFailure(string message)
        {
            return Json((new ErrorResponse()).AddError(message));
        }

        protected virtual JsonResult JsonPostFailure<T>(T data)
            where T : class
        {
            return Json(new SuccessResponse<T>() { Success = false, Results = data });
        }

        protected virtual JsonResult JsonPostFailure<T>(T data, string message)
            where T : class
        {
            return Json((new SuccessResponse<T>() { Success = false, Results = data }).AddError(message));
        }
        #endregion
    }
}