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

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace thZero.AspNetCore
{
    public interface IStartupExtension
    {
        void ConfigurePost(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider svp);
        void ConfigurePre(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider svp);
        void ConfigureInitializePost(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider svp);
        void ConfigureInitializePre(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider svp);
        void ConfigureInitializeFinalPre(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider svp);
        void ConfigureInitializeFinalPost(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider svp);
        void ConfigureInitializeRoutesBuilderPost(IRouteBuilder routes);
        void ConfigureInitializeRoutesBuilderPre(IRouteBuilder routes);
        void ConfigureInitializeSsl(IApplicationBuilder app);
        void ConfigureInitializeStaticPost(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider svp);
        void ConfigureInitializeStaticPre(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider svp);
        void ConfigureServicesPost(IServiceCollection services, IConfiguration configuration);
        void ConfigureServicesPre(IServiceCollection services, IConfiguration configuration);
        void ConfigureServicesInitializeMvcBuilderPost(IMvcBuilder options);
        void ConfigureServicesInitializeMvcBuilderPre(IMvcBuilder options);
        void ConfigureServicesInitializeMvcBuilderPost(IMvcCoreBuilder options);
        void ConfigureServicesInitializeMvcBuilderPre(IMvcCoreBuilder options);
        void ConfigureServicesInitializeMvcOptionsPost(MvcOptions options);
        void ConfigureServicesInitializeMvcOptionsPre(MvcOptions options);
        void ConfigureServicesInitializeMvcPost(IServiceCollection services, IConfiguration configuration);
        void ConfigureServicesInitializeMvcPre(IServiceCollection services, IConfiguration configuration);
    }
}
