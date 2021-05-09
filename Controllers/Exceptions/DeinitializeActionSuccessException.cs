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
using System.Collections.Generic;

namespace thZero.AspNetCore.Mvc
{
	public sealed class DeinitializeActionSuccessException : Exception
	{
		public DeinitializeActionSuccessException() : base(string.Empty)
		{
		}

		#region Public Methods
		public void Add(string error)
		{
			Enforce.AgainstNullOrEmpty(() => error);

			_errors.Add(new Error() { Message = error });
		}

		public void Add(Error error)
		{
			Enforce.Against(() => error, () => (error.Equals(default(Error))));

			_errors.Add(error);
		}
        #endregion

        #region Public Properties
        public IEnumerable<Error> Errors => _errors;

        public bool Success
        {
            get => (_errors.Count > 0) || _success;
            set => _success = value;
        }
        #endregion

        #region Fields
        private ICollection<Error> _errors = new List<Error>();
		private bool _success;
		#endregion

		public struct Error
		{
			#region Public Properties
			public string Key { get; set; }
			public Exception Exception { get; set; }
			public string Message { get; set; }
			#endregion
		}
	}
}