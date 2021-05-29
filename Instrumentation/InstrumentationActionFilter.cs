/* ------------------------------------------------------------------------- *
thZero.NetCore.Library
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

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

using thZero.Instrumentation;

namespace thZero.AspNetCore.Filters.Instrumentation
{
    public class InstrumentationActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            IInstrumentationPacket packet = context.HttpContext.RequestServices.GetService<IInstrumentationPacket>();
            if (packet == null)
                return;

            string correlationId = context.HttpContext.Request.Headers["CorrelationId"];
            if (!String.IsNullOrEmpty(correlationId))
            {
                try
                {
                    packet.Correlation = Guid.Parse(correlationId);
                }
                catch (Exception) { }
            }

            if (String.IsNullOrEmpty(correlationId))
                packet.Correlation = Guid.NewGuid();

            if (!context.HttpContext.Items.ContainsKey(KeyInstrumentation))
                context.HttpContext.Items.Add(KeyInstrumentation, packet);
            else
                context.HttpContext.Items[KeyInstrumentation] = packet;
        }

        public const string KeyInstrumentation = "Instrumentation";
    }
}
