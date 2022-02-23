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
using System.Collections.Generic;

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
        void ConfigurePost(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider svp);
        void ConfigurePre(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider svp);
        void ConfigureInitializeAuthentication(IApplicationBuilder app);
        void ConfigureInitializeAuthorization(IApplicationBuilder app);
        void ConfigureInitializePost(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IServiceProvider svp);
        void ConfigureInitializePre(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IServiceProvider svp);
        void ConfigureInitializeFinalPre(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider svp);
        void ConfigureInitializeFinalPost(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider svp);
        void ConfigureInitializeRoutingEndpointsRouteBuilderPost(IEndpointRouteBuilder endpointsRouteBuilder);
        void ConfigureInitializeRoutingEndpointsRouteBuilderPre(IEndpointRouteBuilder endpointsRouteBuilder);
        void ConfigureInitializeSsl(IApplicationBuilder app);
        void ConfigureInitializeStaticPost(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider svp);
        void ConfigureInitializeStaticPre(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider svp);
        void ConfigureServicesPost(IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration);
        void ConfigureServicesPre(IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration);
        void ConfigureServicesInitializeMvcBuilderPost(IMvcBuilder builder);
        void ConfigureServicesInitializeMvcBuilderPre(IMvcBuilder builder);
        void ConfigureServicesInitializeMvcBuilderPost(IMvcCoreBuilder builder);
        void ConfigureServicesInitializeMvcBuilderPre(IMvcCoreBuilder builder);
        void ConfigureServicesInitializeMvcBuilderOptionsPost(MvcOptions options);
        void ConfigureServicesInitializeMvcBuilderOptionsPre(MvcOptions options);
        void ConfigureServicesInitializeAuthentication(IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration);
        void ConfigureServicesInitializeAuthorization(IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration);
        void ConfigureServicesInitializeMvcPost(IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration);
        void ConfigureServicesInitializeMvcPre(IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration);
        void RegisterStartupExtensions(ICollection<IStartupExtension> extensions);
    }
}
