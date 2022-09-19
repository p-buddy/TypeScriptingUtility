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
        
        public TsInterface(string reference)
        {
            Reference = reference;
            Declaration = null;
        }
        
        public TsInterface(Type type, Func<Type, string> getClassName, IClrToTsNameMapper nameMapper)
        {
            string className = getClassName(type);
            
            if (className is not null)
            {
                Reference = className;
                Declaration = null;
            }
            
            if (TsPrimitives.TryGetTsName(type, out string name))
            {
                Reference = name;
                Declaration = null;
            }
            
            if (type.IsArray)
            {
                Type elementType = type.GetElementType() ?? throw new Exception(nameof(Array));
                Reference = $"{getClassName(elementType) ?? new TsInterface(elementType, getClassName, nameMapper).Reference}[]";
                Declaration = null;
            }

            if (type.IsEnum)
            {
                Reference = type.Name;
                Declaration = Enum(type);
            }

            if (type.IsClass || type.IsValueType)
            {
                Reference = type.Name;
                Declaration = FromMembers(type, getClassName, nameMapper);
            }

            throw new Exception($"Unhandled type: {type}");
        }

        private static string Enum(Type type)
        {
            var names = System.Enum.GetNames(type);
            var values = System.Enum.GetValues(type) as object[] ?? throw new Exception("enum");
            var underlyingType = System.Enum.GetUnderlyingType(type);
            var entries = new string[names.Length];

            for (int i = 0; i < names.Length; i++)
            {
                entries[i] = $"{names[i]} = {values[i].As(underlyingType)},";
            }
            
            return @$"export const enum {type.Name} {{
    {String.Join(NewLineIndent, entries)}
}}";
        }

        private static string FromMembers(Type type, Func<Type, string> getClassName, IClrToTsNameMapper nameMapper)
        {
            const BindingFlags flags = BindingFlags.Public |
                                       BindingFlags.Instance |
                                       BindingFlags.DeclaredOnly;

            static bool IsCorrectMemberType(MemberInfo info)
            {
                return info.MemberType switch
                {
                    MemberTypes.Field => true,
                    MemberTypes.Property => true,
                    _ => false
                };
            }

            string GetDeclaration(MemberInfo memberInfo)
            {
                var member = new DataMember(memberInfo);
                string reference = new TsInterface(member.Type, getClassName, nameMapper).Reference;
                return $"{nameMapper.ToTs(member.Name)}: {reference}";
            }

            var members = String.Join(NewLineIndent,
                                      type.GetMembers(flags).Where(IsCorrectMemberType).Select(GetDeclaration));
            
            return @$"export type {type.Name} = {{
    {members}
 }}";
        }
    }
}