using System;
using System.Reflection;

namespace ChubbyQuokka.DayZ.Utils
{
    internal static class ReflectionUtil
    {
        public static T GetPrivateField<T>(object instance, string field)
        {
            Type type = instance.GetType();

            FieldInfo fieldInfo = type.GetField(field, BindingFlags.NonPublic | BindingFlags.Instance);

            return (T)fieldInfo.GetValue(instance);
        }

        public static T GetPrivateProperty<T>(object instance, string property)
        {
            Type type = instance.GetType();

            PropertyInfo propertyInfo = type.GetProperty(property, BindingFlags.NonPublic | BindingFlags.Instance);

            return (T)propertyInfo.GetValue(instance, null);
        }
    }
}
