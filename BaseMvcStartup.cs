/* ------------------------------------------------------------------------- *
thZero.Registry
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
using System.Collections.Generic;
using System.Globalization;
using System.IO.Compression;
using System.Linq;

using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
#if !DEBUG
using Microsoft.AspNetCore.Rewrite;
#endif
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using thZero.AspNetCore.Filters.Instrumentation;
using thZero.Instrumentation;
using thZero.Services;

namespace thZero.AspNetCore
{
    public abstract class BaseMvcStartup
    {
    }

    public abstract class LoggableMvcStartup<TStartup> : BaseMvcStartup
        where TStartup : BaseMvcStartup
    {
        protected LoggableMvcStartup(IConfiguration configuration, ILogger<TStartup> logger)
        {
            Configuration = configuration;
            Logger = logger;

        }

        #region Public Methods
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // This gets called after ConfigureServices.
        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup
        public virtual void Configure(IApplicationBuilder app, IHostApplicationLifetime lifetime, IWebHostEnvironment env, ILoggerFactory loggerFactory, IServiceProvider svp)
        {
            Utilities.Services.Version.Instance.Version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;

            lifetime.ApplicationStopping.Register(OnShutdown);

            IServiceVersionInformation serviceVersionInformation = svp.GetService<IServiceVersionInformation>();
            serviceVersionInformation.Version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;

            Utilities.ServiceProvider.Instance = svp;
            Utilities.Web.Environment.IsDevelopment = env.IsDevelopment();
            Utilities.Web.Environment.IsProduction = env.IsProduction();
            Utilities.Web.Environment.IsStaging = env.IsStaging();

            ConfigureInitializeServiceProvider(svp);

            if (StartupExtensions != null)
                StartupExtensions.ToList().ForEach(l => l.ConfigurePre(app, env, svp));

            if (RequiresSsl)
            {
                if (StartupExtensions != null)
                    StartupExtensions.ToList().ForEach(l => l.ConfigureInitializeSsl(app));

                ConfigureInitializeSsl(app, env);
            }

            if (_useCompression)
                ConfigureInitializeCompression(app, env);

            if (StartupExtensions != null)
                StartupExtensions.ToList().ForEach(l => l.ConfigureInitializePre(app, env, loggerFactory, svp));

            ConfigureInitialize(app, env, loggerFactory, svp);

            if (StartupExtensions != null)
                StartupExtensions.ToList().ForEach(l => l.ConfigureInitializePost(app, env, loggerFactory, svp));

            if (StartupExtensions != null)
                StartupExtensions.ToList().ForEach(l => l.ConfigureInitializeStaticPre(app, env, svp));

            ConfigureInitializeStaticPre(app, env, svp);

            ConfigureInitializeStatic(app, env, svp);

            if (StartupExtensions != null)
                StartupExtensions.ToList().ForEach(l => l.ConfigureInitializeStaticPost(app, env, svp));

            ConfigureInitializeStaticPost(app, env, svp);

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

            ConfigureInitializeRouting(app);

            if (StartupExtensions != null)
                StartupExtensions.ToList().ForEach(l => l.ConfigureInitializeFinalPre(app, env, svp));

            ConfigureInitializeFinal(app, env, svp);

            if (StartupExtensions != null)
                StartupExtensions.ToList().ForEach(l => l.ConfigureInitializeFinalPost(app, env, svp));

            if (StartupExtensions != null)
                StartupExtensions.ToList().ForEach(l => l.ConfigurePost(app, env, svp));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // This gets called before Configure.
        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup
        public virtual void ConfigureServices(IServiceCollection services)
        {
            ConfigureServicesConfiguration(services);

            RegisterStartupExtensions();

            ServiceDescriptor envDescriptor = services.Where(l => l.ServiceType.Equals(typeof(IWebHostEnvironment))).FirstOrDefault();
            Enforce.AgainstNull(() => envDescriptor);

            IWebHostEnvironment env = (IWebHostEnvironment)envDescriptor.ImplementationInstance;
            Enforce.AgainstNull(() => env);

            //services.AddSingleton<IServiceCollectionProvider>(new ServiceCollectionProvider(services));

            ConfigureServicesInitializePre(services, env);

            if (StartupExtensions != null)
                StartupExtensions.ToList().ForEach(l => l.ConfigureServicesPre(services, env, Configuration));

            services.AddSingleton<IServiceVersionInformation, ServiceVersionInformation>();

            ConfigureServicesInitializeMvcPre(services);

            if (StartupExtensions != null)
                StartupExtensions.ToList().ForEach(l => l.ConfigureServicesInitializeMvcPre(services, env, Configuration));

            ConfigureServicesInitializeServerTypes(services);

            ConfigureServicesInitializeMvcAntiforgery(services);

            if (StartupExtensions != null)
                StartupExtensions.ToList().ForEach(l => l.ConfigureServicesInitializeMvcPost(services, env, Configuration));

            ConfigureServicesInitializeMvcPost(services);

            // Only if UseCompression middleware was enabled on the IApplicationBuilder...
            _useCompression = ConfigureServicesInitializeCompression(services);
            if (_useCompression)
                ConfigureServicesInitializeCompressionOptions(services);

            ConfigureServicesInitializePost(services, env);

            if (StartupExtensions != null)
                StartupExtensions.ToList().ForEach(l => l.ConfigureServicesPost(services, env, Configuration));
        }
        #endregion

        #region Protected Methods
        protected virtual void ConfigureInitialize(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IServiceProvider svp)
        {
            if (env.IsDevelopment())
                ConfigureInitializeDebug(app, env, svp);
            else
                ConfigureInitializeProduction(app, env, svp);
        }

        protected virtual void ConfigureInitializeCompression(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();
        }

        protected virtual void ConfigureInitializeDebug(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider svp)
        {
            app.UseDeveloperExceptionPage();
        }

        protected virtual void ConfigureInitializeFinal(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider svp)
        {
        }

        protected abstract void ConfigureInitializeProduction(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider svp);

        protected virtual void ConfigureInitializeRouting(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(endpointsRouteBuilder =>
            {
                if (StartupExtensions != null)
                    StartupExtensions.ToList().ForEach(l => l.ConfigureInitializeRoutingEndpointsRouteBuilderPre(endpointsRouteBuilder));

                ConfigureInitializeRoutingEndpointsRouteBuilder(endpointsRouteBuilder);

                if (StartupExtensions != null)
                    StartupExtensions.ToList().ForEach(l => l.ConfigureInitializeRoutingEndpointsRouteBuilderPost(endpointsRouteBuilder));
            });
        }

        protected virtual void ConfigureInitializeRoutingEndpointsRouteBuilder(IEndpointRouteBuilder endpointsRouteBuilder)
        {
        }

        protected virtual void ConfigureInitializeServiceProvider(IServiceProvider svp)
        {
        }

        protected virtual void ConfigureInitializeSsl(IApplicationBuilder app, IWebHostEnvironment env)
        {
#if !DEBUG
            //var options = new RewriteOptions()
            //    //.AddRedirectToHttps();
            //    .AddRedirectToHttpsPermanent();

            //app.UseRewriter(options);

            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
            app.UseHttpsRedirection();
#endif
        }

        protected virtual void ConfigureInitializeStatic(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider svp)
        {
        }

        protected virtual void ConfigureInitializeStaticPost(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider svp)
        {
        }

        protected virtual void ConfigureInitializeStaticPre(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider svp)
        {
        }

        protected virtual bool ConfigureServicesInitializeCompression(IServiceCollection services)
        {
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
            });
            return true;
        }

        protected virtual void ConfigureServicesInitializeCompressionOptions(IServiceCollection services)
        {
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });
        }

        protected virtual void ConfigureServicesConfiguration(IServiceCollection services)
        {
        }

        protected virtual void ConfigureServicesInitializeMvcAntiforgery(IServiceCollection services)
        {
            string antiforgeryTokenName = ConfigureServicesInitializeMvcAntiforgeryTokenGenerate();
            if (string.IsNullOrEmpty(antiforgeryTokenName))
                return;

            services.AddAntiforgery(options => ConfigureServicesInitializeMvcAntiforgery(options, antiforgeryTokenName));
        }

        protected virtual void ConfigureServicesInitializeMvcAntiforgery(AntiforgeryOptions options, string antiforgeryTokenName)
        {
            options.Cookie.Name = antiforgeryTokenName;
        }

        protected virtual string ConfigureServicesInitializeMvcAntiforgeryTokenGenerate()
        {
            return null;
        }

        protected virtual void ConfigureServicesInitializeMvcBuilder(IMvcCoreBuilder builder)
        {
        }

        protected virtual void ConfigureServicesInitializeMvcBuilder(IMvcBuilder builder)
        {
        }

        protected virtual void ConfigureServicesInitializeMvcBuilderOptions(MvcOptions options)
        {
        }

        protected virtual void ConfigureServicesInitializeMvcBuilderOptionsFilters(MvcOptions options)
        {
            options.Filters.Add(typeof(InstrumentationActionFilter));
        }

        protected virtual void ConfigureServicesInitializeMvcPost(IServiceCollection services)
        {
            if (!string.IsNullOrEmpty(Localization))
                services.AddLocalization(options => options.ResourcesPath = Localization);
        }

        protected virtual void ConfigureServicesInitializeMvcPre(IServiceCollection services)
        {
            ServiceCollection = services;

            ConfigureServicesInitializeInstrumentation(services);
        }

        protected virtual void ConfigureServicesInitializeInstrumentation(IServiceCollection services)
        {
            services.AddTransient<IInstrumentationPacket, DefaultInstrumentationPacket>();
        }

        protected virtual void ConfigureServicesInitializePost(IServiceCollection services, IWebHostEnvironment env)
        {
        }

        protected virtual void ConfigureServicesInitializePre(IServiceCollection services, IWebHostEnvironment env)
        {
        }

        protected virtual void ConfigureServicesInitializeServerTypes(IServiceCollection services)
        {
            if (MvcType == MvcTypes.Core)
            {
                IMvcCoreBuilder mvcBuilder = services.AddMvcCore(options =>
                {
                    if (StartupExtensions != null)
                        StartupExtensions.ToList().ForEach(l => l.ConfigureServicesInitializeMvcBuilderOptionsPre(options));

                    ConfigureServicesInitializeMvcBuilderOptionsFilters(options);
                    ConfigureServicesInitializeMvcBuilderOptions(options);

                    if (StartupExtensions != null)
                        StartupExtensions.ToList().ForEach(l => l.ConfigureServicesInitializeMvcBuilderOptionsPost(options));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

                if (StartupExtensions != null)
                    StartupExtensions.ToList().ForEach(l => l.ConfigureServicesInitializeMvcBuilderPre(mvcBuilder));

                ConfigureServicesInitializeMvcBuilder(mvcBuilder);

                if (StartupExtensions != null)
                    StartupExtensions.ToList().ForEach(l => l.ConfigureServicesInitializeMvcBuilderPost(mvcBuilder));
            }
            else
            {
                IMvcBuilder mvcBuilder = services.AddMvc(options =>
                {
                    if (StartupExtensions != null)
                        StartupExtensions.ToList().ForEach(l => l.ConfigureServicesInitializeMvcBuilderOptionsPre(options));

                    ConfigureServicesInitializeMvcBuilderOptionsFilters(options);
                    ConfigureServicesInitializeMvcBuilderOptions(options);

                    if (StartupExtensions != null)
                        StartupExtensions.ToList().ForEach(l => l.ConfigureServicesInitializeMvcBuilderOptionsPost(options));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

                if (StartupExtensions != null)
                    StartupExtensions.ToList().ForEach(l => l.ConfigureServicesInitializeMvcBuilderPre(mvcBuilder));

                ConfigureServicesInitializeMvcBuilder(mvcBuilder);

                if (StartupExtensions != null)
                    StartupExtensions.ToList().ForEach(l => l.ConfigureServicesInitializeMvcBuilderPost(mvcBuilder));
            }
        }

        protected virtual void OnShutdown() { }

        protected virtual void RegisterStartupExtension(IStartupExtension extension)
        {
            if (extension == null)
                return;

            extension.RegisterStartupExtensions(StartupExtensions);
            StartupExtensions.Add(extension);
        }

        protected virtual void RegisterStartupExtension<TStartupExtension>()
            where TStartupExtension : IStartupExtension
        {
            StartupExtensions.Add(Utilities.Activator.CreateInstance<TStartupExtension>());
        }

        protected virtual void RegisterStartupExtensions()
        {
        }
        #endregion

        #region Protected Properties
        protected IConfiguration Configuration { get; set; }
        protected virtual string Localization { get { return KeyLocalization; } }
        protected ILogger<TStartup> Logger { get; private set; }
        protected MvcTypes MvcType { get; } = MvcTypes.Default;
        protected abstract bool RequiresSsl { get; }
        protected static IServiceCollection ServiceCollection { get; private set; }
        protected ICollection<IStartupExtension> StartupExtensions { get; } = new List<IStartupExtension>();
        #endregion

        #region Fields
        private bool _useCompression = false;
        #endregion

        #region Constants
        protected const string KeyLocalization = "Resources";
        #endregion
    }

    public enum MvcTypes
    {
        Core,
        Default
    }
}