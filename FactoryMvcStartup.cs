﻿/* ------------------------------------------------------------------------- *
thZero.NetCore.Library.Asp
Copyright (C) 2016-2022 thZero.com

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

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

using thZero.Configuration;
using thZero.Services;

namespace thZero.AspNetCore
{
    [Obsolete("FactoryMvcStartup is deprecated, please use FactoryStartupExtension instead.")]
    public abstract class FactoryMvcStartup<TApplicationConfiguration, TApplicationConfigurationDefaults, TApplicationConfigurationEmail, TStartup> :
        ConfigurableMvcStartup<TApplicationConfiguration, TApplicationConfigurationDefaults, TApplicationConfigurationEmail, TStartup>
        where TApplicationConfiguration : Application<TApplicationConfigurationDefaults, TApplicationConfigurationEmail>, new()
        where TApplicationConfigurationDefaults : ApplicationDefaults
        where TApplicationConfigurationEmail : ApplicationEmail
        where TStartup : BaseMvcStartup
    {
        public FactoryMvcStartup(string copyrightDate, IConfiguration configuration, ILogger<TStartup> logger) : base(copyrightDate, configuration, logger)
        {
        }

        #region Public Methods
        public override void ConfigureServices(IServiceCollection services)
        {
            InitializeFactory();

            base.ConfigureServices(services);
        }
        #endregion

        #region Protected Methods
        protected override void ConfigureInitialize(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IServiceProvider svp)
        {
            base.ConfigureInitialize(app, env, loggerFactory, svp);

            ConfigureInitializeFactoryLoggerFactory(svp, loggerFactory);
            ConfigureInitializeFactory(svp);
        }

        protected virtual void ConfigureInitializeFactory(IServiceProvider svp)
        {
        }

        protected virtual void ConfigureInitializeFactoryLoggerFactory(IServiceProvider svp, ILoggerFactory loggerFactory)
        {
            Factory.Instance.AddSingleton<ILoggerFactory>(loggerFactory);
        }

        protected override void ConfigureServicesInitializeMvcPost(IServiceCollection services)
        {
            base.ConfigureServicesInitializeMvcPost(services);

            Factory.Instance.AddSingleton<IServiceVersionInformation, ServiceVersionInformation>();

            ConfigureServicesInitializeFactory(services);
        }

        protected virtual void ConfigureServicesInitializeFactory(IServiceCollection services)
        {
        }
        #endregion

        #region Private Methods
        private void InitializeFactory()
        {
            if (_initialized)
                return;

            lock (_lock)
            {
                if (_initialized)
                    return;

                Factory.InitializeByAttribute(GetType());

                _initialized = true;
            }
        }
        #endregion

        #region Fields
        private static volatile bool _initialized = false;
        private static readonly object _lock = new();
        #endregion
    }
}