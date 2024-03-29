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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using thZero.Services;

namespace thZero.DependencyInjection
{
    public class ServiceScanner : IService
    {
        #region Public Methods
#pragma warning disable CA1822 // Mark members as static
        public void RegisterAssembly(IServiceCollection services, AssemblyName assemblyName)
#pragma warning restore CA1822 // Mark members as static
        {
            IEnumerable<DependencyAttribute> attributes;
            ServiceDescriptor descriptor;
            var assembly = Assembly.Load(assemblyName);
            foreach (var type in assembly.DefinedTypes)
            {
                attributes = type.GetCustomAttributes<DependencyAttribute>();
                foreach (var dependencyAttribute in attributes)
                {
                    descriptor = dependencyAttribute.BuildServiceDescriptor(type);
                    services.Add(descriptor);
                }
            }
        }

#pragma warning disable CA1822 // Mark members as static
        public void RegisterRequests(IServiceCollection services)
#pragma warning restore CA1822 // Mark members as static
        {
            IOrderedEnumerable<ServiceScannerRequest> ordered = _requests.OrderBy(l => l.Order);
            foreach (ServiceScannerRequest request in ordered)
                services.Add(request.Descriptor);
        }

#pragma warning disable CA1822 // Mark members as static
        public void RequestAssembly(AssemblyName assemblyName)
#pragma warning restore CA1822 // Mark members as static
        {
            IEnumerable<DependencyAttribute> attributes;
            ServiceScannerRequest request;
            ServiceDescriptor descriptor;
            var assembly = Assembly.Load(assemblyName);
            foreach (var type in assembly.DefinedTypes)
            {
                attributes = type.GetCustomAttributes<DependencyAttribute>();
                foreach (var dependencyAttribute in attributes)
                {
                    descriptor = dependencyAttribute.BuildServiceDescriptor(type);
                    request = new ServiceScannerRequest() { Descriptor = descriptor, Order = dependencyAttribute.Order };
                    _requests.Add(request);
                }
            }
        }
        #endregion

        #region Fields
        private static readonly ICollection<ServiceScannerRequest> _requests = new List<ServiceScannerRequest>();
        #endregion
    }
}