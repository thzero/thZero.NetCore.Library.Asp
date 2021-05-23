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
using System.Linq;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace thZero
{
    public static class SelectListExtensions
    {
        #region Public Methods
        public static IEnumerable<SelectListItemEx> ToSelectListAsEnumerable<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> valueSelector, Func<TItem, string> nameSelector)
        {
            return items.ToSelectListAsEnumerable(valueSelector, nameSelector, x => false);
        }

        public static IEnumerable<SelectListItemEx> ToSelectListAsEnumerable<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> valueSelector, Func<TItem, string> nameSelector, IEnumerable<TValue> selectedItems)
        {
            return items.ToSelectListAsEnumerable(valueSelector, nameSelector, x => selectedItems != null && selectedItems.Contains(valueSelector(x)));
        }

        public static IEnumerable<SelectListItemEx> ToSelectListAsEnumerable<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> valueSelector, Func<TItem, string> nameSelector, Func<TItem, bool> selectedValueSelector)
        {
            Enforce.AgainstNull(() => (items));
            Enforce.AgainstNull(() => (valueSelector));
            Enforce.AgainstNull(() => (nameSelector));
            Enforce.AgainstNull(() => (selectedValueSelector));

            foreach (var item in items)
            {
                yield return new SelectListItemEx
                {
                    Text = nameSelector(item),
                    Value = valueSelector(item).ToString(),
                    Selected = selectedValueSelector(item)
                };
            }
        }

        public static ICollection<SelectListItemEx> ToSelectListAsCollection<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> valueSelector, Func<TItem, string> nameSelector)
        {
            return ToSelectListCore(items, valueSelector, nameSelector, x => default(TValue), x => false);
        }

        public static ICollection<SelectListItemEx> ToSelectListAsCollection<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> valueSelector, Func<TItem, string> nameSelector, IEnumerable<TValue> selectedItems)
        {
            return ToSelectListCore(items, valueSelector, nameSelector, x => default(TValue), x => selectedItems != null && selectedItems.Contains(valueSelector(x)));
        }

        public static ICollection<SelectListItemEx> ToSelectListAsCollection<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> valueSelector, Func<TItem, string> nameSelector, Func<TItem, bool> selectedValueSelector)
        {
            return ToSelectListCore(items, valueSelector, nameSelector, x => default(TValue), selectedValueSelector);
        }

        public static List<SelectListItemEx> ToSelectList<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> valueSelector, Func<TItem, string> nameSelector)
        {
            return ToSelectListCore(items, valueSelector, nameSelector, x => default(TValue), x => false);
        }

        public static List<SelectListItemEx> ToSelectList<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> valueSelector, Func<TItem, string> nameSelector, Func<TItem, TValue> relatedValueSelector)
        {
            return ToSelectListCore(items, valueSelector, nameSelector, relatedValueSelector, x => false);
        }

        public static List<SelectListItemEx> ToSelectList<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> valueSelector, Func<TItem, string> nameSelector, IEnumerable<TValue> selectedItems)
        {
            return ToSelectListCore(items, valueSelector, nameSelector, x => default(TValue), x => selectedItems != null && selectedItems.Contains(valueSelector(x)));
        }

        public static List<SelectListItemEx> ToSelectList<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> valueSelector, Func<TItem, string> nameSelector, Func<TItem, TValue> relatedValueSelector, IEnumerable<TValue> selectedItems)
        {
            return ToSelectListCore(items, valueSelector, nameSelector, relatedValueSelector, x => selectedItems != null && selectedItems.Contains(valueSelector(x)));
        }

        public static List<SelectListItemEx> ToSelectList<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> valueSelector, Func<TItem, string> nameSelector, Func<TItem, TValue> relatedValueSelector, Func<TItem, bool> selectedValueSelector)
        {
            return ToSelectListCore(items, valueSelector, nameSelector, relatedValueSelector, selectedValueSelector);
        }
        #endregion

        #region Private Methods
        private static List<SelectListItemEx> ToSelectListCore<TItem, TValue>(IEnumerable<TItem> items, Func<TItem, TValue> valueSelector, Func<TItem, string> nameSelector, Func<TItem, TValue> relatedValueSelector, Func<TItem, bool> selectedValueSelector)
        {
            Enforce.AgainstNull(() => (items));
            Enforce.AgainstNull(() => (valueSelector));
            Enforce.AgainstNull(() => (nameSelector));
            Enforce.AgainstNull(() => (selectedValueSelector));
            Enforce.AgainstNull(() => (relatedValueSelector));

            List<SelectListItemEx> list = new List<SelectListItemEx>();

            foreach (var item in items)
            {
                list.Add(new SelectListItemEx
                {
                    Text = nameSelector(item),
                    Value = valueSelector(item).ToString(),
                    Selected = selectedValueSelector(item),
                    Related = relatedValueSelector(item).ToString()
                });
            }

            return list;
        }
        #endregion
    }

    public class SelectListItemEx : SelectListItem
    {
        public string Related { get; set; }
    }
}