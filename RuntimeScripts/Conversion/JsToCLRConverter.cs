using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public static class JsToClrConverter
    {
        private static readonly Dictionary<Type, DataMember[]> DataMembersByType;
        
        static JsToClrConverter()
        {
            DataMembersByType = new Dictionary<Type, DataMember[]>();
        }

        public static object As(this object obj, Type type, IClrToTsNameMapper mapper = null)
        {
            try
            {
                if (type.IsInstanceOfType(obj))
                {
                    return obj;
                }

                if (IsDirectlyConvertible(type))
                {
                    return AsType(obj, type);
                }

                switch (obj)
                {
                    case ExpandoObject expandoObject:
                        return To(expandoObject, type, Activator.CreateInstance, mapper);
                    case Array arr:
                        return ExtractArray(type, arr);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Uh  oh! There was this exception while trying to cast {obj} to type {type}: {e}");
            }


            throw new Exception($"Uh oh! Unhandled case for casting {obj} to type {type}.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="mapper"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T To<T>(ExpandoObject obj, IClrToTsNameMapper mapper = null) where T : new()
        {
            return (T)To(obj, typeof(T), _ => new T(), mapper);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public static object To(Type type, ExpandoObject obj, IClrToTsNameMapper mapper = null)
        {
            return To(obj, type, Activator.CreateInstance, mapper);
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
            !type.IsArray && type.IsPrimitive || typeof(string).IsAssignableFrom(type);
        
        private static object AsType(this object obj, Type type) => Convert.ChangeType(obj, type);

        private static object To(ExpandoObject obj,
                                 Type type,
                                 Func<Type, object> constructor,
                                 IClrToTsNameMapper mapper = null)
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
                string name = mapper.MapToClr(kvp.Key);
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
                    member.SetValue(container, ExtractArray(memberType, kvp.Value));
                    continue;
                }
                
                member.SetValue(container, To(kvp.Value as ExpandoObject, memberType, Activator.CreateInstance));
            }

            return container;
        }

        private static Array ExtractArray(Type arrayType, object arrayObject)
        {
            Type elementType = arrayType.GetElementType();
            Assert.IsNotNull(elementType);
            Array arr = arrayObject as Array;
            Assert.IsNotNull(arr);
            Array converted = Array.CreateInstance(elementType, arr.Length);
            if (IsDirectlyConvertible(elementType))
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    object element = arr.GetValue(i);
                    converted.SetValue(element.AsType(elementType), i);
                }
            }
            else
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    ExpandoObject element = arr.GetValue(i) as ExpandoObject;
                    converted.SetValue(To(element, elementType, Activator.CreateInstance).AsType(elementType), i); 
                }
            }

            return converted;
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

            foreach (DataMember candidate in members)
            {
                if (candidate.Name == name)
                {
                    member = candidate;
                    return true;
                }
            }

            return false;
        }
    }
}