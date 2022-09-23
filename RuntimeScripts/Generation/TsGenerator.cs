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
            Dictionary<Type, TsReference> referenceMap = new Dictionary<Type, TsReference>();
            IShared[] shared = api.Shared;
            
            referenceMap.AddPrimitiveReferences();
            referenceMap.AddClassReferences(api);

            List<Type> nestedReferences = shared.RetrieveNestedTypes().ToList();
            int iterationCount = 0;
            int maxIterations = nestedReferences.Count + 1;
            while (nestedReferences.Count > 0 && iterationCount < maxIterations)
            {
                for (int index = nestedReferences.Count - 1; index >= 0; index--)
                {
                    Type type = nestedReferences[index];
                    
                    if (!TsReference.TryDefine(type, api, referenceMap, out TsReference reference)) continue;
                    
                    nestedReferences.RemoveAt(index);
                    referenceMap[type] = reference;
                }

                iterationCount++;
            }

            HashSet<string> lines = new HashSet<string>(referenceMap.Count + shared.Length);
            foreach ((_, TsReference reference) in referenceMap)
            {
                if (reference.TryGetDeclaration(api, referenceMap, out string declaration))
                {
                    lines.Add(declaration);
                }
            }
            
            foreach (IShared share in shared)
            {
                ITsThing tsThing = share.TsType.Match(new TsType.Matcher.Func<ITsThing>
                {
                    OnVariable = () => new TsVariable(share, referenceMap),
                    OnClass = () => new TsClass(share, referenceMap),
                    OnFunction = () => new TsFunction(share, referenceMap)
                });
                lines.Add(tsThing.Declaration);
            }

            return String.Join(Environment.NewLine, lines);
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

        private static void AddClassReferences(this Dictionary<Type, TsReference> referenceMap, IAPI api)
        {
            foreach ((Type classType, IShared shareClass) in api[TsType.Specification.Class])
            {
                referenceMap[classType] = TsReference.Class(shareClass.ClrType, shareClass.TsType.Name);
            }
        }

        private static void RetrieveNestedTypes(Type type, HashSet<Type> types, bool dtoOnly = false)
        {
            if (type is null || types.Contains(type) || type.IsTsPrimitive() || type.IsGenericTypeDefinition) return;
            
            types.Add(type);

            if (type.IsEnum) return;

            if (type.IsArray)
            {
                RetrieveNestedTypes(type.GetElementType(), types, true);
                return;
            }

            if (type.IsGenericType)
            {
                foreach (Type genericType in type.GenericTypeArguments)
                {
                    RetrieveNestedTypes(genericType, types, true);
                    // ^Above causes crashes -- not why yet or if this is necessary
                }
            }

            const BindingFlags flags = BindingFlags.Public |
                                       BindingFlags.Instance |
                                       BindingFlags.DeclaredOnly;

            static bool IsCorrectMemberType(Type type, MemberInfo info, bool dtoOnly)
            {
                return info.MemberType switch
                {
                    MemberTypes.Field => true,
                    MemberTypes.Property => true,
                    MemberTypes.NestedType => true,
                    MemberTypes.Method => !dtoOnly,
                    _ => false
                };
            }

            var relevantMembers = type.GetMembers(flags).Where(info => IsCorrectMemberType(type, info, dtoOnly));
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
                    case MemberTypes.NestedType:
                        RetrieveNestedTypes(member as Type, types, true);
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