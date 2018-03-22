using System;
using System.Reflection;

namespace ChubbyQuokka.DayZ.Utils
{
    internal static class ReflectionUtil
    {
        public static T GetPrivateField<T>(object instance, string field)
        {
            Type type = instance.GetType();

            FieldInfo fieldInfo = type.GetField(field);

            return (T)fieldInfo.GetValue(instance);
        }
    }
}
