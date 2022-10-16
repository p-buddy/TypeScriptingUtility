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
        
        private static string InternalConstructorName(string name) => $"make_{name}";

        private static string GetDeclaration(IShared shared, in TypeReferenceMap typeMap)
        {
            const string tab = "\t";
            static string Tab(int count) => String.Concat(Enumerable.Repeat(tab, count));
            const string internalName = "internal";
            const string self = "this." + internalName;

            Type type = shared.ClrType;
            TypeWrapper wrapped = shared.ClrType.Wrap(typeMap.API.NameMapper);
            string name = shared.TsType.Name;
            
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"class {name} {{");
            builder.AppendLine(tab + $"private {internalName}: any;");

            TypeReferenceMap localMap = typeMap;
            string GetParamText((Type type, string name) param) => $"{param.name}: {localMap.GetReference(param.type)}";
            string paramsText = string.Join(", ", wrapped.ConstructorParams?.Select(GetParamText) ?? Array.Empty<string>());

            builder.Append(@$"
{Tab(1)}constructor({paramsText}) {{ 
{Tab(2)}{TsGenerator.TsIgnore}
{Tab(2)}{self} = {InternalConstructorName(name)}(...arguments); 
{Tab(1)}}}
");
               
            foreach (MemberInfo member in TsReference.GetMembersRequiringDeclaration(type, in typeMap))
            {
                string memberName = typeMap.API.NameMapper.ToTs(member.Name);
                string internalMember = $"{self}.{memberName}";

                switch (member.MemberType)
                {
                    case MemberTypes.Field:
                    case MemberTypes.Property:
                        DataMember dataMember = new (member);
                        string typeRef = typeMap.GetReference(dataMember.Type);
                        builder.AppendLine(tab + $"get {memberName}(): {typeRef} {{ return {internalMember}; }}");
                        builder.AppendLine(tab + $"set {memberName}(value: {typeRef}) {{ {internalMember} = value; }}");
                        continue;
                    case MemberTypes.Method:
                        MethodInfo method = member as MethodInfo;
                        Assert.IsNotNull(method);
                        string returnStatement = method.ReturnType == typeof(void) ? "" : "return ";

                        builder.AppendLine(@$"
{Tab(1)}{TsReference.GetMemberDeclaration(member, in typeMap, false)} {{ 
{Tab(2)}{returnStatement}{self}.{memberName}(...arguments); 
{Tab(1)}}}
");
                        continue;
                    case MemberTypes.Constructor:
                        continue; // ignore
                }
            }

            builder.AppendLine("}");
            return builder.ToString();
        }
    }
}