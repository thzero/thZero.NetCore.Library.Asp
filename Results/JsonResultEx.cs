///* ------------------------------------------------------------------------- *
//thZero.NetCore.Library.Asp
//Copyright (C) 2016-2022 thZero.com

//<development [at] thzero [dot] com>

//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at

//	http://www.apache.org/licenses/LICENSE-2.0

//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
// * ------------------------------------------------------------------------- */

//using System;

//using Microsoft.AspNetCore.Mvc;

//using Newtonsoft.Json;

//namespace thZero.AspNetCore.Mvc
//{
//    public class JsonResultEx : JsonResult
//    {
//        public JsonResultEx(object value) : base(value)
//        {
//            if (_settings != null)
//                return;

//            lock (_lock)
//            {
//                if (_settings != null)
//                    return;

//                _settings = new JsonSerializerSettings
//                {
//                    MissingMemberHandling = MissingMemberHandling.Ignore,
//                    NullValueHandling = NullValueHandling.Ignore,
//                    DefaultValueHandling = DefaultValueHandling.Include,
//                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
//                };
//            }
//        }

//        #region Fields
//        private static volatile JsonSerializerSettings _settings;
//        private static readonly object _lock = new();
//        #endregion
//    }
//}