using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using pbuddy.TypeScriptingUtility.RuntimeScripts;

namespace pbuddy.TypeScriptingUtility.EditorScripts
{
    public readonly struct TsInterface: ITsThing
    {
        public string Reference { get; }
        
        [CanBeNull] 
        public string Declaration { get; }

        private static readonly string NewLineIndent = Environment.NewLine + "\t";

        public TsInterface(Type type, IAPI containingAPI)
        {
            Dictionary<Type, IShared> classTypes = containingAPI[TsType.Specification.Class];
            
            if (classTypes.TryGetValue(type, out IShared sharedClass))
            {
                Reference = sharedClass.TsType.Name;
                Declaration = null;
                return;
            }
            
            if (TsPrimitives.TryGetTsName(type, out string name))
            {
                Reference = name;
                Declaration = null;
                return;
            }
            
            if (type.IsArray)
            {
                Type elementType = type.GetElementType() ?? throw new Exception(nameof(Array));
                Reference = classTypes.ContainsKey(type)
                    ? $"{classTypes[type].TsType.Name}[]"
                    : $"{new TsInterface(elementType, containingAPI).Reference}[]";
                Declaration = null;
                return;
            }

            if (TsTuple.IsTuple(type))
            {
                TsTuple tuple = new TsTuple(type, containingAPI);
                Reference = tuple.Reference;
                Declaration = tuple.Declaration;
                return;
            }
            
            if (type.IsEnum)
            {
                Reference = type.Name;
                Declaration = Enum(type);
                return;
            }

            if (typeof(MulticastDelegate).IsAssignableFrom(type))
            {
                Reference = "() => TODO";
                Declaration = null;
                return;
            }
            
            if (type.IsClass || type.IsValueType)
            {
                Reference = type.Name;
                Declaration = FromMembers(type, containingAPI);
                return;
            }


            throw new Exception($"Unhandled type: {type}");
        }

        private static string Enum(Type type)
        {
            var names = System.Enum.GetNames(type);
            Array values = System.Enum.GetValues(type);
            var underlyingType = System.Enum.GetUnderlyingType(type);
            var entries = new string[names.Length];

            for (int i = 0; i < names.Length; i++)
            {
                entries[i] = $"{names[i]} = {values.GetValue(i).As(underlyingType)},";
            }
            
            return @$"export const enum {type.Name} {{
    {String.Join(NewLineIndent, entries)}
}}";
        }

        private static string FromMembers(Type type, IAPI api)
        {
            const BindingFlags flags = BindingFlags.Public |
                                       BindingFlags.Instance |
                                       BindingFlags.DeclaredOnly;

            static bool IsPropertyGetterSetter(MemberInfo info) => info.Name.StartsWith("get_") || info.Name.StartsWith("set_");

            static bool IsCorrectMemberType(MemberInfo info, Type type, IAPI api)
            {
                return info.MemberType switch
                {
                    MemberTypes.Field => true,
                    MemberTypes.Property => true,
                    MemberTypes.Method => !IsPropertyGetterSetter(info) && api[TsType.Specification.Variable].ContainsKey(type),
                    _ => false
                };
            }

            static string GetDeclaration(MemberInfo memberInfo, IAPI api)
            {
                if (memberInfo.MemberType == MemberTypes.Method)
                {
                    return $"{api.NameMapper.ToTs(memberInfo.Name)}: () => TODO;";
                }
                
                var member = new DataMember(memberInfo);
                string reference = new TsInterface(member.Type, api).Reference;
                return $"{api.NameMapper.ToTs(member.Name)}: {reference}";
            }

            var members = String.Join(NewLineIndent,
                                      type.GetMembers(flags)
                                          .Where(member => IsCorrectMemberType(member, type, api))
                                          .Select(member => GetDeclaration(member, api)));
            
            return @$"export type {type.Name} = {{{NewLineIndent}{members}
}}";
        }
    }
}