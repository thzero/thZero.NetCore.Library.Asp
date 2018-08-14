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
using System.Linq;

namespace thZero.AspNetCore.Results
{
	public class SubmitResult
	{
		#region Public Methods
		public void AddError(string message, params object[] args)
		{
			if ((args != null) && (args.Count() > 0))
				message = string.Format(message, args);
			_errors.Add(new ErrorMessage() { Message = message });
		}

		public void AddError(string inputElement, string message, params object[] args)
		{
			if ((args != null) && (args.Count() > 0))
				message = string.Format(message, args);
			_errors.Add(new ErrorMessage() { Message = message, InputElement = inputElement });
		}
		#endregion

		#region Public Properties
		public IEnumerable<ErrorMessage> Errors { get { return _errors; } }

		public bool IsCancel { get; set; }
		public bool IsDelete { get; set; }
		public bool IsSave { get; set; }

		public bool Success { get { return _errors.Count() == 0; } }
		#endregion

		#region Fields
		private ICollection<ErrorMessage> _errors = new List<ErrorMessage>();
		#endregion

		public class ErrorMessage
		{
			protected internal ErrorMessage() { }

			#region Public Properties
			public string InputElement { get; set; }
			public string Message { get; set; }
			#endregion
		}
	}
}