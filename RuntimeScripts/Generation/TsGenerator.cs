using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using pbuddy.TypeScriptingUtility.RuntimeScripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace pbuddy.TypeScriptingUtility.EditorScripts
{
    public static class TsGenerator
    {
        public const string ExportConst = "export const";
        public const string TsIgnore = "// @ts-ignore";
        
        public static string Generate(this IAPI api)
        {
            Dictionary<Type, ITsThing> typeMap = new Dictionary<Type, ITsThing>();
            IShared[] shared = api.Shared;

            var types = shared.RetrieveNestedTypes();
            StringBuilder builder = new StringBuilder(shared.Length + types.Count);

            foreach (Type type in types)
            {
                TsInterface tsInterface = new (type, api);
                if (tsInterface.Declaration is not null) builder.AppendLine(tsInterface.Declaration);
                typeMap[type] = tsInterface;
            }

            foreach (IShared share in shared)
            {
                ITsThing tsThing = share.TsType.Match(new TsType.Matcher.Func<ITsThing>
                {
                    OnVariable = () => new TsVariable(share, typeMap),
                    OnClass = () => new TsClass(share, typeMap),
                    OnFunction = () => new TsFunction(share, typeMap)
                });

                builder.AppendLine(tsThing.Declaration);
            }

            return builder.ToString();
        }

        private static HashSet<Type> RetrieveNestedTypes(this IShared[] allShared)
        {
            HashSet<Type> nestedTypes = new HashSet<Type>();
            foreach (IShared shared in allShared)
            {
                switch (shared.TsType.Spec)
                {
                    case TsType.Specification.Class:
                        RetrieveNestedTypes(shared.ClrType, nestedTypes);
                        break;
                    case TsType.Specification.Function:
                        foreach (Type type in ExtractTypesFromFunction(shared))
                        {
                            RetrieveNestedTypes(type, nestedTypes);
                        }
                        break;
                    case TsType.Specification.Variable:
                        RetrieveNestedTypes(shared.ClrType, nestedTypes);
                        break;
                }
            }

            return nestedTypes;
        }

        private static Type[] ExtractTypesFromFunction(IShared shared)
        {
            MethodInfo method = (shared.NonSpecificClrObject as MulticastDelegate)?.Method ?? throw new Exception("");
            ParameterInfo[] parameters = method.GetParameters();
            Type returnType = method.ReturnType;
            Type[] types = new Type[parameters.Length + (returnType == typeof(void) ? 0 : 1)];
            for (int i = 0; i < types.Length; i++)
            {
                types[i] = i == parameters.Length ? returnType : parameters[i].ParameterType;
            }

            return types;
        }

        private static void RetrieveNestedTypes(Type type, HashSet<Type> types, bool dtoOnly = false)
        {
            if (type is null || types.Contains(type) || type.IsTsPrimitive()) return;
            
            types.Add(type);

            if (type.IsEnum) return;

            if (type.IsArray)
            {
                RetrieveNestedTypes(type.GetElementType(), types, true);
                return;
            }

            if (type.IsGenericType)
            {
                foreach (Type genericType in type.GetGenericArguments())
                {
                    // RetrieveNestedTypes(genericType, types, true);
                    // ^Above causes crashes -- not yet sure if necessary
                }
            }

            const BindingFlags flags = BindingFlags.Public |
                                       BindingFlags.Instance |
                                       BindingFlags.DeclaredOnly;

            static bool IsCorrectMemberType(MemberInfo info, Type type, bool dtoOnly)
            {
                return info.MemberType switch
                {
                    MemberTypes.Field => true,
                    MemberTypes.Property => true,
                    MemberTypes.Method => !dtoOnly,
                    _ => false
                };
            }

            var relevantMembers = type.GetMembers(flags).Where(info => IsCorrectMemberType(info, type, dtoOnly));
            foreach (MemberInfo member in relevantMembers.ToArray())
            {
                switch (member.MemberType)
                {
                    case MemberTypes.Field:
                        RetrieveNestedTypes((member as FieldInfo)?.FieldType, types, true);
                        break;
                    case MemberTypes.Property:
                        RetrieveNestedTypes((member as PropertyInfo)?.PropertyType, types, true);
                        break;
                    case MemberTypes.Method:
                        var method = member as MethodInfo ?? throw new Exception();
                        foreach (var parameter in method.GetParameters())
                        {
                            RetrieveNestedTypes(parameter.ParameterType, types, true);
                        }
                        RetrieveNestedTypes(method.ReturnType, types, true);
                        break;
                }
            }
        }
    }
}