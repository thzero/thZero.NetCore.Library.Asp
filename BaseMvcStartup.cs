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
using System.Collections.Generic;
using System.Globalization;
using System.IO.Compression;
using System.Linq;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Routing;
#if !DEBUG
using Microsoft.AspNetCore.Rewrite;
#endif
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace thZero.AspNetCore
{
	public abstract class BaseMvcStartup
	{
		protected BaseMvcStartup(IHostingEnvironment env)
		{
		}

		#region Public Methods
		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		// This gets called after ConfigureServices.
		// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup
		public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider svp)
		{
#if NETSTANDARD2_0
			Utilities.Services.Version.Instance.Version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
#else
			var version = Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationVersion;
			Utilities.Services.Web.Version.Instance.Version = new Version(version);
#endif

			Utilities.Services.Web.ServiceProvider.Instance = svp;
			Utilities.Web.Environment.IsDevelopment = env.IsDevelopment();
			Utilities.Web.Environment.IsProduction = env.IsProduction();
			Utilities.Web.Environment.IsStaging = env.IsStaging();

            ConfigureInitializeServiceProvider(svp);
            ConfigureInitializeLoggerFactory(loggerFactory);

            if (RequiresSsl)
            {
                app.UseHsts(opts => opts.AllResponses());
                ConfigureInitializeSsl(app, env);
            }

            _useCompression = ConfigureInitializeCompression(app, env);

            ConfigureInitialize(app, env, loggerFactory, svp);

            ConfigureInitializeStaticPre(app, env);
            ConfigureInitializeStatic(app, env);
            ConfigureInitializeStaticPost(app, env);

            var defaultCultureName = "en";
			var defaultCulture = new CultureInfo(defaultCultureName);
			var config = Utilities.Web.Configuration.Application;
			IList<CultureInfo> supportedCultures = new List<CultureInfo>();
			CultureInfo info = null;
			foreach (var item in config.Cultures)
			{
				info = new CultureInfo(item.Abbreviation);
				supportedCultures.Add(info);
				if (item.Default)
					defaultCulture = info;
			}
			if (supportedCultures.Count == 0)
				supportedCultures.Add(defaultCulture);

			app.UseRequestLocalization(new RequestLocalizationOptions
			{
				DefaultRequestCulture = new RequestCulture(defaultCultureName),
				// Formatting numbers, dates, etc.
				SupportedCultures = supportedCultures,
				// UI strings that we have localized.
				SupportedUICultures = supportedCultures
			});

			app.UseStatusCodePagesWithReExecute("/errors/{0}");

			ConfigureInitializeRoutes(app);

            ConfigureInitializeFinal(app, env, loggerFactory, svp);
        }

		// This method gets called by the runtime. Use this method to add services to the container.
		// This gets called before Configure.
		// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup
		public virtual void ConfigureServices(IServiceCollection services)
		{
			ServiceDescriptor envDescriptor = services.Where(l => l.ServiceType.Equals(typeof(IHostingEnvironment))).FirstOrDefault();
			Enforce.AgainstNull(() => envDescriptor);

			IHostingEnvironment env = (IHostingEnvironment)envDescriptor.ImplementationInstance;

			// Doing some initialization and the ConfigurationBuilder here so that
			// we do not have virtual methods in the constructor.
			ConfigurationBuilder builder = new ConfigurationBuilder();
            ConfigureServicesInitializeBuilder(env, builder);
			Configuration = builder.Build();

			ConfigureServicesInitializeMvcPre(services);

			services.AddMvc();

			ConfigureServicesInitializeMvcPost(services);

			// Only if UseCompression middleware was enabled on the IApplicationBuilder...
			if (_useCompression)
			{
				ConfigureServicesInitializeCompression(services);
				ConfigureServicesInitializeCompressionOptions(services);
			}
		}
		#endregion

		#region Protected Methods
		protected virtual void ConfigureInitialize(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider svp)
		{
			if (env.IsDevelopment())
				ConfigureInitializeDebug(app, env, loggerFactory, svp);
			else
				ConfigureInitializeProduction(app, env, loggerFactory, svp);
        }

        protected virtual bool ConfigureInitializeCompression(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseResponseCompression();
            return true;
        }

        protected virtual void ConfigureInitializeDebug(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider svp)
		{
			app.UseDeveloperExceptionPage();
        }

        protected virtual void ConfigureInitializeFinal(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider svp)
        {
        }

        protected virtual void ConfigureInitializeLoggerFactory(ILoggerFactory loggerFactory)
        {
        }

        protected abstract void ConfigureInitializeProduction(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider svp);

		protected virtual void ConfigureInitializeRoutes(IApplicationBuilder app)
		{
			app.UseMvc(routes =>
			{
				ConfigureInitializeRoutesBuilder(routes);
			});
		}

		protected virtual void ConfigureInitializeRoutesBuilder(IRouteBuilder routes)
		{
		}

		protected virtual void ConfigureInitializeServiceProvider(IServiceProvider svp)
		{
        }

        protected virtual void ConfigureInitializeSsl(IApplicationBuilder app, IHostingEnvironment env)
        {
#if !DEBUG
			var options = new RewriteOptions()
				//.AddRedirectToHttps();
				.AddRedirectToHttpsPermanent();

			app.UseRewriter(options);
#endif
        }

        protected virtual void ConfigureInitializeStatic(IApplicationBuilder app, IHostingEnvironment env)
        {
        }

        protected virtual void ConfigureInitializeStaticPre(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseXContentTypeOptions();
            app.UseReferrerPolicy(opts => opts.NoReferrer());
            app.UseXDownloadOptions();
            app.UseXXssProtection(opts => opts.Enabled());
        }

        protected virtual void ConfigureInitializeStaticPost(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseNoCacheHttpHeaders();
        }

        protected virtual void ConfigureServicesInitializeBuilder(IHostingEnvironment env, ConfigurationBuilder builder)
        {
            builder.SetBasePath(env.ContentRootPath);
        }

        protected virtual void ConfigureServicesInitializeMvcPost(IServiceCollection services)
		{
		}

		protected virtual void ConfigureServicesInitializeMvcPre(IServiceCollection services)
		{
			ServiceCollection = services;
			services.AddResponseCompression();
		}

		protected virtual void ConfigureServicesInitializeCompression(IServiceCollection services)
		{
			services.AddResponseCompression(options =>
			{
				options.Providers.Add<GzipCompressionProvider>();
			});
		}

		protected virtual void ConfigureServicesInitializeCompressionOptions(IServiceCollection services)
		{
			services.Configure<GzipCompressionProviderOptions>(options =>
			{
				options.Level = CompressionLevel.Fastest;
			});
		}
		#endregion

		#region Protected Properties
		protected IConfigurationRoot Configuration { get; set; }
        protected abstract bool RequiresSsl { get; }
        protected static IServiceCollection ServiceCollection { get; private set; }
        #endregion

        #region Fields
        private bool _useCompression = false;
		#endregion
	}
}