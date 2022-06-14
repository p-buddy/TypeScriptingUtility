using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public static class JaToClrConverter
    {
        private static Dictionary<Type, List<PropertyInfo>> PropertiesByType;

        static JaToClrConverter()
        {
            PropertiesByType = new Dictionary<Type, List<PropertyInfo>>();
        }

        public static object As(this object obj, Type type)
        {
            /* TODO Handle when type is array.
             // Likely should split out the internals of the To<> function
             
            if (type == typeof(object) || type.IsArray && type.GetElementType() == typeof(object))
            {
                return obj;
            }*/
            
            if (type.IsInstanceOfType(obj))
            {
                return obj;
            }

            if (IsDirectlyConvertible(type))
            {
                return AsType(obj, type);
            }
            
            if (obj is ExpandoObject expandoObject)
            {
                return To(expandoObject, type, Activator.CreateInstance);
            }

            throw new Exception($"Uh oh! {obj} vs {type}");
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T To<T>(ExpandoObject obj) where T : new()
        {
            return (T)To(obj, typeof(T), _ => new T());
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object To(Type type, ExpandoObject obj)
        {
            return To(obj, type, Activator.CreateInstance);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsValue"></param>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static object ConvertEnumValueBack(string jsValue, Type enumType)
        {
            return Enum.Parse(enumType, jsValue);
        }

        private static bool IsDirectlyConvertible(Type type) =>
            type.IsPrimitive || typeof(string).IsAssignableFrom(type);
        

        private static object AsType(this object obj, Type type) => Convert.ChangeType(obj, type);

        private static object To(ExpandoObject obj, Type type, Func<Type, object> constructor)
        {
            if (type.IsInstanceOfType(obj))
            {
                return obj;
            }
            
            if (IsDirectlyConvertible(type))
            {
                return obj.AsType(type);
            }
            
            IDictionary<string, object> valueByProperty = obj;
            object container = constructor.Invoke(type);

            foreach (KeyValuePair<string, object> kvp in valueByProperty)
            {
                string propertyName = kvp.Key;
                if (TryMatchProperty(propertyName, type, out PropertyInfo propertyInfo))
                {
                    Type propertyType = propertyInfo.PropertyType;
                    if (IsDirectlyConvertible(propertyType))
                    {
                        propertyInfo.SetValue(container, kvp.Value.AsType(propertyType));
                        continue;
                    }

                    if (propertyType.IsEnum)
                    {
                        string entry = kvp.Value as string;
                        propertyInfo.SetValue(container, ConvertEnumValueBack(entry, propertyType).AsType(propertyType));
                        continue;
                    }

                    if (propertyType.IsArray)
                    {
                        Type elementType = propertyInfo.PropertyType.GetElementType();
                        Array input = kvp.Value as Array;
                        Array converted = Array.CreateInstance(elementType, input.Length);
                        if (IsDirectlyConvertible(elementType))
                        {
                            for (int i = 0; i < input.Length; i++)
                            {
                                object element = input.GetValue(i);
                                converted.SetValue(element.AsType(elementType), i);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < input.Length; i++)
                            {
                                ExpandoObject element = input.GetValue(i) as ExpandoObject;
                                converted.SetValue(To(element, elementType, Activator.CreateInstance).AsType(elementType), i); 
                            }
                        }

                        propertyInfo.SetValue(container, converted);
                        continue;
                    }
                    
                    propertyInfo.SetValue(container, To(kvp.Value as ExpandoObject, propertyType, Activator.CreateInstance));
                    continue;
                }

                throw new Exception($"Could not match property for: {propertyName}");
            }

            return container;
        }
        
        private static bool TryMatchProperty(string name, Type type, out PropertyInfo propertyInfo)
        {
            propertyInfo = null;
            if (!PropertiesByType.TryGetValue(type, out List<PropertyInfo> properties))
            {
                properties = type.GetProperties().ToList();
            }
            
            string camel = AsCamelCase(name);
            string pascal = AsPascalCase(name);
            
            foreach (PropertyInfo property in properties)
            {
                if (property.Name == camel || property.Name == pascal)
                {
                    propertyInfo = property;
                    return true;
                }
            }

            return false;
        }

        private static string AsPascalCase(string s) => Char.ToUpper(s[0]) + s.Substring(1);
        private static string AsCamelCase(string s) => Char.ToLower(s[0]) + s.Substring(1);
    }
}