using System;
using System.Collections.Generic;
using System.Reflection;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public static class TypeCache
    {
        private static readonly Dictionary<MethodBase, ParameterInfo[]> ParametersByConstructorInfo = new();
        private static readonly Dictionary<MethodBase, Type[]> ParameterTypesByConstructorInfo = new();
        private static readonly Dictionary<MethodInfo, Delegate> DelegateByMethodInfo = new();
        private static readonly Dictionary<ParameterInfo, ParamArrayAttribute[]> ParamsAttributesByParameter = new();
        public static ParameterInfo[] GetCachedParameters(this MethodBase info)
        {
            if (ParametersByConstructorInfo.TryGetValue(info, out ParameterInfo[] parameters)) return parameters;
            parameters = info.GetParameters();
            ParametersByConstructorInfo[info] = parameters;
            return parameters;
        }
        
        public static Type[] GetCachedParameterTypes(this MethodBase info)
        {
            if (ParameterTypesByConstructorInfo.TryGetValue(info, out Type[] types)) return types;
            ParameterInfo[] parameters = GetCachedParameters(info);
            types = new Type[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                types[i] = parameters[i].ParameterType;
            }

            ParameterTypesByConstructorInfo[info] = types;
            return types;
        }

        public static Delegate GetCachedDelegate(this MethodInfo method, Type type, object container)
        {
            if (DelegateByMethodInfo.TryGetValue(method, out Delegate @delegate)) return @delegate;
            @delegate = Delegate.CreateDelegate(type, container, method);
            DelegateByMethodInfo[method] = @delegate;
            return @delegate;
        }

        public static ParamArrayAttribute[] GetCachedParamsArrayAttributes(this ParameterInfo parameterInfo, bool inherit = false)
        {
            if (ParamsAttributesByParameter.TryGetValue(parameterInfo, out ParamArrayAttribute[] attributes)) return attributes;
            attributes = parameterInfo.GetCustomAttributes(typeof(ParamArrayAttribute), inherit) as ParamArrayAttribute[];
            ParamsAttributesByParameter[parameterInfo] = attributes;
            return attributes;
        }
    }
}