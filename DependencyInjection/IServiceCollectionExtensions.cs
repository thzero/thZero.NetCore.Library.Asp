/* ------------------------------------------------------------------------- *
thZero.NetCore.Library.Asp
Copyright (C) 2016-2021 thZero.com

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
using System.Linq;
using System.Reflection;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;

namespace thZero.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		#region Public Methods
		public static IServiceCollection ScanForDependencies(this IServiceCollection services)
		{
			ServiceScanner scanner = Initialized(services);

			var hosting = services.BuildServiceProvider().GetService<IWebHostEnvironment>();
			scanner.RegisterAssembly(services, new AssemblyName(hosting.ApplicationName));
			return services;
		}

		public static IServiceCollection ScanForDependencies(this IServiceCollection services, Type rootAssembly, params string[] includes)
		{
			Enforce.AgainstNull(() => rootAssembly);

			if (includes != null)
			{
				ServiceScanner scanner = Initialized(services);

				//https://github.com/aspnet/Announcements/issues/149
				// Default DependencyContext is retrieved from entry assembly
				var deps = DependencyContext.Default;
#if DEBUG
				Console.WriteLine($"Compilation depenencies");
#endif
				foreach (var compilationLibrary in deps.CompileLibraries)
				{
#if DEBUG
					Console.WriteLine($"\tPackage {compilationLibrary.Name} {compilationLibrary.Version}");
#endif
					if (includes.Where(l => compilationLibrary.Name.StartsWithIgnore(l)).Count() == 0)
					{
#if DEBUG
						Console.WriteLine("\tIgnored");
#endif
						continue;
					}

					AssemblyName assemblyName = new AssemblyName(compilationLibrary.Name);
					//scanner.RegisterAssembly(services, assemblyName);
					scanner.RequestAssembly(services, assemblyName);
				}

				scanner.RegisterRequests(services);

//#if DEBUG
//				Console.WriteLine($"Runtime depenencies");
//#endif
//				foreach (var compilationLibrary in deps.RuntimeLibraries)
//				{
//#if DEBUG
//					Console.WriteLine($"\tPackage {compilationLibrary.Name} {compilationLibrary.Version}");
//#endif
//					foreach (var assembly2 in compilationLibrary.Assemblies)
//					{
//						if (includes.Where(l => assembly2.Name.Name.StartsWithIgnore(l)).Count() == 0)
//						{
//#if DEBUG
//							Console.WriteLine("\tIgnored");
//#endif
//							continue;
//						}
//#if DEBUG
//						Console.WriteLine($"\t\tReference: {assembly2.Name}");
//						scanner.RegisterAssembly(services, assembly2.Name);
//#endif
//					}
//				}

				//				Assembly assembly = rootAssembly.GetTypeInfo().Assembly;
				//				var assemblyNames = assembly.GetReferencedAssemblies();
				//				foreach (AssemblyName assemblyName in assemblyNames)
				//				{
				//#if DEBUG
				//					Console.WriteLine("Name={0}, Version={1}", assemblyName.Name, assemblyName.Version);
				//#endif
				//					if (includes.Where(l => assemblyName.Name.StartsWithIgnore(l)).Count() == 0)
				//					{
				//#if DEBUG
				//						Console.WriteLine("\tIgnored");
				//#endif
				//						continue;
				//					}

				//					scanner.RegisterAssembly(services, assemblyName);
				//				}
			}

			return services;
		}

		public static IServiceCollection ScanForDependencies(this IServiceCollection services, AssemblyName assemblyName)
		{
			ServiceScanner scanner = Initialized(services);
			scanner.RegisterAssembly(services, assemblyName);
			return services;
		}
		#endregion

		#region Private Methods
		private static ServiceScanner Initialized(this IServiceCollection services)
		{
			if (!_initialized)
			{
				lock (_lock)
				{
					if (!_initialized)
					{

						services.AddSingleton<ServiceScanner>();
						_initialized = true;
					}
				}
			}

			var scanner = services.BuildServiceProvider().GetService<ServiceScanner>();
			if (null == scanner)
				throw new InvalidOperationException("Invalid ServiceScanner");

			return scanner;
		}
		#endregion

		#region Fields
		private static bool _initialized = false;
		private static readonly object _lock = new object();
		#endregion
	}
}