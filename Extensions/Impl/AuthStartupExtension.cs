/* ------------------------------------------------------------------------- *
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
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace thZero.AspNetCore
{
    public abstract class AuthStartupExtension<TConfiguration> : BaseStartupExtension
        where TConfiguration : AuthorizationConfiguration
    {
        #region Public Methods
        /// <summary>
        /// Set the environment variable GOOGLE_APPLICATION_CREDENTIALS with location of the Firebase Admin secret key config.
        /// </summary>
        public override void ConfigureServicesPre(IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration)
        {
            IConfigurationSection config = GetConfigurationSection(configuration);
            if (config == null)
                throw new Exception("Invalid Authorization config.");

            services.Configure<TConfiguration>(config);
        }

        public override void ConfigureInitializeAuthentication(IApplicationBuilder app)
        {
            app.UseAuthentication();
        }

        public override void ConfigureInitializeAuthorization(IApplicationBuilder app)
        {
            app.UseAuthorization();
        }
        #endregion

        #region Protected Methods
        protected virtual IConfigurationSection GetConfigurationSection(IConfiguration configuration)
        {
            return configuration.GetSection("Authorization")?.GetSection(ConfigurationSectionKey);
        }
        #endregion

        #region Protected Properties
        protected abstract string ConfigurationSectionKey { get; }
        #endregion
    }

    public abstract class AuthorizationConfiguration
    {
    }
}
