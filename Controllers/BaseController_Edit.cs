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

namespace thZero.AspNetCore.Mvc
{
    public abstract partial class BaseController<TController>
    {
        #region Protected Methods
        protected IActionResult DeinitializeEditAction<T>(T model, Func<T, T, bool> methodValidate, Action<T, T, SubmitResult> methodSuccess, Func<T, SubmitResult, IActionResult> methodFail, Action<T, SubmitResult> methodDelete, string view)
            where T : class, IEditViewModel
        {
            const string Declaration = "DeinitializeEditAction";

            Enforce.AgainstNull(() => model);

            try
            {
                SubmitResult results = new();

                object value = TempData[Constants.CRUD.KeyCorrelation];
                if (value == null)
                    throw new InvalidCorrelationKeyException(InvalidCorrelationKey);

                Guid? correlationKey = (Guid)value;
                if (!correlationKey.HasValue || (correlationKey == Guid.Empty))
                    throw new InvalidCorrelationKeyException(InvalidCorrelationKey);
                if (correlationKey != model.CorrelationKey)
                    throw new InvalidCorrelationKeyMatchException(string.Format(InvalidCorrelationKeyFormat, correlationKey, model.CorrelationKey));

                T previousModel = (T)TempData[correlationKey.ToString()];

                if (!string.IsNullOrEmpty(model.Action))
                {
                    if (model.Action.EqualsIgnore(ActionCancel))
                        results.IsCancel = true;

                    if (model.Action.EqualsIgnore(ActionDelete))
                        results.IsDelete = true;

                    if (model.Action.EqualsIgnore(ActionSave))
                        results.IsSave = true;

                    DeinitializeRequestDetermineAction<T>(model, results);
                }

                if (results.IsCancel)
                    return JsonPost();

                if (results.IsSave)
                {
                    methodValidate?.Invoke(model, previousModel);

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            methodSuccess?.Invoke(model, previousModel, results);

                            if (results.Success)
                            {
                                DeinitializeEditActionSuccess();

                                if (Request.IsAjaxRequest())
                                    return JsonPost();

                                return (!string.IsNullOrEmpty(view) ? View(view, model) : View(model));
                            }
                        }
                        catch (DeinitializeActionSuccessException ex)
                        {
                            foreach (var error in ex.Errors)
                            {
                                if (error.Exception != null)
                                    ModelState.AddModelError(error.Key, error.Exception.Message);
                                else
                                    ModelState.AddModelError(error.Key, error.Message);
                            }
                        }
                    }

                    TempData.Keep();

                    if (methodFail != null)
                        return methodFail(model, results);

                    var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                    if (Request.IsAjaxRequest())
                        return JsonPostFailure(allErrors.Select(l => l.ErrorMessage));

                    foreach (var error in results.Errors)
                        ModelState.AddModelError(error.InputElement, error.Message);

                    throw new InvalidFailureException(InvalidFailureResult);
                }

                if (results.IsDelete)
                {
                    methodDelete?.Invoke(model, results);
                    DeinitializeEditActionSuccess();
                    return JsonPost();
                }

                return DeinitializeRequestHandleAction<T>(model, results);
            }
            catch (Exception ex)
            {
                Logger?.LogError2(Declaration, ex);
                if (Request.IsAjaxRequest())
                    return JsonPostFailure();
                throw;
            }
        }

        protected virtual void DeinitializeEditActionSuccess() { }

        protected virtual async Task<bool> DeinitializeEditActionSuccessAsync()
        {
            return await Task.FromResult(true);
        }

        protected IActionResult DeinitializeEditActionResult<T>(T model, Func<T, T, SubmitResult, IActionResult> methodSuccess)
            where T : class, IEditViewModel
        {
            return DeinitializeEditActionResult<T>(model, null, methodSuccess, null, null);
        }

        protected async Task<IActionResult> DeinitializeEditActionResultAsync<T>(T model, Func<T, T, SubmitResult, Task<IActionResult>> methodSuccess)
             where T : class, IEditViewModel
        {
            return await DeinitializeEditActionResultAsync<T>(model, null, methodSuccess, null, null);
        }

        protected IActionResult DeinitializeEditActionResult<T>(T model, Func<T, T, SubmitResult, IActionResult> methodSuccess, Func<T, SubmitResult, IActionResult> methodFail)
            where T : class, IEditViewModel
        {
            return DeinitializeEditActionResult<T>(model, null, methodSuccess, methodFail, null);
        }

        protected async Task<IActionResult> DeinitializeEditActionResultAsync<T>(T model, Func<T, T, SubmitResult, Task<IActionResult>> methodSuccess, Func<T, SubmitResult, Task<IActionResult>> methodFail)
            where T : class, IEditViewModel
        {
            return await DeinitializeEditActionResultAsync<T>(model, null, methodSuccess, methodFail, null);
        }

        protected IActionResult DeinitializeEditActionResult<T>(T model, Func<T, T, bool> methodValidate, Func<T, T, SubmitResult, IActionResult> methodSuccess)
            where T : class, IEditViewModel
        {
            return DeinitializeEditActionResult<T>(model, methodValidate, methodSuccess, null, null);
        }

        protected async Task<IActionResult> DeinitializeEditActionResultAsync<T>(T model, Func<T, T, Task<bool>> methodValidate, Func<T, T, SubmitResult, Task<IActionResult>> methodSuccess)
            where T : class, IEditViewModel
        {
            return await DeinitializeEditActionResultAsync<T>(model, methodValidate, methodSuccess, null, null);
        }

        protected IActionResult DeinitializeEditActionResult<T>(T model, Func<T, T, bool> methodValidate, Func<T, T, SubmitResult, IActionResult> methodSuccess, Func<T, SubmitResult, IActionResult> methodFail)
            where T : class, IEditViewModel
        {
            return DeinitializeEditActionResult<T>(model, methodValidate, methodSuccess, methodFail, null);
        }

        protected async Task<IActionResult> DeinitializeEditActionResultAsync<T>(T model, Func<T, T, Task<bool>> methodValidate, Func<T, T, SubmitResult, Task<IActionResult>> methodSuccess, Func<T, SubmitResult, Task<IActionResult>> methodFail)
            where T : class, IEditViewModel
        {
            return await DeinitializeEditActionResultAsync<T>(model, methodValidate, methodSuccess, methodFail, null);
        }

        protected IActionResult DeinitializeEditActionResult<T>(T model, Func<T, T, bool> methodValidate, Func<T, T, SubmitResult, IActionResult> methodSuccess, Action<T, SubmitResult> methodDelete)
            where T : class, IEditViewModel
        {
            return DeinitializeEditActionResult<T>(model, methodValidate, methodSuccess, null, methodDelete);
        }

        protected async Task<IActionResult> DeinitializeEditActionResultAsync<T>(T model, Func<T, T, Task<bool>> methodValidate, Func<T, T, SubmitResult, Task<IActionResult>> methodSuccess, Func<T, SubmitResult, Task<bool>> methodDelete)
            where T : class, IEditViewModel
        {
            return await DeinitializeEditActionResultAsync<T>(model, methodValidate, methodSuccess, null, methodDelete);
        }

        protected IActionResult DeinitializeEditActionResult<T>(T model, Func<T, T, SubmitResult, IActionResult> methodSuccess, Action<T, SubmitResult> methodDelete)
            where T : class, IEditViewModel
        {
            return DeinitializeEditActionResult<T>(model, null, methodSuccess, null, methodDelete);
        }

        protected async Task<IActionResult> DeinitializeEditActionResultAsync<T>(T model, Func<T, T, SubmitResult, Task<IActionResult>> methodSuccess, Func<T, SubmitResult, Task<bool>> methodDelete)
            where T : class, IEditViewModel
        {
            return await DeinitializeEditActionResultAsync<T>(model, null, methodSuccess, null, methodDelete);
        }

        protected IActionResult DeinitializeEditActionResult<T>(T model, Func<T, T, SubmitResult, IActionResult> methodSuccess, Func<T, SubmitResult, IActionResult> methodFail, Action<T, SubmitResult> methodDelete)
            where T : class, IEditViewModel
        {
            return DeinitializeEditActionResult<T>(model, null, methodSuccess, methodFail, methodDelete);
        }

        protected async Task<IActionResult> DeinitializeEditActionResultAsync<T>(T model, Func<T, T, SubmitResult, Task<IActionResult>> methodSuccess, Func<T, SubmitResult, Task<IActionResult>> methodFail, Func<T, SubmitResult, Task<bool>> methodDelete)
            where T : class, IEditViewModel
        {
            return await DeinitializeEditActionResultAsync<T>(model, null, methodSuccess, methodFail, methodDelete);
        }

        protected IActionResult DeinitializeEditActionResult<T>(T model, Func<T, T, bool> methodValidate, Func<T, T, SubmitResult, IActionResult> methodSuccess, Func<T, SubmitResult, IActionResult> methodFail, Action<T, SubmitResult> methodDelete)
            where T : class, IEditViewModel
        {
            const string Declaration = "DeinitializeEditActionResult";

            Enforce.AgainstNull<IEditViewModel>(() => model);
            Enforce.AgainstNull(() => methodSuccess);

            try
            {
                SubmitResult results = new();

                object value = TempData[Constants.CRUD.KeyCorrelation];
                if (value == null)
                    throw new InvalidCorrelationKeyException(InvalidCorrelationKey);

                Guid? correlationKey = (Guid)value;
                if (!correlationKey.HasValue || (correlationKey == Guid.Empty))
                    throw new InvalidCorrelationKeyException(InvalidCorrelationKey);

                if (correlationKey != model.CorrelationKey)
                    throw new InvalidCorrelationKeyMatchException(string.Format(InvalidCorrelationKeyFormat, correlationKey, model.CorrelationKey));

                T previousModel = (T)TempData[correlationKey.ToString()];

                if (!string.IsNullOrEmpty(model.Action))
                {
                    if (model.Action.EqualsIgnore(ActionCancel))
                        results.IsCancel = true;

                    if (model.Action.EqualsIgnore(ActionDelete))
                        results.IsDelete = true;

                    if (model.Action.EqualsIgnore(ActionSave))
                        results.IsSave = true;

                    DeinitializeRequestDetermineAction<T>(model, results);
                }

                if (results.IsCancel)
                    return JsonPost();

                if (results.IsSave)
                {
                    methodValidate?.Invoke(model, previousModel);

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            IActionResult result = methodSuccess(model, previousModel, results);

                            DeinitializeEditActionResultSuccess();

                            if (result != null)
                            {
                                if (Request.IsAjaxRequest())
                                    return JsonPost();

                                return result;
                            }

                            throw new InvalidFailureException(InvalidFailureResult);
                        }
                        catch (DeinitializeActionSuccessException ex)
                        {
                            foreach (var error in ex.Errors)
                            {
                                if (error.Exception != null)
                                    ModelState.AddModelError(error.Key, error.Exception.Message);
                                else
                                    ModelState.AddModelError(error.Key, error.Message);
                            }
                        }
                    }

                    TempData.Keep();

                    if (methodFail != null)
                        return methodFail(model, results);

                    var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                    if (Request.IsAjaxRequest())
                        return JsonPostFailure(allErrors.Select(l => l.ErrorMessage));

                    throw new InvalidFailureException(InvalidFailureResult);
                }

                if (results.IsDelete)
                {
                    methodDelete?.Invoke(model, results);
                    DeinitializeEditActionResultSuccess();

                    return JsonPost();
                }

                return DeinitializeRequestHandleAction<T>(model, results);
            }
            catch (Exception ex)
            {
                Logger?.LogError2(Declaration, ex);
                if (Request.IsAjaxRequest())
                    return JsonPostFailure();
                throw;
            }
        }

        protected async Task<IActionResult> DeinitializeEditActionResultAsync<T>(T model, Func<T, T, Task<bool>> methodValidate, Func<T, T, SubmitResult, Task<IActionResult>> methodSuccess, Func<T, SubmitResult, Task<IActionResult>> methodFail, Func<T, SubmitResult, Task<bool>> methodDelete)
            where T : class, IEditViewModel
        {
            const string Declaration = "DeinitializeEditActionResultAsync";

            Enforce.AgainstNull<IEditViewModel>(() => model);
            Enforce.AgainstNull(() => methodSuccess);

            try
            {
                SubmitResult results = new();

                object value = TempData[Constants.CRUD.KeyCorrelation];
                if (value == null)
                    throw new InvalidCorrelationKeyException(InvalidCorrelationKey);

                Guid? correlationKey = (Guid)value;
                if (!correlationKey.HasValue || (correlationKey == Guid.Empty))
                    throw new InvalidCorrelationKeyException(InvalidCorrelationKey);

                if (correlationKey != model.CorrelationKey)
                    throw new InvalidCorrelationKeyMatchException(string.Format(InvalidCorrelationKeyFormat, correlationKey, model.CorrelationKey));

                T previousModel = (T)TempData[correlationKey.ToString()];

                if (!string.IsNullOrEmpty(model.Action))
                {
                    if (model.Action.EqualsIgnore(ActionCancel))
                        results.IsCancel = true;

                    if (model.Action.EqualsIgnore(ActionDelete))
                        results.IsDelete = true;

                    if (model.Action.EqualsIgnore(ActionSave))
                        results.IsSave = true;

                    await DeinitializeRequestDetermineActionAsync<T>(model, results);
                }

                if (results.IsCancel)
                    return await Task.FromResult(JsonPost());

                if (results.IsSave)
                {
                    if (methodValidate != null)
                        await methodValidate(model, previousModel);

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            IActionResult result = await methodSuccess(model, previousModel, results);

                            DeinitializeEditActionResultSuccess();

                            if (result != null)
                            {
                                if (Request.IsAjaxRequest())
                                    return await Task.FromResult(JsonPost());

                                return await Task.FromResult(result);
                            }

                            throw new InvalidFailureException(InvalidFailureResult);
                        }
                        catch (DeinitializeActionSuccessException ex)
                        {
                            foreach (var error in ex.Errors)
                            {
                                if (error.Exception != null)
                                    ModelState.AddModelError(error.Key, error.Exception.Message);
                                else
                                    ModelState.AddModelError(error.Key, error.Message);
                            }
                        }
                    }

                    TempData.Keep();

                    if (methodFail != null)
                        return await methodFail(model, results);

                    var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                    if (Request.IsAjaxRequest())
                        return await Task.FromResult(JsonPostFailure(allErrors.Select(l => l.ErrorMessage)));

                    throw new InvalidFailureException(InvalidFailureResult);
                }

                if (results.IsDelete)
                {
                    if (methodDelete != null)
                        await methodDelete(model, results);
                    await DeinitializeEditActionResultSuccessAsync();

                    return await Task.FromResult(JsonPost());
                }

                return await DeinitializeRequestHandleActionAsync<T>(model, results);
            }
            catch (Exception ex)
            {
                Logger?.LogError2(Declaration, ex);
                if (Request.IsAjaxRequest())
                    return await Task.FromResult(JsonPostFailure());
                throw;
            }
        }

        protected virtual void DeinitializeEditActionResultSuccess() { }

        protected virtual async Task<bool> DeinitializeEditActionResultSuccessAsync()
        {
            return await Task.FromResult(true);
        }

        protected IActionResult InitializeEditAction<T>(T model, string view)
            where T : class, IEditViewModel
        {
            return InitializeEditAction(model, null, null, view);
        }

        protected async Task<IActionResult> InitializeEditActionAsync<T>(T model, string view)
            where T : class, IEditViewModel
        {
            return await InitializeEditActionAsync(model, null, null, view);
        }

        protected IActionResult InitializeEditAction<T>(T model, Action<T> methodLoad)
            where T : class, IEditViewModel
        {
            return InitializeEditAction(model, null, methodLoad, null);
        }

        protected async Task<IActionResult> InitializeEditActionAsync<T>(T model, Func<T, Task<bool>> methodLoad)
             where T : class, IEditViewModel
        {
            return await InitializeEditActionAsync(model, null, methodLoad, null);
        }

        protected IActionResult InitializeEditAction<T>(T model, Func<T, bool> methodValidate, Action<T> methodLoad)
            where T : class, IEditViewModel
        {
            return InitializeEditAction(model, methodValidate, methodLoad, null);
        }

        protected async Task<IActionResult> InitializeEditActionAsync<T>(T model, Func<T, Task<bool>> methodValidate, Func<T, Task<bool>> methodLoad)
             where T : class, IEditViewModel
        {
            return await InitializeEditActionAsync(model, methodValidate, methodLoad, null);
        }

        protected IActionResult InitializeEditAction<T>(T model, Func<T, bool> methodValidate, Action<T> methodLoad, string view)
            where T : class, IEditViewModel
        {
            const string Declaration = "InitializeEditAction";

            Enforce.AgainstNull<IEditViewModel>(() => model);

            try
            {
                model.CorrelationKey = Guid.NewGuid();
                TempData[model.CorrelationKey.ToString()] = model;
                TempData[Constants.CRUD.KeyCorrelation] = model.CorrelationKey;

                if (methodValidate != null)
                {
                    if (!methodValidate(model))
                    {
                        if (Request.IsAjaxRequest())
                            return JsonGetFailure();

                        return (!string.IsNullOrEmpty(view) ? View(view, model) : View(model));
                    }
                }

                methodLoad?.Invoke(model);

                return (!string.IsNullOrEmpty(view) ? View(view, model) : View(model));
            }
            catch (Exception ex)
            {
                Logger?.LogError2(Declaration, ex);
                throw;
            }
        }

        protected async Task<IActionResult> InitializeEditActionAsync<T>(T model, Func<T, Task<bool>> methodValidate, Func<T, Task<bool>> methodLoad, string view)
            where T : class, IEditViewModel
        {
            const string Declaration = "InitializeEditActionAsync";

            Enforce.AgainstNull<IEditViewModel>(() => model);

            try
            {
                model.CorrelationKey = Guid.NewGuid();
                TempData[model.CorrelationKey.ToString()] = model;
                TempData[Constants.CRUD.KeyCorrelation] = model.CorrelationKey;

                if (methodValidate != null)
                {
                    var resultV = await methodValidate(model);
                    if (Request.IsAjaxRequest())
                        return await Task.FromResult(JsonGetFailure());
                }

                if (methodLoad != null)
                    await methodLoad(model);

                return await Task.FromResult(!string.IsNullOrEmpty(view) ? View(view, model) : View(model));
            }
            catch (Exception ex)
            {
                Logger?.LogError2(Declaration, ex);
                throw;
            }
        }

        protected IActionResult InitializeEditActionResult<T>(T model, Func<T, IActionResult> methodValidate, Func<T, IActionResult> methodLoad)
            where T : class, IEditViewModel
        {
            const string Declaration = "InitializeEditActionResult";

            Enforce.AgainstNull<IEditViewModel>(() => model);
            Enforce.AgainstNull(() => methodLoad);

            try
            {
                model.CorrelationKey = Guid.NewGuid();
                TempData[model.CorrelationKey.ToString()] = model;
                TempData[Constants.CRUD.KeyCorrelation] = model.CorrelationKey;

                if (methodValidate != null)
                {
                    IActionResult resultV = methodValidate(model);
                    if (resultV != null)
                        return resultV;
                }

                IActionResult result = methodLoad(model);
                if (result != null)
                    return result;

                throw new InvalidFailureException();
            }
            catch (Exception ex)
            {
                Logger?.LogError2(Declaration, ex);
                throw;
            }
        }

        protected async Task<IActionResult> InitializeEditActionResultAsync<T>(T model, Func<T, Task<IActionResult>> methodValidate, Func<T, Task<IActionResult>> methodLoad)
            where T : class, IEditViewModel
        {
            const string Declaration = "InitializeEditActionResultAsync";

            Enforce.AgainstNull<IEditViewModel>(() => model);
            Enforce.AgainstNull(() => methodLoad);

            try
            {
                model.CorrelationKey = Guid.NewGuid();
                TempData[model.CorrelationKey.ToString()] = model;
                TempData[Constants.CRUD.KeyCorrelation] = model.CorrelationKey;

                if (methodValidate != null)
                {
                    IActionResult resultV = await methodValidate(model);
                    if (resultV != null)
                        return await Task.FromResult(resultV);
                }

                IActionResult result = await methodLoad(model);
                if (result != null)
                    return await Task.FromResult(result);

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
}