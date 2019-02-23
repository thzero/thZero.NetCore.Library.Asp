/* ------------------------------------------------------------------------- *
thZero.NetCore.Library.Asp
Copyright (C) 2016-2019 thZero.com

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

using thZero.Services;

using thZero.Configuration;

namespace thZero.Utilities.Web
{
	public static class Configuration
	{
		#region Public Properties
		public static BaseApplication Application { get; set; }
		#endregion
	}

	public static class Environment
	{
		#region Public Properties
		public static bool IsDevelopment { get; set; }
		public static bool IsProduction { get; set; }
		public static bool IsStaging { get; set; }
		#endregion
	}

	public static class General
	{
		#region Public Properties
		public static string CopyrightDate { get; set; }

		public static bool Initialized
		{
			get { return _initialized; }
			set
			{
				if (!_initialized)
					_initialized = value;
			}
		}

		public static string RootPath
		{
			get { return _rootPath; }
			set
			{
				_rootPath = value;
				if (!string.IsNullOrEmpty(_rootPath))
				{
					if (!_rootPath.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
						_rootPath += System.IO.Path.DirectorySeparatorChar;
				}
			}
		}
		#endregion

		#region Fields
		private static bool _initialized;
		private static string _rootPath;
		#endregion
	}

	public static class Path
	{
		#region Public Methods
		public static string Combine(string path)
		{
			if (string.IsNullOrEmpty(path))
				return General.RootPath;

			if (path.StartsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
				path = path.Replace(System.IO.Path.DirectorySeparatorChar.ToString(), string.Empty);

			return System.IO.Path.Combine(General.RootPath, path);
		}
		#endregion
	}

	public static class TimeZone
	{
		private static readonly thZero.Services.IServiceLog log = thZero.Factory.Instance.RetrieveLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		#region Public Methods
		public static DateTime ConvertTo(DateTime date, TimeZoneInfo timeZone)
		{
			Enforce.AgainstNull(() => timeZone);

			return TimeZoneInfo.ConvertTime(date, timeZone);
		}

		public static DateTime ConvertToDefault(DateTime date)
		{
			return TimeZoneInfo.ConvertTime(date.ToUniversalTime(), Default);
		}

		public static TimeZoneInfo Get()
		{
			return Get(string.Empty);
		}

		public static TimeZoneInfo Get(string timeZoneId)
		{
			TimeZoneInfo timeZone = TimeZoneInfo.Local;
			bool found = false;
			if (!string.IsNullOrEmpty(timeZoneId))
			{
				try
				{
					timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
					found = true;
				}
				catch (Exception) { }
			}

			if (!found)
				timeZone = Default;

			return timeZone;
		}

		public static void SetDefault()
		{
			SetDefault(string.Empty);
		}

		public static void SetDefault(string defaultTimeZoneId)
		{
			const string Declaration = "SetDefault";

			string defaultTimeZoneIdOverride = string.Empty;// Utilities.Web.Configuration.Application.Defaults.TimeZone; // TODO
			if (!string.IsNullOrEmpty(defaultTimeZoneIdOverride))
				defaultTimeZoneId = defaultTimeZoneIdOverride;

			bool found = false;
			if (!string.IsNullOrEmpty(defaultTimeZoneId))
			{
				try
				{
					_timeZoneDefault = TimeZoneInfo.FindSystemTimeZoneById(defaultTimeZoneId);
					found = true;
				}
				catch (Exception) { }
			}

			if (!found)
			{
				log.Warn(Declaration, string.Concat("Unable to find the timezone for '", defaultTimeZoneId, "'."));
				_timeZoneDefault = TimeZoneInfo.Local;
			}
		}
		#endregion

		#region Public Properties
		public static TimeZoneInfo Default
		{
			get { return _timeZoneDefault; }
		}
		#endregion

		#region Fields
		private static TimeZoneInfo _timeZoneDefault = TimeZoneInfo.Local;
		#endregion
	}
}

namespace thZero.Utilities.Services.Web
{
	public static class DataFormat
	{
		#region Public Properties
		public static IServiceDataFormat Instance => Factory.Instance.Retrieve<IServiceDataFormat>();
		#endregion
	}
}