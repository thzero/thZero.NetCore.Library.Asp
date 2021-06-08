/* ------------------------------------------------------------------------- *
thZero.Registry
Copyright (C) 2021-2021 thZero.com

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

using thZero.AspNetCore.Filters.Instrumentation;
using thZero.Instrumentation;
using thZero.Responses;

namespace thZero.AspNetCore.Mvc
{
    public abstract partial class BaseController : Controller
    {
        #region Protected Methods
        protected bool IsAjax()
        {
            return Request.IsAjaxRequest();
        }
        protected ErrorResponse Error()
        {
            return new ErrorResponse();
        }

        protected ErrorResponse Error(string message, params object[] args)
        {
            ErrorResponse error = new();
            error.AddError(message, args);
            return error;
        }

        protected TResult Error<TResult>(TResult result)
             where TResult : SuccessResponse
        {
            result.Success = false;
            return result;
        }

        protected TResult Error<TResult>(TResult result, string message, params object[] args)
             where TResult : SuccessResponse
        {
            result.AddError(message, args);
            result.Success = false;
            return result;
        }

        protected bool IsSuccess(SuccessResponse response)
        {
            return (response != null) && response.Success;
        }

        protected SuccessResponse Success()
        {
            return new SuccessResponse();
        }

        protected SuccessResponse Success(bool success)
        {
            return new SuccessResponse(success);
        }

        protected bool Validate(params bool[] values)
        {
            if (values == null)
                return false;

            return (!values.Any(l => l == false));
        }
        #endregion

        #region Protected Properties
        protected IInstrumentationPacket Instrumentation
        {
            get
            {
                return (IInstrumentationPacket)HttpContext.Items[InstrumentationActionFilter.KeyInstrumentation];
            }
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