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
using System.IO;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace thZero.AspNetCore
{
	public abstract class SelfHostingMvcProgram<TProgram, TStartup>
		where TProgram : SelfHostingMvcProgram<TProgram, TStartup>
        where TStartup : BaseMvcStartup
	{
		#region Public Methods
		public static void Start(string[] args)
		{
			Utilities.Activator.CreateInstance<TProgram>()
				.Initialize(args);
		}

		public void Initialize(string[] args)
		{
			var configuration = InitializeConfigurationBuilder(
				new ConfigurationBuilder()
					.AddCommandLine(args)
				)
				.Build();

			var webHostBuilder = new WebHostBuilder()
					.UseConfiguration(configuration)
                    .ConfigureAppConfiguration((hostingContext, configurationBuilder) =>
                    {
                        configurationBuilder.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath);
                        InitializeConfigurationBuilder(hostingContext, configurationBuilder);
                    })
                    .ConfigureLogging((hostingContext, loggingBuilder) =>
                    {
                        InitializeConfigureLoggingBuilder(hostingContext, loggingBuilder);
                    })
					.UseStartup<TStartup>();

			webHostBuilder.UseKestrel(c => c.AddServerHeader = false);

			InitializeWebHostBuilder(webHostBuilder);
			InitializeContentRoot(webHostBuilder);

			webHostBuilder
				.Build()
				.Run();
		}
		#endregion

		#region Protected Methods
		protected virtual IConfigurationBuilder InitializeConfigurationBuilder(IConfigurationBuilder builder)
		{
			return builder;
        }

        protected virtual void InitializeConfigurationBuilder(WebHostBuilderContext hostingContext, IConfigurationBuilder configurationBuilder)
        {
        }

        protected virtual void InitializeConfigureLoggingBuilder(WebHostBuilderContext hostingContext, ILoggingBuilder loggingBuilder)
        {
        }

        protected virtual IWebHostBuilder InitializeWebHostBuilder(IWebHostBuilder builder)
		{
			return builder;
		}

		protected virtual void InitializeContentRoot(IWebHostBuilder builder)
		{
			builder.UseContentRoot(Directory.GetCurrentDirectory());
		}
		#endregion
	}
}