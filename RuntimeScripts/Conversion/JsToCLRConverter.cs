using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public static class JaToClrConverter
    {
        private static readonly Dictionary<Type, DataMember[]> DataMembersByType;
        
        static JaToClrConverter()
        {
            DataMembersByType = new Dictionary<Type, DataMember[]>();
        }

        public static object As(this object obj, Type type)
        {
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

            if (obj is Array)
            {
                Debug.Log("Array!!!!");
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
                string name = kvp.Key;
                if (!TryMatchMember(name, type, out DataMember member))
                {
                    throw new Exception($"Could not match property for: {name}");
                }
                
                Type memberType = member.Type;
                if (IsDirectlyConvertible(memberType))
                {
                    member.SetValue(container, kvp.Value.AsType(memberType));
                    continue;
                }

                if (memberType.IsEnum)
                {
                    string entry = kvp.Value as string;
                    member.SetValue(container, ConvertEnumValueBack(entry, memberType).AsType(memberType));
                    continue;
                }

                if (memberType.IsArray)
                {
                    Type elementType = memberType.GetElementType();
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

                    member.SetValue(container, converted);
                    continue;
                }
                
                member.SetValue(container, To(kvp.Value as ExpandoObject, memberType, Activator.CreateInstance));
            }

            return container;
        }
        
        private static bool TryMatchMember(string name, Type type, out DataMember member)
        {
            member = default;
            
            if (!DataMembersByType.TryGetValue(type, out DataMember[] members))
            {
                var properties = type.GetProperties().ToList().Select(prop => new DataMember(prop));
                var fields = type.GetFields().ToList().Select(field => new DataMember(field));
                members = properties.Concat(fields).ToArray();
                DataMembersByType.Add(type, members);
            }

            string camel = AsCamelCase(name);
            string pascal = AsPascalCase(name);
            
            foreach (DataMember candidate in members)
            {
                if (candidate.Name == camel || candidate.Name == pascal)
                {
                    member = candidate;
                    return true;
                }
            }

            return false;
        }

        private static string AsPascalCase(string s) => Char.ToUpper(s[0]) + s.Substring(1);
        private static string AsCamelCase(string s) => Char.ToLower(s[0]) + s.Substring(1);
    }
}