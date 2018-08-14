/* ------------------------------------------------------------------------- *
thZero.NetCore.Library.Asp
Copyright (C) 2016-2018 thZero.com

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

namespace thZero.AspNetCore.Mvc
{
	public abstract partial class BaseController<TController>
	{
		#region Protected Methods
		protected IActionResult InitializeJsonGetResult<T>(T model, Func<T, IActionResult> methodLoad)
			where T : RequestViewModel
		{
			return InitializeJsonGetResult(model, null, methodLoad);
		}

		protected IActionResult InitializeJsonGetResult<T>(T model, Func<T, bool> methodValidate, Func<T, IActionResult> methodLoad)
			where T : RequestViewModel
		{
			const string Declaration = "InitializeJsonGetResult";

			Enforce.AgainstNull<RequestViewModel>(() => model);
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
				Logger.LogError(Declaration, ex);
				throw;
			}
		}

		protected async Task<IActionResult> InitializeJsonGetResultAsync<T>(T model, Func<T, Task<IActionResult>> methodLoad)
			where T : RequestViewModel
		{
			return await InitializeJsonGetResultAsync<T>(model, null, methodLoad);
		}

		protected async Task<IActionResult> InitializeJsonGetResultAsync<T>(T model, Func<T, Task<bool>> methodValidate, Func<T, Task<IActionResult>> methodLoad)
			where T : RequestViewModel
		{
			const string Declaration = "InitializeJsonGetResult";

			Enforce.AgainstNull<RequestViewModel>(() => model);
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
				Logger.LogError(Declaration, ex);
				throw;
			}
		}
		#endregion
	}

	public abstract partial class BaseController
	{
		#region Protected Methods
		protected JsonResult JsonGet()
		{
			return JsonGet(null);
		}

		protected JsonResult JsonGet(object data)
		{
			return new JsonResultEx(new JsonOutputResponseViewModel<object>() { Success = true, Data = data });
		}

		protected JsonResult JsonGet<T>(T data)
			where T : class
		{
			return new JsonResultEx(new JsonOutputResponseViewModel<T>() { Success = true, Data = data });
		}

		public virtual JsonResult JsonGetFailure(string message)
		{
			return new JsonResultEx((new JsonOutputErrorResponseViewModel()).AddError(message));
		}

		public virtual JsonResult JsonGetFailure()
		{
			return new JsonResultEx(new JsonOutputErrorResponseViewModel());
		}

		public virtual JsonResult JsonGetSelect(IEnumerable<SelectListItem> list)
		{
			return new JsonResultEx(new JsonOutputSearchResponseViewModel<SelectListItem>() { Success = true, Data = list });
		}

		protected JsonResult JsonPost()
		{
			return JsonPost(null);
		}

		protected JsonResult JsonPost(object data)
		{
			return new JsonResultEx(new JsonOutputResponseViewModel<object>() { Success = true, Data = data });
		}

		protected JsonResult JsonPost<T>(T data)
			where T : class
		{
			return new JsonResultEx(new JsonOutputResponseViewModel<T>() { Success = true, Data = data });
		}

		public virtual JsonResult JsonPostFailure()
		{
			return new JsonResultEx(new JsonOutputErrorResponseViewModel());
		}

		public virtual JsonResult JsonPostFailure(IEnumerable<string> errors)
		{
			return new JsonResultEx((new JsonOutputErrorResponseViewModel()).AddErrors(errors));
		}

		public virtual JsonResult JsonPostFailure(string message)
		{
			return new JsonResultEx((new JsonOutputErrorResponseViewModel()).AddError(message));
		}

		public virtual JsonResult JsonPostFailure<T>(T data)
			where T : class
		{
			return new JsonResultEx(new JsonOutputResponseViewModel<T>() { Success = false, Data = data });
		}

		public virtual JsonResult JsonPostFailure<T>(T data, string message)
			where T : class
		{
			return new JsonResultEx((new JsonOutputResponseViewModel<T>() { Success = false, Data = data }).AddError(message));
		}
		#endregion
	}
}