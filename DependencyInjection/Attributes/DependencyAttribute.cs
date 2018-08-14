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
using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace thZero.AspNetCore.DependencyInjection
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public abstract class DependencyAttribute : Attribute
	{
		protected DependencyAttribute(ServiceLifetime dependencyType)
		{
			DependencyType = dependencyType;
			Order = long.MaxValue;
		}

		#region Public Methods
		public ServiceDescriptor BuildServiceDescriptor(TypeInfo type)
		{
			var serviceType = ServiceType ?? type.AsType();
			return new ServiceDescriptor(serviceType, type.AsType(), DependencyType);
		}
		#endregion

		#region Public Properties
		public ServiceLifetime DependencyType { get; set; }

		public long Order { get; set; }

		public Type ServiceType { get; set; }
		#endregion
	}
}