using System;

namespace Math
{
    public static class SubclassCheck
    {
        /// <summary>
        /// Check if a type is a subclass of a generic type with any parameter
        /// </summary>
        /// <param name="sub"></param>
        /// <param name="generic"></param>
        /// <returns></returns>
        public static bool IsSubclassOfGeneric(this Type sub, Type generic)
        {
            while (sub != null && sub != typeof(object)) {
                Type cur = sub.IsGenericType ? sub.GetGenericTypeDefinition() : sub;
                if (generic == cur) {
                    return true;
                }
                sub = sub.BaseType;
            }
            return false;
        }

        public static bool IsSubclass(this Type sub, Type b)
        {
            return sub.IsSubclassOf(b) || sub.IsSubclassOfGeneric(b);
        }
    }
}