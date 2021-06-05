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
    public abstract class BackgroundService<TService> : IHostedService, IDisposable
    {
        public BackgroundService(ILogger<TService> logger)
        {
            _logger = logger;
        }

        #region Public Methods
        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            const string Declaration = "StartAsync";

            Running = true;

            int heartbeatInterval = HeartbeatInterval;
            _timer = new Timer(o => {
                Task.Run(async () => {
                    try
                    {
                        await Run();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError2(Declaration, ex);
                    }
                }).Wait(cancellationToken);
            },
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(heartbeatInterval));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Running = false;
            return Task.CompletedTask;
        }
        #endregion

        #region Protected Methods
        protected abstract Task Run();
        #endregion

        #region Protected Properties
        protected abstract int HeartbeatInterval { get; }
        protected bool Running { get; private set; }
        #endregion

        #region Fields
        private readonly ILogger<TService> _logger;
        private Timer _timer;
        #endregion
    }
}
