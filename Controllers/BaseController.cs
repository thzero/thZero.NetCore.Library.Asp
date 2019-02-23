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
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace thZero.AspNetCore.Mvc
{
	public abstract partial class BaseController : Controller
	{
		#region Protected Methods
		protected bool IsAjax()
		{
			return Request.IsAjaxRequest();
		}

        protected bool Validate(params bool[] values)
		{
			if (values == null)
				return false;

			return (!values.Any(l => l == false));
		}
		#endregion

		#region Constants
		protected const string InvalidCorrelationKey = "Invalid correlation key.";
		protected const string InvalidCorrelationKeyFormat = "Correlation key '{0}' does not match key '{1}'.";
		protected const string InvalidFailureResult = "This was not an ajax request and no IActionResult was supplied.";
		#endregion
	}

	public abstract partial class BaseController<TController> : BaseController
		where TController : BaseController
	{
		protected BaseController(ILogger<TController> logger)
		{
			Logger = logger;
		}

		#region Protected Properties
		protected ILogger<TController> Logger { get; }
		#endregion
	}
}