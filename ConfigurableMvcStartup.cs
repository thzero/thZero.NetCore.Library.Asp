/* ------------------------------------------------------------------------- *
thZero.Registry
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

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

using thZero.Configuration;

namespace thZero.AspNetCore
{
    public abstract class ConfigurableMvcStartup<TApplicationConfiguration, TApplicationConfigurationDefaults, TApplicationConfigurationEmail, TStartup> :
        LoggableMvcStartup<TStartup>
        where TApplicationConfiguration : Application<TApplicationConfigurationDefaults, TApplicationConfigurationEmail>, new()
        where TApplicationConfigurationDefaults : ApplicationDefaults
        where TApplicationConfigurationEmail : ApplicationEmail
        where TStartup : BaseMvcStartup
    {
        public ConfigurableMvcStartup(string copyrightDate, IConfiguration configuration, ILogger<TStartup> logger) : base(configuration, logger)
        {
            Utilities.Web.General.CopyrightDate = copyrightDate;
        }

        #region Protected Methods
        protected override void ConfigureServicesConfiguration(IServiceCollection services)
        {
            // Configuration Options...
            services.Configure<TApplicationConfiguration>(Configuration.GetSection("Application"));
        }

        protected override void ConfigureInitializeServiceProvider(IServiceProvider svp)
        {
            base.ConfigureInitializeServiceProvider(svp);

            Utilities.Web.Configuration.Application = ConfigurationOptions(svp);
        }

        //protected override void ConfigureServicesInitializeMvcPost(IServiceCollection services)
        //{
        //    base.ConfigureServicesInitializeMvcPost(services);

        //    // Configuration Options...
        //    services.Configure<TApplicationConfiguration>(Configuration.GetSection("Application"));
        //}

        protected TApplicationConfiguration ConfigurationOptions(IServiceProvider svp)
        {
            var site = svp.GetService<IOptions<TApplicationConfiguration>>();
            if ((site == null) || (site.Value == null))
                throw new InvalidApplicationConfigurationException();
            return site.Value;
        }
        #endregion

        protected TApplicationConfiguration AppConfiguration { get { return (TApplicationConfiguration)Utilities.Web.Configuration.Application; } }
    }

    public class InvalidApplicationConfigurationException : Exception
    {
        public InvalidApplicationConfigurationException() : base("Invalid Application configuration.")
        {
        }
    }
}