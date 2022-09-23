using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using pbuddy.TypeScriptingUtility.RuntimeScripts;
using UnityEngine.Assertions;

namespace pbuddy.TypeScriptingUtility.EditorScripts
{
    public readonly struct TsReference
    {
        private enum ReferenceType
        {
            ExportedClass, 
            Array, 
            Enum, 
            Primitive,
            Tuple,
            LocalFunction,
            Structure,
        }

        public string Reference { get; }
        
        private readonly Type type;
        private readonly ReferenceType referenceType;

        private static readonly string NewLineIndent = Environment.NewLine + "\t";

        public static TsReference Primitive(Type type, string reference) => new (reference, type, ReferenceType.Primitive);
        public static TsReference Class(Type type, string reference) => new (reference, type, ReferenceType.ExportedClass);
        
        private TsReference(string reference, Type type, ReferenceType referenceType)
        {
            Reference = reference;
            this.type = type;
            this.referenceType = referenceType;
        }

        public bool TryGetDeclaration(IAPI api, Dictionary<Type, TsReference> referenceMap, out string declaration)
        {
            switch (referenceType)
            {
                case ReferenceType.ExportedClass:
                case ReferenceType.Tuple:
                case ReferenceType.Primitive:
                case ReferenceType.LocalFunction:
                case ReferenceType.Array:
                   declaration = null;
                   return false;
                case ReferenceType.Enum:
                    declaration = EnumDeclaration(type);
                    return true;
                case ReferenceType.Structure:
                    declaration = GetDeclarationFromMembers(type.IsGenericType ? type.GetGenericTypeDefinition() : type,
                                                            api,
                                                            referenceMap);
                    return true;
            }

            throw new Exception($"Unhandled reference type '{referenceType}' for: {type}");
        }
        
        public static bool TryDefine(Type type, IAPI api, Dictionary<Type, TsReference> referenceMap, out TsReference tsInterface)
        {
            static bool TryGetReference(Type type, IReadOnlyDictionary<Type, TsReference> typeMap, out string reference)
            {
                reference = typeMap.ContainsKey(type) ? typeMap[type].Reference : null;
                return reference is not null;
            }

            if (referenceMap.TryGetValue(type, out tsInterface)) return true;
            
            if (type.IsArray)
            {
                static string Bracket(string typeName) => $"{typeName}[]";
                
                Type elementType = type.GetElementType() ?? throw new Exception(nameof(Array));
                if (TryGetReference(elementType, referenceMap, out string reference))
                {
                    tsInterface = new TsReference(Bracket(reference), type, ReferenceType.Array);
                    return true;
                }
                
                return false;
            }

            if (TsTuple.IsTuple(type))
            {
                Type[] types = type.GetGenericArguments();
                List<string> references = new List<string>();
                foreach (Type tupleType in types)
                {
                    if (!TryGetReference(tupleType, referenceMap, out string reference)) return false;
                    references.Add(reference);
                }

                tsInterface = new TsReference($"[{string.Join(", ", references)}]", type, ReferenceType.Tuple);
                return true;
            }
            
            if (type.IsEnum)
            {
                tsInterface = new TsReference(type.Name, type, ReferenceType.Enum);
                return true;
            }

            if (typeof(MulticastDelegate).IsAssignableFrom(type))
            {
                if (!type.IsGenericType)
                {
                    tsInterface = new TsReference("() => void", type, ReferenceType.LocalFunction);
                    return true;
                }
                
                bool isAction = type.Name.StartsWith(nameof(Action));
                Type[] genericParameters = type.GenericTypeArguments;

                Type returnType = isAction ? null : type.GenericTypeArguments[^1];
                string returnText = returnType is not null
                    ? referenceMap.ContainsKey(returnType)
                        ? referenceMap[returnType].Reference
                        : null
                    : "void";
                
                if (returnText is null)
                {
                    return false;
                }

                if (!isAction) Array.Resize(ref genericParameters, genericParameters.Length - 1);

                List<string> args = new List<string>();
                for (var index = 0; index < genericParameters.Length; index++)
                {
                    Type arg = genericParameters[index];
                    if (!TryGetReference(arg, referenceMap, out string reference)) return false;
                    args.Add($"arg{index}: {reference}");
                }

                string argsText = string.Join(", ", args);
                tsInterface = new TsReference($"({argsText}) => {returnText}", type, ReferenceType.LocalFunction);
                return true;
            }
            
            if (type.IsClass || type.IsValueType || type.IsInterface)
            {
                if (!type.IsGenericType)
                {
                    tsInterface = new TsReference(type.Name, type, ReferenceType.Structure);
                    return true;
                }

                Type[] genericArgs = type.GenericTypeArguments;

                List<string> argsText = new List<string>();
                
                foreach (Type arg in genericArgs)
                {
                    if (!TryGetReference(arg, referenceMap, out string reference)) return false;
                    argsText.Add(reference);
                }

                tsInterface = new TsReference(NonGenericName(type, argsText), type, ReferenceType.Structure);
                return true;
            }

            throw new Exception($"Unhandled type: {type}");
        }

        private static string NonGenericName(Type type, IEnumerable<string> argsText) =>
            $"{type.Name.Substring(0, type.Name.IndexOf("`", StringComparison.Ordinal))}<{string.Join(", ", argsText)}>";

        private static string EnumDeclaration(Type type)
        {
            var names = Enum.GetNames(type);
            Array values = Enum.GetValues(type);
            var underlyingType = Enum.GetUnderlyingType(type);
            var entries = new string[names.Length];

            for (int i = 0; i < names.Length; i++)
            {
                entries[i] = $"{names[i]} = {values.GetValue(i).As(underlyingType)},";
            }
            
            return @$"export const enum {type.Name} {{{NewLineIndent}{string.Join(NewLineIndent, entries)}
}}";
        }
        
        private static string GetDeclarationFromMembers(Type type, IAPI api, Dictionary<Type, TsReference> typeMap)
        {
            const BindingFlags flags = BindingFlags.Public |
                                       BindingFlags.Instance |
                                       BindingFlags.DeclaredOnly;

            var members = String.Join(", " + NewLineIndent,
                                      type.GetMembers(flags)
                                          .Where(member => IsCorrectMemberType(member, type, api))
                                          .Select(member => GetMemberDeclaration(member, api, typeMap)));
            
            string name = type.IsGenericTypeDefinition
                ? NonGenericName(type, type.GetGenericArguments().Select(t => t.Name))
                : type.Name;
            return @$"export type {name} = {{{NewLineIndent}{members}
}}";
            static bool IsPropertyGetterSetter(MemberInfo info) => info.Name.StartsWith("get_") || info.Name.StartsWith("set_");
            static bool IsInvocable(Type type, IAPI api) => api[TsType.Specification.Variable].ContainsKey(type);
            static bool IsCorrectMemberType(MemberInfo info, Type type, IAPI api)
            {
                return info.MemberType switch
                {
                    MemberTypes.Field => true,
                    MemberTypes.Property => true,
                    MemberTypes.Method => !IsPropertyGetterSetter(info) && IsInvocable(type, api),
                    _ => false
                };
            }

            static string GetMethodDeclaration(MemberInfo memberInfo, 
                                               IAPI api, 
                                               Dictionary<Type, TsReference> typeMap)
            {
                MethodInfo methodInfo = memberInfo as MethodInfo;
                Assert.IsNotNull(methodInfo);
                var parameters = methodInfo.GetParameters();
                List<string> parametersText = new List<string>();
                
                for (var index = 0; index < parameters.Length; index++)
                {
                    ParameterInfo parameter = parameters[index];
                    string reference = parameter.ParameterType.IsGenericParameter
                        ? parameter.ParameterType.Name
                        : typeMap[parameter.ParameterType].Reference;

                    if (index < parameters.Length - 1 || !parameter.UsesParams())
                    {
                        parametersText.Add($"{parameter.Name}: {reference}");
                        continue;
                    }
                    
                    parametersText.Add($"...{parameter.Name}: {reference}");
                }
                
                string returnReference = methodInfo.ReturnType.IsGenericParameter
                    ? methodInfo.ReturnType.Name
                    : typeMap[methodInfo.ReturnType].Reference;
                
                string name = api.NameMapper.ToTs(memberInfo.Name);
                return $"{name}: ({string.Join(", ", parametersText)}) => {returnReference}";
            }

            static string GetMemberDeclaration(MemberInfo memberInfo,
                                                IAPI api,
                                                Dictionary<Type, TsReference> typeMap)
            {
                if (memberInfo.MemberType == MemberTypes.Method)
                {
                    return GetMethodDeclaration(memberInfo, api, typeMap);
                }
                
                DataMember member = new DataMember(memberInfo);
                string reference = member.Type.IsGenericParameter ? member.Type.Name : typeMap[member.Type].Reference;
                return $"{api.NameMapper.ToTs(member.Name)}: {reference}";
            }
        }
    }
}