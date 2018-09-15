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
using System.Text;

using Microsoft.Extensions.Logging;

namespace thZero.AspNetCore
{
	public static class LoggerExtensions
	{
		#region Public Methods
		public static string LogFormat(this ILogger logger, string method, string message)
		{
			return string.Concat(method, SeparatorColon, message);
		}

		public static string LogFormat(this ILogger logger, string method, string attribute, object value)
		{
			return string.Concat(method, SeparatorColon, attribute, SeparatorColon, (value != null ? value.ToString() : Null));
		}

		public static string LogFormat(this ILogger logger, string method, string attribute, Func<object> func)
		{
			return string.Concat(method, SeparatorColon, attribute, SeparatorColon, (func != null ? func() : null));
		}

		public static string LogFormat(this ILogger logger, string method, Exception ex)
		{
			return string.Concat(method, SeparatorColon, FormatException(ex));
		}

		public static string LogFormat(this ILogger logger, string method, string message, Exception ex)
		{
			return string.Concat(method, SeparatorColon, message, SeparatorComma, FormatException(ex));
		}

		public static void LogError(this ILogger logger, string method, Exception ex)
		{
			logger?.LogError(LogFormat(logger, method, ex));
		}

		public static void LogError(this ILogger logger, string method, string message, Exception ex)
		{
			logger?.LogError(LogFormat(logger, method, ex));
		}
		#endregion

		#region Private Methods
		private static string FormatException(Exception ex)
		{
			if (ex == null)
				return string.Empty;

			StringBuilder builder = new StringBuilder();

			builder.Append(ex.ToString()).AppendLine();
			builder.AppendLine(ex.StackTrace);

			ex = ex.InnerException;
			while (ex != null)
				builder.Append(InnerMessage).Append(ex.ToString()).AppendLine();

			return builder.ToString();
		}
		#endregion

		#region Constants
		private const string InnerMessage = "\tInner Message: ";
		private const string Null = "null ";
		private const string SeparatorColon = ": ";
		private const string SeparatorComma = ", ";
		#endregion
	}
}