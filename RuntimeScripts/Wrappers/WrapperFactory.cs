using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public static class WrapperFactory
    {
        internal static void Add<T>(this ExpandoObject expando, string name, T value) => 
            ((IDictionary<string, object>)expando)[name] = value;
        
        internal static object Get(this ExpandoObject expando, string name) => ((IDictionary<string, object>)expando)[name];
        private static string MappedName(this MemberInfo member, IClrToTsNameMapper mapper) => mapper.MapToTs(member.Name);

        public static object Wrap(this object obj, IClrToTsNameMapper nameMapper = null)
        {
            if (obj is MulticastDelegate del)
            {
                var target = del.Target;
                MethodInfo methodInfo = del.GetMethodInfo();
                return new MethodWrapper(target, methodInfo, nameMapper).Delegate;
            }
            
            Type type = obj.GetType();
            MemberInfo[] members = type.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            ExpandoObject expando = new ExpandoObject();
            var fieldAndPropertyWrappers = new List<FieldAndPropertyWrapper>();
            
            foreach (MemberInfo member in members)
            {
                switch (member.MemberType)
                {
                    case MemberTypes.Field:
                        var field = member as FieldInfo;
                        Assert.IsNotNull(field);
                        fieldAndPropertyWrappers.Add(new FieldAndPropertyWrapper(obj, field, expando, nameMapper));
                        break;
                    case MemberTypes.Property:
                        var property = member as PropertyInfo;
                        Assert.IsNotNull(property);
                        fieldAndPropertyWrappers.Add(new FieldAndPropertyWrapper(obj, property, expando, nameMapper));
                        break;
                    case MemberTypes.Method:
                        var method = member as MethodInfo;
                        Assert.IsNotNull(method);
                        var methodWrapper = new MethodWrapper(obj, method, nameMapper);
                        expando.Add(member.MappedName(nameMapper), methodWrapper.Delegate);
                        break;
                }
            }
            
            // Collect wrappers to avoid them being garbage collected
            expando.Add("_wrappers", fieldAndPropertyWrappers.ToArray()); 
            
            return expando;
        }
    }
}