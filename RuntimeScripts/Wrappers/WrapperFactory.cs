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

        public static object Wrap(this object obj)
        {
            if (obj is MulticastDelegate del)
            {
                var target = del.Target;
                MethodInfo methodInfo = del.GetMethodInfo();
                return new MethodWrapper(target, methodInfo).Delegate;
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
                        fieldAndPropertyWrappers.Add(new FieldAndPropertyWrapper(obj, field, expando));
                        break;
                    case MemberTypes.Property:
                        var property = member as PropertyInfo;
                        Assert.IsNotNull(property);
                        fieldAndPropertyWrappers.Add(new FieldAndPropertyWrapper(obj, property, expando));
                        break;
                    case MemberTypes.Method:
                        var method = member as MethodInfo;
                        Assert.IsNotNull(method);
                        var methodWrapper = new MethodWrapper(obj, method);
                        expando.Add(member.Name, methodWrapper.Delegate);
                        break;
                }
            }
            expando.Add("_wrappers", fieldAndPropertyWrappers.ToArray());
            return expando;
        }
    }
}