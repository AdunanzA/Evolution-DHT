using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

namespace HashLib
{
    [DebuggerStepThrough]
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Take<T>(this IEnumerable<T> a_enumerable, int a_from, int a_count)
        {
            return a_enumerable.Skip(a_from).Take(a_count);
        }
    }
}
