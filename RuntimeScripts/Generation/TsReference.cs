using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine.Assertions;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
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

        public bool TryGetDeclaration(in TypeReferenceMap referenceMap, out string declaration)
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
                                                            in referenceMap);
                    return true;
            }

            throw new Exception($"Unhandled reference type '{referenceType}' for: {type}");
        }
        
        public static bool TryDefine(Type type, in TypeReferenceMap referenceMap, out TsReference tsInterface)
        {
            if (referenceMap.TryGetTsReference(type, out tsInterface)) return true;
            
            if (type.IsArray)
            {
                static string Bracket(string typeName) => $"{typeName}[]";
                
                Type elementType = type.GetElementType() ?? throw new Exception(nameof(Array));
                if (referenceMap.TryGetReferenceString(elementType, out string reference))
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
                    if (!referenceMap.TryGetReferenceString(tupleType, out string reference)) return false;
                    references.Add(reference);
                }

                tsInterface = new TsReference($"[{references.Csv()}]", type, ReferenceType.Tuple);
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
                    ? referenceMap.Contains(returnType) ? referenceMap.GetReference(returnType) : null
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
                    if (!referenceMap.TryGetReferenceString(arg, out string reference)) return false;
                    args.Add($"arg{index}: {reference}");
                }

                tsInterface = new TsReference($"({args.Csv()}) => {returnText}", type, ReferenceType.LocalFunction);
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
                    if (!referenceMap.TryGetReferenceString(arg, out string reference)) return false;
                    argsText.Add(reference);
                }

                tsInterface = new TsReference(NonGenericName(type, argsText), type, ReferenceType.Structure);
                return true;
            }

            throw new Exception($"Unhandled type: {type}");
        }

        private static string NonGenericName(Type type, IEnumerable<string> argsText) =>
            $"{type.Name.Substring(0, type.Name.IndexOf("`", StringComparison.Ordinal))}<{argsText.Csv()}>";

        private static string EnumDeclaration(Type type)
        {
            var names = Enum.GetNames(type);
            Array values = Enum.GetValues(type);
            var underlyingType = Enum.GetUnderlyingType(type);
            var entries = new string[names.Length];

            for (int i = 0; i < names.Length; i++)
            {
                entries[i] = $"{names[i]} = {values.GetValue(i).As(underlyingType, null)},";
            }
            
            return @$"export const enum {type.Name} {{{NewLineIndent}{string.Join(NewLineIndent, entries)}
}}";
        }

        public static string GetMemberDeclaration(MemberInfo memberInfo,
                                                  in TypeReferenceMap typeMap,
                                                  bool isArrowFunction = true)
        {
            if (memberInfo.MemberType == MemberTypes.Method)
            {
                return GetMethodDeclaration(memberInfo, typeMap.API, typeMap, isArrowFunction);
            }
                
            DataMember member = new DataMember(memberInfo);

            if (member.IndexParams is not null)
            {
                var localMap = typeMap;
                string parameters = member
                                    .IndexParams
                                    .Select(param => $"{param.Name}: {localMap.GetReference(param.ParameterType)}")
                                    .Csv();
                return $"[{parameters}]: {typeMap.GetReference(member.Type)}";
            }
            
            // TODO handle indexers
            
            return $"{typeMap.API.NameMapper.ToTs(member.Name)}: {typeMap.GetReference(member.Type)}";
            
            #region Local Static Functions

            static string GetMethodDeclaration(MemberInfo memberInfo, IAPI api, in TypeReferenceMap typeMap, bool isArrowFunction)
            {
                MethodInfo methodInfo = memberInfo as MethodInfo;
                Assert.IsNotNull(methodInfo);
                var parameters = methodInfo.GetParameters();
                List<string> parametersText = new List<string>();
                
                for (var index = 0; index < parameters.Length; index++)
                {
                    ParameterInfo parameter = parameters[index];
                    string reference = typeMap.GetReference(parameter.ParameterType);

                    if (index < parameters.Length - 1 || !parameter.UsesParams())
                    {
                        parametersText.Add($"{parameter.Name}: {reference}");
                        continue;
                    }
                    
                    parametersText.Add($"...{parameter.Name}: {reference}");
                }
                
                string name = api.NameMapper.ToTs(memberInfo.Name);
                return $"{name}{(isArrowFunction ? ": " : "")}({parametersText.Csv()}){(isArrowFunction ? " =>" : ":")} {typeMap.GetReference(methodInfo.ReturnType)}";
            }
            
            #endregion Local Static Functions
        }

        public static IEnumerable<MemberInfo> GetMembersRequiringDeclaration(Type type, in TypeReferenceMap typeMap)
        {
            const BindingFlags flags = BindingFlags.Public |
                                       BindingFlags.Instance |
                                       BindingFlags.DeclaredOnly;

            TypeReferenceMap localMap = typeMap;

            return type.GetMembers(flags)
                .Where(member => IsCorrectMemberType(member, type, localMap.API))
                .Where(HideFromAPIAttribute.IsNotHidden);
            
            #region Local Static Functions
            static bool IsPropertyGetterSetter(MemberInfo info) => info.Name.StartsWith("get_") || info.Name.StartsWith("set_");
            static bool IsInvocable(Type type, IAPI api) =>
                api[TsType.Specification.Variable].ContainsKey(type) ||
                api[TsType.Specification.Class].ContainsKey(type);
            
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
            #endregion Local Static Functions
        }

        public static string GetMembersDeclaration(Type type, in TypeReferenceMap typeMap)
        {
            TypeReferenceMap localMap = typeMap;
            return GetMembersRequiringDeclaration(type, in typeMap)
                   .Select(member => GetMemberDeclaration(member, in localMap))
                   .Csv(NewLineIndent);
        }
        
        private static string GetDeclarationFromMembers(Type type, in TypeReferenceMap typeMap)
        {
            string name = type.IsGenericTypeDefinition
                ? NonGenericName(type, type.GetGenericArguments().Select(t => t.Name))
                : type.Name;
            
            return @$"export type {name} = {{{NewLineIndent}{GetMembersDeclaration(type, in typeMap)}
}}";
        }
    }
}