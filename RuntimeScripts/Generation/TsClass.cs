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
            static string Tab(int count) => String.Concat(Enumerable.Repeat(tab, count));
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
{Tab(2)}{TsGenerator.TsIgnore}
{Tab(2)}{self} = {TypeWrapper.InternalConstructorName(name)}(...arguments); 
{Tab(1)}}}
");
               
            foreach (MemberInfo member in TsReference.GetMembersRequiringDeclaration(type, in typeMap))
            {
                string tsName = typeMap.API.NameMapper.ToTs(member.Name);
                string internalMember = $"{self}.{member.Name}";

                switch (member.MemberType)
                {
                    case MemberTypes.Field:
                    case MemberTypes.Property:
                        DataMember dataMember = new (member);
                        string typeRef = typeMap.GetReference(dataMember.Type);
                        builder.AppendLine(tab + $"get {tsName}(): {typeRef} {{ return {internalMember}; }}");
                        builder.AppendLine(tab + $"set {tsName}(value: {typeRef}) {{ {internalMember} = value; }}");
                        continue;
                    case MemberTypes.Method:
                        MethodInfo method = member as MethodInfo;
                        Assert.IsNotNull(method);
                        string returnStatement = method.ReturnType == typeof(void) ? "" : "return ";

                        Func<object, int> x = typeMap.API.ConvertTo<int>;
                        
                        // covert_int(4);
                        // convertTo_KeyFrame(..)
                        builder.AppendLine(@$"
{Tab(1)}{TsReference.GetMemberDeclaration(member, in typeMap, false)} {{ 
{Tab(2)}//{returnStatement}{self}.{member.Name}({method.GetParameters().Select(p => p.Name).Csv()}); 
{Tab(1)}}}
");
                        continue;
                    case MemberTypes.Constructor:
                        continue; // ignore
                }
            }

            // TODO
            // returned objects should also be converted to expando's before being returned
            
            /**
             * return Expand(this.internal.AddFrame(convertTo_KeyFrame(frame)));
             */
            
            builder.AppendLine("}");
            return builder.ToString();
        }
    }
}