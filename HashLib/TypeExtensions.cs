using System;
using System.Diagnostics;

namespace HashLib
{
    [DebuggerStepThrough]
    public static class TypeExtensions
    {
        public static bool IsDerivedFrom(this Type a_type, Type a_baseType)
        {
            Debug.Assert(a_type.IsClass);
            Debug.Assert(a_baseType.IsClass);

            return a_baseType.IsAssignableFrom(a_type);
        }

        public static bool IsImplementingInterface(this Type a_type, Type a_interfaceType)
        {
            Debug.Assert(a_type.IsClass || a_type.IsInterface);
            Debug.Assert(a_interfaceType.IsInterface);

            return a_interfaceType.IsAssignableFrom(a_type);
        }
    }
}
