/* ------------------------------------------------------------------------- *
thZero.NetCore.Library.Asp
Copyright (C) 2021-2022 thZero.com

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

using Microsoft.Extensions.Logging;

namespace thZero.AspNetCore.Services
{
    public abstract class IntervalBackgroundService<TService> : BackgroundService<TService>, IDisposable
    {
        public IntervalBackgroundService(ILogger<TService> logger) : base(logger)
        {
        }

        #region Public Methods
        public void Dispose()
        {
            _timer?.Dispose();
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Protected Methods
        protected async override Task StartAsyncI(CancellationToken cancellationToken)
        {
            const string Declaration = "StartAsyncI";

            int heartbeatInterval = HeartbeatInterval;
            _timer = new Timer(o => {
                Task.Run(async () => {
                    try
                    {
                        await RunAsync(cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError2(Declaration, ex);
                    }
                }).Wait(cancellationToken);
            },
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(heartbeatInterval));

            await Task.FromResult<object>(null);
        }

        protected abstract Task RunAsync(CancellationToken cancellationToken);
        #endregion

        #region Protected Properties
        protected abstract int HeartbeatInterval { get; }
        #endregion

        #region Fields
        private Timer _timer;
        #endregion
    }
}
