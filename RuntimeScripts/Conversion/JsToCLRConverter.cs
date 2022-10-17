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

        public static bool Has(this ExpandoObject expando, string name) =>
            ((IDictionary<string, object>)expando).ContainsKey(name);

        public static object Get(this ExpandoObject expando, string name) => ((IDictionary<string, object>)expando)[name];
        public static void Add<T>(this ExpandoObject expando, string name, T value) => ((IDictionary<string, object>)expando)[name] = value;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static object As(this object obj, Type type, IClrToTsNameMapper mapper)
        {
            try
            {
                if (obj is null)
                {
                    int x = 0;
                }
                if (type.IsInstanceOfType(obj))
                {
                    return obj;
                }

                if (type.IsDirectlyConvertible())
                {
                    return obj.AsType(type);
                }

                if (type.IsEnum)
                {
                    return ConvertEnumValueBack($"{obj}", type).AsType(type);
                }

                if (obj.TryTreatAsValuable(type, mapper, out object converted))
                {
                    return converted;
                }

                switch (obj)
                {
                    case ExpandoObject expandoObject:
                        if (expandoObject.Has("internal"))
                        {
                            return expandoObject.Get("internal");
                        }
                        return expandoObject.To(type, Activator.CreateInstance, mapper);
                    case Array arr:
                        return ExtractArray(type, arr, mapper);
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
        /// <param name="jsValue"></param>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static object ConvertEnumValueBack(string jsValue, Type enumType)
        {
            return Enum.Parse(enumType, jsValue);
        }

        public static bool TryGetValuableInterface(this Type type, out Type valuableInterface)
        {
            Type[] interfaces = type.GetInterfaces();
            foreach (Type @interface in interfaces)
            {
                if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IValuable<>))
                {
                    valuableInterface = @interface;
                    return true;
                }
            }

            valuableInterface = default;
            return false;
        }
        
        private static bool TryTreatAsValuable(this object obj, Type type, IClrToTsNameMapper mapper, out object withValue)
        {
            if (!type.TryGetValuableInterface(out Type valuableInterface))
            {
                withValue = null;
                return false;
            }
            
            string valueFieldName = mapper.MapToTs(nameof(IValuable<object>.Value));
            if (obj is ExpandoObject expando && expando.Has(valueFieldName))
            {
                withValue = default;
                return false;
            }
            
            Type valuableType = valuableInterface.GenericTypeArguments[0];
            ExpandoObject expandoWithValue = new() { { valueFieldName, obj.As(valuableType, mapper) } };
            withValue = expandoWithValue.To(type, Activator.CreateInstance, mapper);
            return true;
        }

        private static bool IsDirectlyConvertible(this Type type) =>
            !type.IsArray && type.IsPrimitive || typeof(string).IsAssignableFrom(type);
        
        private static object AsType(this object obj, Type type) => Convert.ChangeType(obj, type);

        private static object To(this ExpandoObject obj,
                                 Type type,
                                 Func<Type, object> constructor,
                                 IClrToTsNameMapper mapper)
        {
            if (type.IsInstanceOfType(obj))
            {
                return obj;
            }
            
            if (type.IsDirectlyConvertible())
            {
                return obj.AsType(type);
            }
            
            IDictionary<string, object> valueByProperty = obj;
            object container = constructor.Invoke(type);

            foreach (KeyValuePair<string, object> kvp in valueByProperty)
            {
                string name = mapper.MapToClr(kvp.Key);

                if (name == "Internal") continue;
                
                if (!TryMatchMember(name, type, out DataMember member))
                {
                    throw new Exception($"Could not match member for: {name}");
                }
                
                Type memberType = member.Type;
                if (IsDirectlyConvertible(memberType))
                {
                    member.SetValue(container, kvp.Value.AsType(memberType));
                    continue;
                }

                if (memberType.IsEnum)
                {
                    string entry = $"{kvp.Value}";
                    member.SetValue(container, ConvertEnumValueBack(entry, memberType).AsType(memberType));
                    continue;
                }

                if (memberType.IsArray)
                {
                    member.SetValue(container, ExtractArray(memberType, kvp.Value, mapper));
                    continue;
                }

                if (kvp.Value.TryTreatAsValuable(memberType, mapper, out object casted))
                {
                    member.SetValue(container, casted);
                    continue;
                }

                if (kvp.Value.GetType() == memberType)
                {
                    member.SetValue(container, kvp.Value);
                    continue;
                }
                
                member.SetValue(container, To(kvp.Value as ExpandoObject, memberType, Activator.CreateInstance, mapper));
            }

            return container;
        }

        private static Array ExtractArray(Type arrayType, object arrayObject, IClrToTsNameMapper mapper)
        {
            Type elementType = arrayType.GetElementType();
            Assert.IsNotNull(elementType);
            Array array = arrayObject as Array;
            Assert.IsNotNull(array);
            Array converted = Array.CreateInstance(elementType, array.Length);
            if (IsDirectlyConvertible(elementType))
            {
                for (int i = 0; i < array.Length; i++)
                {
                    object element = array.GetValue(i);
                    converted.SetValue(element.AsType(elementType), i);
                }
            }
            else if (elementType.TryGetValuableInterface(out Type valuableInterface))
            {
                for (int i = 0; i < array.Length; i++)
                {
                    object element = array.GetValue(i);

                    string valueFieldName = mapper.MapToTs(nameof(IValuable<object>.Value));
                    if (element is ExpandoObject expando && expando.Has(valueFieldName))
                    {
                        converted.SetValue(To(expando, elementType, Activator.CreateInstance, mapper).AsType(elementType), i);
                        continue;
                    }
            
                    Type valuableType = valuableInterface.GenericTypeArguments[0];
                    ExpandoObject expandoWithValue = new() { { valueFieldName, element.As(valuableType, mapper) } };
                    converted.SetValue(expandoWithValue.To(elementType, Activator.CreateInstance, mapper), i); 
                }
            }
            else
            {
                for (int i = 0; i < array.Length; i++)
                {
                    object objElement = array.GetValue(i);
                    ExpandoObject element = objElement as ExpandoObject;
                    converted.SetValue(To(element, elementType, Activator.CreateInstance, mapper).AsType(elementType), i); 
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