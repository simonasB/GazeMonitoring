using System;
using System.Reflection;

namespace GazeMonitoring.Tests
{
    public static class Extensions
    {
        public static TInstance WithMemberValue<TInstance, T>(this TInstance instance, string member, T value)
        {
            if (!WithFieldValue(instance, typeof(TInstance), member, value) && !WithPropertyValue(instance, typeof(TInstance), member, value))
                throw new Exception($"There is no member '{member}' for type '{instance?.GetType().Name ?? "null"}'.");
            return instance;
        }

        private static bool WithFieldValue<T>(object instance, Type instanceType, string member, T value)
        {
            var fieldInfo = instanceType.GetField(member, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            if (fieldInfo == null)
                return false;
            fieldInfo.SetValue(instance, value);
            return true;
        }

        private static bool WithPropertyValue<T>(object instance, Type instanceType, string member, T value)
        {
            var propertyInfo = instanceType.GetProperty(member, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            if (propertyInfo == null)
                return false;
            propertyInfo.SetValue(instance, value, new object[0]);
            return true;
        }

        public static T GetMemberValue<T>(this object instance, string member)
        {
            if (instance == null || !TryGetFieldValue(instance, instance.GetType(), member, out T value) && !TryGetPropertyValue(instance, instance.GetType(), member, out value))
                throw new Exception($"There is no member '{member}' for type '{instance?.GetType().Name ?? "null"}'.");
            return value;
        }

        private static bool TryGetFieldValue<T>(object instance, IReflect instanceType, string member, out T value)
        {
            value = default(T);
            var fieldInfo = instanceType.GetField(member, BindingFlags.Instance | BindingFlags.NonPublic);
            if (fieldInfo == null || !fieldInfo.FieldType.IsAssignableFrom(typeof(T)))
                return false;
            value = (T)fieldInfo.GetValue(instance);
            return true;
        }

        private static bool TryGetPropertyValue<T>(object instance, IReflect instanceType, string member, out T value)
        {
            value = default(T);
            var propertyInfo = instanceType.GetProperty(member, BindingFlags.Instance | BindingFlags.NonPublic);
            if (propertyInfo == null || !propertyInfo.PropertyType.IsAssignableFrom(typeof(T)))
                return false;
            value = (T)propertyInfo.GetValue(instance, new object[0]);
            return true;
        }
    }
}
