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
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace thZero.AspNetCore.Mvc
{
	public abstract class ErrorBaseController : BaseController
	{
		#region Public Methods
		[HttpGet]
		[Route("errors")]
		public async Task<ActionResult> Index()
		{
			return await Task.FromResult(new StatusCodeResult(500));
		}

		[HttpGet]
		[Route("errors/{code}")]
		public virtual IActionResult Index(string code)
		{
			IActionResult result = HandleIndex(code);
			if (IsAjax())
				return result;

			return result;
		}
		#endregion

		#region Protected Methods
		protected virtual IActionResult HandleIndex(string code)
		{
#if DEBUG

			string requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
			ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
#endif
			HttpStatusCode result = HttpStatusCode.InternalServerError;
			if (!Enum.TryParse<HttpStatusCode>(code, out result))
				return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
			return new StatusCodeResult((int)result);
		}
#endregion
	}
}