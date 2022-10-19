using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine.Assertions;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public readonly struct TsClass: ITsThing
    {
        public string Declaration { get; }
        public string Reference { get; }

        public TsClass(IShared shared, in TypeReferenceMap typeMap)
        {
            Reference = shared.TsType.Name;
            Declaration = GetDeclaration(shared, in typeMap); 
        }
        
        private static string GetDeclaration(IShared shared, in TypeReferenceMap typeMap)
        {
            const string tab = "\t";
            static string Tab(int count, string content = "") => $"{String.Concat(Enumerable.Repeat(tab, count))}{content}";
            const string internalName = "internal";
            const string self = "this." + internalName;

            Type type = shared.ClrType;
            TypeWrapper wrapped = shared.ClrType.Wrap(typeMap.API.NameMapper);
            string name = shared.TsType.Name;
            
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"export class {name} {{");
            builder.AppendLine(tab + $"private {internalName}: any;");

            TypeReferenceMap localMap = typeMap;

            string GetParamText(ParameterInfo param) =>
                $"{(param.UsesParams() ? "..." : "")}{param.Name}: {localMap.GetReference(param.ParameterType)}";

            string paramsText = (wrapped.ConstructorParams?.Select(GetParamText) ?? Array.Empty<string>()).Csv();

            builder.Append(@$"
{Tab(1)}constructor({paramsText}) {{ 
{Tab(2, TsGenerator.TsIgnore)}
{Tab(2)}{self} = {TypeWrapper.InternalConstructorName(name)}(...arguments); 
{Tab(1)}}}
");
               
            foreach (MemberInfo member in TsReference.GetMembersRequiringDeclaration(type, in typeMap))
            {
                string tsName = typeMap.API.NameMapper.ToTs(member.Name);

                switch (member.MemberType)
                {
                    case MemberTypes.Field:
                    case MemberTypes.Property:
                        DataMember dataMember = new (member);
                        Type memberType = dataMember.Type;
                        string typeRef = typeMap.GetReference(dataMember.Type);
                        string internalMember = $"{self}.{member.Name}";
                        builder.AppendLine(tab + TsGenerator.TsIgnore);
                        builder.AppendLine(tab + $"get {tsName}(): {typeRef} {{ return {TypeWrapper.InternalWrapName}({internalMember}); }}");
                        builder.AppendLine(tab + TsGenerator.TsIgnore);
                        builder.AppendLine(tab + $"set {tsName}(value: {typeRef}) {{ {internalMember} = {TypeWrapper.InternalConverterName(memberType)}(value); }}");
                        continue;
                    case MemberTypes.Method:
                        MethodInfo method = member as MethodInfo;
                        Assert.IsNotNull(method);
                        
                        builder.AppendLine(@$"
{Tab(1)}{TsReference.GetMemberDeclaration(member, in typeMap, false)} {{ 
{Tab(2, TsGenerator.TsIgnore)}
{Tab(2)}{ReturnStatement(method)}
{Tab(1)}}}
");
                        continue;
                    case MemberTypes.Constructor:
                        continue; // ignore
                }
            }

            builder.AppendLine("}");
            return builder.ToString();

            static string ReturnStatement(MethodInfo method)
            {
                bool returnsValue = method.ReturnType != typeof(void);
                string call = $"{self}.{method.Name}({method.GetParameters().Select(ConvertedParameter).Csv()})";
                if (!returnsValue) return $"{call};";
                return $"return {TypeWrapper.InternalWrapName}({call});";
            }

            static string ConvertedParameter(ParameterInfo parameter) =>
                $"{TypeWrapper.InternalConverterName(parameter.ParameterType)}({parameter.Name})";
        }
    }
}