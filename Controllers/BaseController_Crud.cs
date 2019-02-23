/* ------------------------------------------------------------------------- *
thZero.NetCore.Library.Asp
Copyright (C) 2016-2019 thZero.com

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
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using thZero.AspNetCore.Mvc.Views.Models;
using thZero.AspNetCore.Results;

namespace thZero.AspNetCore.Mvc
{
	public abstract partial class BaseController<TController>
	{
		#region Protected Methods
		protected virtual void DeinitializeRequestDetermineAction<T>(T model, SubmitResult result)
			where T : class, IEditViewModel
		{
		}

		protected virtual async Task<bool> DeinitializeRequestDetermineActionAsync<T>(T model, SubmitResult result)
			where T : class, IEditViewModel
		{
			return await Task.FromResult(true);
		}

		protected virtual IActionResult DeinitializeRequestHandleAction<T>(T model, SubmitResult result)
			where T : class, IEditViewModel
		{
			return View(model);
		}

		protected virtual async Task<IActionResult> DeinitializeRequestHandleActionAsync<T>(T model, SubmitResult result)
			where T : class, IEditViewModel
		{
			return await Task.FromResult(View(model));
		}

		protected IActionResult InitializeAction<T>(T model, Action<T> methodLoad, string view)
			where T : RequestViewModel
		{
			return InitializeAction<T>(model, null, methodLoad, view);
		}

		protected IActionResult InitializeAction<T>(T model, Func<T, bool> methodValidate, Action<T> methodLoad, string view)
			where T : RequestViewModel
		{
			const string Declaration = "InitializeAction";

			Enforce.AgainstNull<RequestViewModel>(() => model);

			try
			{
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

		protected async Task<IActionResult> InitializeActionAsync<T>(T model, Func<T, Task<bool>> methodLoad, string view)
			where T : RequestViewModel
		{
			return await InitializeActionAsync<T>(model, null, methodLoad, view);
		}

		protected async Task<IActionResult> InitializeActionAsync<T>(T model, Func<T, Task<bool>> methodValidate, Func<T, Task<bool>> methodLoad, string view)
			where T : RequestViewModel
		{
			const string Declaration = "InitializeAction";

			Enforce.AgainstNull<RequestViewModel>(() => model);

			try
			{
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

		protected IActionResult InitializeIActionResult<T>(T model, Func<T, IActionResult> methodLoad)
			where T : RequestViewModel
		{
			return InitializeIActionResult<T>(model, (Func<T, bool>)null, methodLoad);
		}

		protected IActionResult InitializeIActionResult<T>(T model, Func<T, bool> methodValidate, Func<T, IActionResult> methodLoad)
			where T : RequestViewModel
		{
			const string Declaration = "InitializeIActionResult";

			Enforce.AgainstNull<RequestViewModel>(() => model);
			Enforce.AgainstNull(() => methodLoad);

			try
			{
				if ((methodValidate != null) && !methodValidate(model))
					throw new InvalidFailureException();

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

		protected async Task<IActionResult> InitializeIActionResultAsync<T>(T model, Func<T, Task<IActionResult>> methodLoad)
			where T : RequestViewModel
		{
			return await InitializeIActionResultAsync<T>(model, (Func<T, Task<bool>>)null, methodLoad);
		}

		protected async Task<IActionResult> InitializeIActionResultAsync<T>(T model, Func<T, Task<bool>> methodValidate, Func<T, Task<IActionResult>> methodLoad)
			where T : RequestViewModel
		{
			const string Declaration = "InitializeIActionResult";

			Enforce.AgainstNull<RequestViewModel>(() => model);
			Enforce.AgainstNull(() => methodLoad);

			try
			{
				if ((methodValidate != null) && !await methodValidate(model))
					throw new InvalidFailureException();

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

		protected IActionResult InitializeIActionResult<T>(T model, Func<T, IActionResult> methodValidate, Func<T, IActionResult> methodLoad)
			where T : RequestViewModel
		{
			const string Declaration = "InitializeIActionResult";

			Enforce.AgainstNull<RequestViewModel>(() => model);
			Enforce.AgainstNull(() => methodLoad);

			try
			{
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

		protected async Task<IActionResult> InitializeIActionResultAsync<T>(T model, Func<T, Task<IActionResult>> methodValidate, Func<T, Task<IActionResult>> methodLoad)
			where T : RequestViewModel
		{
			const string Declaration = "InitializeIActionResult";

			Enforce.AgainstNull<RequestViewModel>(() => model);
			Enforce.AgainstNull(() => methodLoad);

			try
			{
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

		#region Constants
		protected const string ActionCancel = "cancel";
        protected const string ActionDelete = "delete";
        protected const string ActionSave = "save";
		#endregion
	}
}