/* ------------------------------------------------------------------------- *
thZero.NetCore.Library.Asp
Copyright (C) 2016-2018 thZero.com

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

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using thZero.Configuration;

namespace thZero.AspNetCore
{
	public abstract class ConfigurableMvcStartup<TApplicationConfiguration, TApplicationConfigurationDefaults, TApplicationConfigurationEmail> : 
		BaseMvcStartup
		where TApplicationConfiguration : Application<TApplicationConfigurationDefaults, TApplicationConfigurationEmail>, new()
		where TApplicationConfigurationDefaults : ApplicationDefaults
		where TApplicationConfigurationEmail : ApplicationEmail
	{
		public ConfigurableMvcStartup(IHostingEnvironment env, string copyrightDate) : base(env)
		{
			Utilities.Web.General.CopyrightDate = copyrightDate;
		}

		#region Protected Methods
		protected override void ConfigureInitializeServiceProvider(IServiceProvider svp)
		{
			base.ConfigureInitializeServiceProvider(svp);

			var site = svp.GetService<IOptions<TApplicationConfiguration>>();
			if ((site == null) || (site.Value == null))
				throw new Exception("Invalid Application configuration.");

			Utilities.Web.Configuration.Application = site.Value;
		}

		protected override void ConfigureServicesInitializeMvcPost(IServiceCollection services)
		{
			base.ConfigureServicesInitializeMvcPost(services);

			// Configuration Options...
			services.Configure<TApplicationConfiguration>(Configuration.GetSection("Application"));
		}
		#endregion
	}
}