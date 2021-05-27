/* ------------------------------------------------------------------------- *
thZero.Registry
Copyright (C) 2021-2021 thZero.com

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

namespace thZero.Configuration
{
    public class WebApplication<TDefaults, TEmail> : Application<TDefaults, TEmail>
        where TDefaults : ApplicationDefaults
        where TEmail : ApplicationEmail
    {
        public WebApplication()
        {
            Analytics = new Analytics();
            Cdn = new Cdn();
            Recaptcha = new Recaptcha();
        }

        #region Public Properties
        public Analytics Analytics { get; set; }
        public Cdn Cdn { get; set; }
        public Recaptcha Recaptcha { get; set; }
        #endregion
    }

    public class Analytics
    {
        #region Public Properties
        public string Key { get; set; }
        public string GoogleAnalyticsDomain
        {
            get
            {
#if DEBUG
                return "none";
#else
				return "auto";
#endif
            }
        }
        #endregion
    }

    public class Cdn
    {
        #region Public Properties
        public string Prefix { get; set; }
        #endregion
    }

    public class Recaptcha
    {
        #region Public Properties
        public string Key { get; set; }
        public string Secret { get; set; }
        #endregion
    }
}