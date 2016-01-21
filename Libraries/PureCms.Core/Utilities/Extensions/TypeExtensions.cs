﻿using System;
using System.ComponentModel;
using System.Reflection;
using System.Linq;

namespace PureCms
{
    public static class TypeExtensions
    {
        public static bool HasAttribute<TAttribute>(this ICustomAttributeProvider target, bool inherits) where TAttribute : Attribute
        {
            return target.IsDefined(typeof(TAttribute), inherits);
        }


        public static T CopyTo<T>(this Type sourceType, object sourceInstance) where T : class , new()
        {
            T target = new T();
            var targetType = typeof(T);
            var targetProps = targetType.GetProperties().ToList();
            foreach (var item in targetProps)
            {
                if (item.CanWrite)
                {
                    var sourceProp = sourceType.GetProperty(item.Name);
                    if (null != sourceProp && sourceProp.CanRead)
                    {
                        targetType.SetPropertyValue(target, item.Name, sourceProp.GetValue(sourceInstance));
                    }
                }
            }
            return target;
        }
        /// <summary>
        /// 设置对象的属性值
        /// </summary>
        /// <param name="target"></param>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetPropertyValue(this Type target, object instance, string name, object value)
        {
            PropertyInfo p = target.GetProperty(name);
            object v = GetFieldValue(value, p.PropertyType);
            if (p != null && p.CanWrite)
            {
                p.SetValue(instance, v);
                return true;
            }
            return false;
        }

        private static object GetFieldValue(object fieldValue, Type propType)
        {
            if (Convert.IsDBNull(fieldValue) || fieldValue == null)
            {
                return null;
            }
            else
            {
                if (propType.IsGenericType && propType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    if (fieldValue != null)
                    {
                        NullableConverter nullableConverter = new NullableConverter(propType);
                        propType = nullableConverter.UnderlyingType;
                    }
                    else
                    {
                        return propType.TypeInitializer;
                    }
                }

                return Convert.ChangeType(fieldValue, propType);
            }
        }
    }
}
