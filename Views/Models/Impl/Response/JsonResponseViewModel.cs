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
using System.Collections.Generic;
using System.Linq;

namespace thZero.AspNetCore.Mvc.Views.Models
{
	public abstract class JsonResponseViewModel : ResponseViewModel
	{
		#region Public Methods
		public JsonResponseViewModel AddError(string message, params object[] args)
		{
			if ((args != null) && (args.Count() > 0))
				message = string.Format(message, args);
			_messages.Add(new ErrorMessage() { Message = message });
			_success = false;
			return this;
		}

		public JsonResponseViewModel AddError(string inputElement, string message, params object[] args)
		{
			if ((args != null) && (args.Count() > 0))
				message = string.Format(message, args);
			_messages.Add(new ErrorMessage() { InputElement = inputElement, Message = message });
			_success = false;
			return this;
		}

		public JsonResponseViewModel AddErrors(IEnumerable<string> messages)
		{
			foreach(var message in messages)
				_messages.Add(new ErrorMessage() { Message = message });
			_success = false;
			return this;
		}
        #endregion

        #region Public Properties
        public IEnumerable<ErrorMessage> Messages => _messages;

        public bool Success
        {
            get => (_messages.Count > 0) || _success;
            set => _success = value;
        }
		#endregion

		#region Fields
		private ICollection<ErrorMessage> _messages = new List<ErrorMessage>();
		private bool _success;
		#endregion

		public struct ErrorMessage
		{
			#region Public Properties
			public string InputElement { get; set; }
			public string Message { get; set; }
			#endregion
		}
	}

	public class JsonOutputErrorResponseViewModel : JsonResponseViewModel
	{
		public JsonOutputErrorResponseViewModel()
		{
			Success = false;
		}
	}

	public class JsonOutputResponseViewModel<T> : JsonResponseViewModel
	{
		#region Public Properties
		public T Data { get; set; }
		#endregion
	}

	public class JsonOutputSearchResponseViewModel<T> : JsonResponseViewModel
	{
		#region Public Properties
		public IEnumerable<T> Data { get; set; }
		#endregion
	}
}