/* ------------------------------------------------------------------------- *
thZero.NetCore.Library.Asp
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
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace thZero.AspNetCore.Services
{
    public abstract class BackgroundService<TService> : IHostedService
    {
        public BackgroundService(ILogger<TService> logger)
        {
            Logger = logger;
        }

        #region Public Methods
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Running = true;
            await StartAsyncI(cancellationToken);
        }

        public async virtual Task StopAsync(CancellationToken cancellationToken)
        {
            Running = false;
            await StopAsyncI(cancellationToken);
        }
        #endregion

        #region Protected Methods
        protected abstract Task StartAsyncI(CancellationToken cancellationToken);
        protected async virtual Task StopAsyncI(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
        #endregion

        #region Protected Properties
        protected ILogger<TService> Logger { get; private set; }
        protected bool Running { get; private set; }
        #endregion
    }
}
