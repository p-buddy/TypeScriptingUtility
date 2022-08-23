using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public static class WrapperFactory
    {
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
            IDictionary<string, object> dictionary = expando; 
            foreach (MemberInfo member in members)
            {
                switch (member.MemberType)
                {
                    case MemberTypes.Field:
                        var field = member as FieldInfo;
                        break;
                    case MemberTypes.Property:
                        var property = member as PropertyInfo;
                        break;
                    case MemberTypes.Method:
                        var method = member as MethodInfo;
                        Assert.IsNotNull(method);
                        var methodWrapper = new MethodWrapper(obj, method);
                        dictionary[member.Name] = methodWrapper.Delegate;
                        break;
                }
            }
            return expando;
        }
    }
}