﻿using System;
using System.Reflection;

namespace Nop.Plugin.Misc.CodeInjector.Extensions
{
    public static class ObjectExtension
    {
        public static Object GetPropValue(this Object obj, String name)
        {
            foreach (String part in name.Split('.'))
            {
                if (obj == null)
                { return null; }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null)
                { return null; }

                obj = info.GetValue(obj, null);
            }
            return obj;
        }

        public static T GetPropValue<T>(this Object obj, String name)
        {
            Object retval = GetPropValue(obj, name);
            if (retval == null)
            { return default(T); }

            // throws InvalidCastException if types are incompatible
            return (T)retval;
        }
    }
}
