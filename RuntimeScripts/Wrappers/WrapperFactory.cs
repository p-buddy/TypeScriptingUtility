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
        public static MethodInfo WrapTypeMethod { get; }
        public static MethodInfo WrapObjectMethod { get; }

        static WrapperFactory()
        {
            MethodInfo[] wrapMethods = typeof(WrapperFactory).GetMethods()
                                                             .Where(method => method.Name == nameof(Wrap))
                                                             .ToArray();

            WrapTypeMethod = FindMatchingParameterType(wrapMethods, typeof(Type));
            WrapObjectMethod = FindMatchingParameterType(wrapMethods, typeof(object));
            
            static MethodInfo FindMatchingParameterType(IEnumerable<MethodInfo> methods, Type type)
            {
                return methods.First(method => method.GetParameters()[0].ParameterType == type);
            }
        }
        
        private static string MappedName(this MemberInfo member, IClrToTsNameMapper mapper) => mapper.MapToTs(member.Name);
        
        private static MemberInfo[] GetWrappedMembers(this Type type) =>
            type.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        
        public static TypeWrapper Wrap(this Type type, IClrToTsNameMapper nameMapper = null) => new (type, nameMapper);

        public static object Wrap(this object obj, IClrToTsNameMapper nameMapper = null)
        {
            if (obj is MulticastDelegate del)
            {
                var target = del.Target;
                MethodInfo methodInfo = del.GetMethodInfo();
                return new MethodWrapper(target, methodInfo, nameMapper).Delegate;
            }

            Type type = obj.GetType();
            MemberInfo[] members = type.GetWrappedMembers();
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