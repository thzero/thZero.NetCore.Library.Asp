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
        protected IActionResult DeinitializeInsertAction<T>(T model, Action<T, T, SubmitResult> methodSuccess, bool useCorrelationKey = true)
            where T : class, IEditViewModel
        {
            return DeinitializeInsertAction<T>(model, null, methodSuccess, null, null, useCorrelationKey);
        }

        protected async Task<IActionResult> DeinitializeInsertActionAsync<T>(T model, Func<T, T, SubmitResult, Task<bool>> methodSuccess, bool useCorrelationKey = true)
            where T : class, IEditViewModel
        {
            return await DeinitializeInsertActionAsync<T>(model, null, methodSuccess, null, null, useCorrelationKey);
        }

        protected IActionResult DeinitializeInsertAction<T>(T model, Action<T, T, SubmitResult> methodSuccess, string view, bool useCorrelationKey = true)
            where T : class, IEditViewModel
        {
            return DeinitializeInsertAction<T>(model, null, methodSuccess, null, view, useCorrelationKey);
        }

        protected async Task<IActionResult> DeinitializeInsertActionAsync<T>(T model, Func<T, T, SubmitResult, Task<bool>> methodSuccess, string view, bool useCorrelationKey = true)
            where T : class, IEditViewModel
        {
            return await DeinitializeInsertActionAsync<T>(model, null, methodSuccess, null, view, useCorrelationKey);
        }

        protected IActionResult DeinitializeInsertAction<T>(T model, Action<T, T, SubmitResult> methodSuccess, Func<T, SubmitResult, IActionResult> methodFail, bool useCorrelationKey = true)
            where T : class, IEditViewModel
        {
            return DeinitializeInsertAction<T>(model, null, methodSuccess, methodFail, null, useCorrelationKey);
        }

        protected async Task<IActionResult> DeinitializeInsertActionAsync<T>(T model, Func<T, T, SubmitResult, Task<bool>> methodSuccess, Func<T, SubmitResult, Task<IActionResult>> methodFail, bool useCorrelationKey = true)
            where T : class, IEditViewModel
        {
            return await DeinitializeInsertActionAsync<T>(model, null, methodSuccess, methodFail, null, useCorrelationKey);
        }
        protected IActionResult DeinitializeInsertAction<T>(T model, Action<T, T, SubmitResult> methodSuccess, Func<T, SubmitResult, IActionResult> methodFail, string view, bool useCorrelationKey = true)
            where T : class, IEditViewModel
        {
            return DeinitializeInsertAction<T>(model, null, methodSuccess, methodFail, view, useCorrelationKey);
        }

        protected async Task<IActionResult> DeinitializeInsertActionAsync<T>(T model, Func<T, T, SubmitResult, Task<bool>> methodSuccess, Func<T, SubmitResult, Task<IActionResult>> methodFail, string view, bool useCorrelationKey = true)
            where T : class, IEditViewModel
        {
            return await DeinitializeInsertActionAsync<T>(model, null, methodSuccess, methodFail, view, useCorrelationKey);
        }

        protected IActionResult DeinitializeInsertAction<T>(T model, Func<T, T, bool> methodValidate, Action<T, T, SubmitResult> methodSuccess, bool useCorrelationKey = true)
            where T : class, IEditViewModel
        {
            return DeinitializeInsertAction<T>(model, methodValidate, methodSuccess, null, null, useCorrelationKey);
        }

        protected async Task<IActionResult> DeinitializeInsertActionAsync<T>(T model, Func<T, T, Task<bool>> methodValidate, Func<T, T, SubmitResult, Task<bool>> methodSuccess, bool useCorrelationKey = true)
            where T : class, IEditViewModel
        {
            return await DeinitializeInsertActionAsync<T>(model, methodValidate, methodSuccess, null, null, useCorrelationKey);
        }

        protected IActionResult DeinitializeInsertAction<T>(T model, Func<T, T, bool> methodValidate, Action<T, T, SubmitResult> methodSuccess, string view, bool useCorrelationKey = true)
            where T : class, IEditViewModel
        {
            return DeinitializeInsertAction<T>(model, methodValidate, methodSuccess, null, view, useCorrelationKey);
        }

        protected async Task<IActionResult> DeinitializeInsertActionAsync<T>(T model, Func<T, T, Task<bool>> methodValidate, Func<T, T, SubmitResult, Task<bool>> methodSuccess, string view, bool useCorrelationKey = true)
            where T : class, IEditViewModel
        {
            return await DeinitializeInsertActionAsync<T>(model, methodValidate, methodSuccess, null, view, useCorrelationKey);
        }

        protected IActionResult DeinitializeInsertAction<T>(T model, Func<T, T, bool> methodValidate, Action<T, T, SubmitResult> methodSuccess, Func<T, SubmitResult, IActionResult> methodFail, bool useCorrelationKey = true)
            where T : class, IEditViewModel
        {
            return DeinitializeInsertAction<T>(model, methodValidate, methodSuccess, methodFail, null, useCorrelationKey);
        }

        protected async Task<IActionResult> DeinitializeInsertActionAsync<T>(T model, Func<T, T, Task<bool>> methodValidate, Func<T, T, SubmitResult, Task<bool>> methodSuccess, Func<T, SubmitResult, Task<IActionResult>> methodFail, bool useCorrelationKey = true)
            where T : class, IEditViewModel
        {
            return await DeinitializeInsertActionAsync<T>(model, methodValidate, methodSuccess, methodFail, null, useCorrelationKey);
        }

        protected IActionResult DeinitializeInsertAction<T>(T model, Func<T, T, bool> methodValidate, Action<T, T, SubmitResult> methodSuccess, Func<T, SubmitResult, IActionResult> methodFail, string view, bool useCorrelationKey = true)
            where T : class, IEditViewModel
        {
            const string Declaration = "DeinitializeInsertAction";

            Enforce.AgainstNull<IEditViewModel>(() => model);

            try
            {
                SubmitResult results = new();

                T previousModel = null;
                if (useCorrelationKey)
                {
                    object value = TempData[Constants.CRUD.KeyCorrelation];
                    if (value == null)
                        throw new InvalidCorrelationKeyException(InvalidCorrelationKey);

                    Guid? correlationKey = (Guid)value;
                    if (!correlationKey.HasValue || (correlationKey == Guid.Empty))
                        throw new InvalidCorrelationKeyException(InvalidCorrelationKey);
                    if (correlationKey != model.CorrelationKey)
                        throw new InvalidCorrelationKeyMatchException(string.Format(InvalidCorrelationKeyFormat, correlationKey, model.CorrelationKey));

                    previousModel = (T)TempData[correlationKey.ToString()];
                }

                if (!string.IsNullOrEmpty(model.Action))
                {
                    if (model.Action.EqualsIgnore(ActionCancel))
                        results.IsCancel = true;

                    if (model.Action.EqualsIgnore(ActionSave))
                        results.IsSave = true;

                    DeinitializeRequestDetermineAction<T>(model, results);
                }

                if (results.IsCancel)
                    return JsonPost();

                if (results.IsSave)
                {
                    bool valid = true;
                    if (methodValidate != null)
                        valid = methodValidate(model, previousModel);

                    if (ModelState.IsValid && valid)
                    {
                        try
                        {
                            methodSuccess?.Invoke(model, previousModel, results);

                            if (results.Success)
                            {
                                DeinitializeInsertIActionResultSuccess();

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

                    return (!string.IsNullOrEmpty(view) ? View(view, model) : View(model));
                }

                return DeinitializeRequestHandleAction<T>(model, results);
            }
            catch (Exception ex)
            {
                Logger?.LogError(Declaration, ex);
                if (Request.IsAjaxRequest())
                    return JsonPostFailure();
                throw;
            }
        }

        protected async Task<IActionResult> DeinitializeInsertActionAsync<T>(T model, Func<T, T, Task<bool>> methodValidate, Func<T, T, SubmitResult, Task<bool>> methodSuccess, Func<T, SubmitResult, Task<IActionResult>> methodFail, string view, bool useCorrelationKey = true)
            where T : class, IEditViewModel
        {
            const string Declaration = "DeinitializeInsertAction";

            Enforce.AgainstNull<IEditViewModel>(() => model);

            try
            {
                SubmitResult results = new();

                T previousModel = null;
                if (useCorrelationKey)
                {
                    object value = TempData[Constants.CRUD.KeyCorrelation];
                    if (value == null)
                        throw new InvalidCorrelationKeyException(InvalidCorrelationKey);

                    Guid? correlationKey = (Guid)value;
                    if (!correlationKey.HasValue || (correlationKey == Guid.Empty))
                        throw new InvalidCorrelationKeyException(InvalidCorrelationKey);
                    if (correlationKey != model.CorrelationKey)
                        throw new InvalidCorrelationKeyMatchException(string.Format(InvalidCorrelationKeyFormat, correlationKey, model.CorrelationKey));

                    previousModel = (T)TempData[correlationKey.ToString()];
                }

                if (!string.IsNullOrEmpty(model.Action))
                {
                    if (model.Action.EqualsIgnore(ActionCancel))
                        results.IsCancel = true;

                    if (model.Action.EqualsIgnore(ActionSave))
                        results.IsSave = true;

                    await DeinitializeRequestDetermineActionAsync<T>(model, results);
                }

                if (results.IsCancel)
                    return await Task.FromResult(JsonPost());

                if (results.IsSave)
                {
                    bool valid = true;
                    if (methodValidate != null)
                        valid = await methodValidate(model, previousModel);

                    if (ModelState.IsValid && valid)
                    {
                        try
                        {
                            if (methodSuccess != null)
                                await methodSuccess(model, previousModel, results);

                            if (results.Success)
                            {
                                await DeinitializeInsertIActionResultSuccessAsync();

                                if (Request.IsAjaxRequest())
                                    return await Task.FromResult(JsonPost());

                                return await Task.FromResult(!string.IsNullOrEmpty(view) ? View(view, model) : View(model));
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
                        return await methodFail(model, results);

                    var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                    if (Request.IsAjaxRequest())
                        return JsonPostFailure(allErrors.Select(l => l.ErrorMessage));

                    foreach (var error in results.Errors)
                        ModelState.AddModelError(error.InputElement, error.Message);

                    return await Task.FromResult(!string.IsNullOrEmpty(view) ? View(view, model) : View(model));
                }

                return await DeinitializeRequestHandleActionAsync<T>(model, results);
            }
            catch (Exception ex)
            {
                Logger?.LogError(Declaration, ex);
                if (Request.IsAjaxRequest())
                    return await Task.FromResult(JsonPostFailure());
                throw;
            }
        }

        protected virtual void DeinitializeInsertActionSuccess() { }

        protected virtual async Task<bool> DeinitializeInsertActionSuccessAsync()
        {
            return await Task.FromResult(true);
        }

        protected IActionResult DeinitializeInsertIActionResult<T>(T model, Func<T, T, SubmitResult, IActionResult> methodSuccess, bool useCorrelationKey = true)
            where T : class, IEditViewModel
        {
            return DeinitializeInsertIActionResult<T>(model, null, methodSuccess, null, useCorrelationKey);
        }

        protected async Task<IActionResult> DeinitializeInsertIActionResultAsync<T>(T model, Func<T, T, SubmitResult, Task<IActionResult>> methodSuccess, bool useCorrelationKey = true)
            where T : class, IEditViewModel
        {
            return await DeinitializeInsertIActionResultAsync<T>(model, null, methodSuccess, null, useCorrelationKey);
        }

        protected IActionResult DeinitializeInsertIActionResult<T>(T model, Func<T, T, SubmitResult, IActionResult> methodSuccess, Func<T, SubmitResult, IActionResult> methodFail, bool useCorrelationKey = true)
            where T : class, IEditViewModel
        {
            return DeinitializeInsertIActionResult<T>(model, null, methodSuccess, methodFail, useCorrelationKey);
        }

        protected async Task<IActionResult> DeinitializeInsertIActionResultAsync<T>(T model, Func<T, T, SubmitResult, Task<IActionResult>> methodSuccess, Func<T, SubmitResult, Task<IActionResult>> methodFail, bool useCorrelationKey = true)
            where T : class, IEditViewModel
        {
            return await DeinitializeInsertIActionResultAsync<T>(model, null, methodSuccess, methodFail, useCorrelationKey);
        }

        protected IActionResult DeinitializeInsertIActionResult<T>(T model, Func<T, T, bool> methodValidate, Func<T, T, SubmitResult, IActionResult> methodSuccess, bool useCorrelationKey = true)
            where T : class, IEditViewModel
        {
            return DeinitializeInsertIActionResult<T>(model, methodValidate, methodSuccess, null, useCorrelationKey);
        }

        protected async Task<IActionResult> DeinitializeInsertIActionResultAsync<T>(T model, Func<T, T, Task<bool>> methodValidate, Func<T, T, SubmitResult, Task<IActionResult>> methodSuccess, bool useCorrelationKey = true)
            where T : class, IEditViewModel
        {
            return await DeinitializeInsertIActionResultAsync<T>(model, methodValidate, methodSuccess, null, useCorrelationKey);
        }

        protected IActionResult DeinitializeInsertIActionResult<T>(T model, Func<T, T, bool> methodValidate, Func<T, T, SubmitResult, IActionResult> methodSuccess, Func<T, SubmitResult, IActionResult> methodFail, bool useCorrelationKey = true)
            where T : class, IEditViewModel
        {
            const string Declaration = "DeinitializeInsertIActionResult";

            Enforce.AgainstNull<IEditViewModel>(() => model);
            Enforce.AgainstNull(() => methodSuccess);

            try
            {
                SubmitResult results = new();

                T previousModel = null;
                if (useCorrelationKey)
                {
                    object value = TempData[Constants.CRUD.KeyCorrelation];
                    if (value == null)
                        throw new InvalidCorrelationKeyException(InvalidCorrelationKey);

                    Guid? correlationKey = (Guid)value;
                    if (!correlationKey.HasValue || (correlationKey == Guid.Empty))
                        throw new InvalidCorrelationKeyException(InvalidCorrelationKey);

                    if (correlationKey != model.CorrelationKey)
                        throw new InvalidCorrelationKeyMatchException(string.Format(InvalidCorrelationKeyFormat, correlationKey, model.CorrelationKey));

                    previousModel = (T)TempData[correlationKey.ToString()];
                }

                if (!string.IsNullOrEmpty(model.Action))
                {
                    if (model.Action.EqualsIgnore(ActionCancel))
                        results.IsCancel = true;

                    if (model.Action.EqualsIgnore(ActionSave))
                        results.IsSave = true;

                    DeinitializeRequestDetermineAction<T>(model, results);
                }

                if (results.IsCancel)
                    return JsonPost();

                if (results.IsSave)
                {
                    bool valid = true;
                    if (methodValidate != null)
                        valid = methodValidate(model, previousModel);

                    if (ModelState.IsValid && valid)
                    {
                        try
                        {
                            IActionResult result = methodSuccess(model, previousModel, results);

                            DeinitializeInsertIActionResultSuccess();

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

                    if (methodFail != null)
                        return methodFail(model, results);

                    var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                    if (Request.IsAjaxRequest())
                        return JsonPostFailure(allErrors.Select(l => l.ErrorMessage));

                    return View(model);
                }

                return DeinitializeRequestHandleAction<T>(model, results);
            }
            catch (Exception ex)
            {
                Logger?.LogError(Declaration, ex);
                if (Request.IsAjaxRequest())
                    return JsonPostFailure();
                throw;
            }
        }

        protected async Task<IActionResult> DeinitializeInsertIActionResultAsync<T>(T model, Func<T, T, Task<bool>> methodValidate, Func<T, T, SubmitResult, Task<IActionResult>> methodSuccess, Func<T, SubmitResult, Task<IActionResult>> methodFail, bool useCorrelationKey = true)
            where T : class, IEditViewModel
        {
            const string Declaration = "DeinitializeInsertIActionResult";

            Enforce.AgainstNull<IEditViewModel>(() => model);
            Enforce.AgainstNull(() => methodSuccess);

            try
            {
                SubmitResult results = new();

                T previousModel = null;
                if (useCorrelationKey)
                {
                    object value = TempData[Constants.CRUD.KeyCorrelation];
                    if (value == null)
                        throw new InvalidCorrelationKeyException(InvalidCorrelationKey);

                    Guid? correlationKey = (Guid)value;
                    if (!correlationKey.HasValue || (correlationKey == Guid.Empty))
                        throw new InvalidCorrelationKeyException(InvalidCorrelationKey);
                    if (correlationKey != model.CorrelationKey)
                        throw new InvalidCorrelationKeyMatchException(string.Format(InvalidCorrelationKeyFormat, correlationKey, model.CorrelationKey));

                    previousModel = (T)TempData[correlationKey.ToString()];
                }

                if (!string.IsNullOrEmpty(model.Action))
                {
                    if (model.Action.EqualsIgnore(ActionCancel))
                        results.IsCancel = true;

                    if (model.Action.EqualsIgnore(ActionSave))
                        results.IsSave = true;

                    await DeinitializeRequestDetermineActionAsync<T>(model, results);
                }

                if (results.IsCancel)
                    return await Task.FromResult(JsonPost());

                if (results.IsSave)
                {
                    bool valid = true;
                    if (methodValidate != null)
                        valid = await methodValidate(model, previousModel);

                    if (ModelState.IsValid && valid)
                    {
                        try
                        {
                            IActionResult result = null;
                            if (methodSuccess != null)
                                result = await methodSuccess(model, previousModel, results);

                            DeinitializeInsertIActionResultSuccess();

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

                    if (methodFail != null)
                        return await methodFail(model, results);

                    var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                    if (Request.IsAjaxRequest())
                        return await Task.FromResult(JsonPostFailure(allErrors.Select(l => l.ErrorMessage)));

                    return await Task.FromResult(View(model));
                }

                return await DeinitializeRequestHandleActionAsync<T>(model, results);
            }
            catch (Exception ex)
            {
                Logger?.LogError(Declaration, ex);
                if (Request.IsAjaxRequest())
                    return await Task.FromResult(JsonPostFailure());
                throw;
            }
        }

        protected virtual void DeinitializeInsertIActionResultSuccess() { }

        protected virtual async Task<bool> DeinitializeInsertIActionResultSuccessAsync()
        {
            return await Task.FromResult(true);
        }

        protected IActionResult InitializeInsertAction<T>(T model, string view)
            where T : class, IEditViewModel
        {
            return InitializeInsertAction<T>(model, null, null, view);
        }

        protected async Task<IActionResult> InitializeInsertActionAsync<T>(T model, string view)
            where T : class, IEditViewModel
        {
            return await InitializeInsertActionAsync<T>(model, null, null, view);
        }

        protected IActionResult InitializeInsertAction<T>(T model, Action<T> methodLoad)
            where T : class, IEditViewModel
        {
            return InitializeInsertAction<T>(model, null, methodLoad, null);
        }

        protected async Task<IActionResult> InitializeInsertActionAsync<T>(T model, Func<T, Task<bool>> methodLoad)
            where T : class, IEditViewModel
        {
            return await InitializeInsertActionAsync<T>(model, null, methodLoad, null);
        }

        protected IActionResult InitializeInsertAction<T>(T model, Func<T, bool> methodValidate, Action<T> methodLoad)
            where T : class, IEditViewModel
        {
            return InitializeInsertAction<T>(model, methodValidate, methodLoad, null);
        }

        protected async Task<IActionResult> InitializeInsertActionAsync<T>(T model, Func<T, Task<bool>> methodValidate, Func<T, Task<bool>> methodLoad)
            where T : class, IEditViewModel
        {
            return await InitializeInsertActionAsync<T>(model, methodValidate, methodLoad, null);
        }

        protected IActionResult InitializeInsertAction<T>(T model, Action<T> methodLoad, string view)
            where T : class, IEditViewModel
        {
            return InitializeInsertAction<T>(model, null, methodLoad, view);
        }

        protected async Task<IActionResult> InitializeInsertActionAsync<T>(T model, Func<T, Task<bool>> methodLoad, string view)
            where T : class, IEditViewModel
        {
            return await InitializeInsertActionAsync<T>(model, null, methodLoad, view);
        }

        protected IActionResult InitializeInsertAction<T>(T model, Func<T, bool> methodValidate, Action<T> methodLoad, string view)
            where T : class, IEditViewModel
        {
            const string Declaration = "InitializeInsertAction";

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
                Logger?.LogError(Declaration, ex);
                throw;
            }
        }

        protected async Task<IActionResult> InitializeInsertActionAsync<T>(T model, Func<T, Task<bool>> methodValidate, Func<T, Task<bool>> methodLoad, string view)
            where T : class, IEditViewModel
        {
            const string Declaration = "InitializeInsertAction";

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

                if (methodValidate != null)
                {
                    if (!await methodValidate(model))
                    {
                        if (Request.IsAjaxRequest())
                            return await Task.FromResult(JsonGetFailure());

                        return await Task.FromResult(!string.IsNullOrEmpty(view) ? View(view, model) : View(model));
                    }
                }

                if (methodLoad != null)
                    await methodLoad(model);

                return await Task.FromResult(!string.IsNullOrEmpty(view) ? View(view, model) : View(model));
            }
            catch (Exception ex)
            {
                Logger?.LogError(Declaration, ex);
                throw;
            }
        }

        protected IActionResult InitializeInsertIActionResult<T>(T model, Func<T, IActionResult> methodValidate, Func<T, IActionResult> methodLoad)
            where T : class, IEditViewModel
        {
            const string Declaration = "InitializeInsertIActionResult";

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
                Logger?.LogError(Declaration, ex);
                throw;
            }
        }

        protected async Task<IActionResult> InitializeInsertIActionResultAsync<T>(T model, Func<T, Task<IActionResult>> methodValidate, Func<T, Task<IActionResult>> methodLoad)
            where T : class, IEditViewModel
        {
            const string Declaration = "InitializeInsertIActionResult";

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
                Logger?.LogError(Declaration, ex);
                throw;
            }
        }
        #endregion
    }
}