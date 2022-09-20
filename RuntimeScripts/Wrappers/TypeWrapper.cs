using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine.Assertions;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public readonly struct TypeWrapper
    {
        private static readonly MethodInfo CreateInstanceMethod;

        static TypeWrapper()
        {
            CreateInstanceMethod = typeof(TypeWrapper).GetMatchingMethods(nameof(CreateInstance))[0];
        }

        private readonly Type type;
        public Delegate ConstructorConstructorWrapper { get; }
        public Dictionary<string, MemberInfo> MembersByName { get; }

        public TypeWrapper(Type type, MemberInfo[] members, IClrToTsNameMapper nameMapper): this()
        {
            this.type = type;
            
            ConstructorInfo[] constructors = type.GetConstructors();
            if (constructors.Length > 1)
            {
                var error = $"In order to add a type to an ${nameof(APIBase<object>)}, it can only have a single public declared constructor.";
                var specifics = $"The type {type.Name} had ${constructors.Length} public declared constructors";
                throw new Exception($"${error} ${specifics}");
            }

            ConstructorInfo constructorInfo = constructors.Length == 1
                ? constructors[0]
                : type.GetConstructor(Type.EmptyTypes);
            ConstructorConstructorWrapper = constructorInfo is null 
                ? new MethodWrapper(this, CreateInstanceMethod, nameMapper).Delegate
                : new ConstructorWrapper(constructorInfo, nameMapper).Delegate;
            
            MembersByName = new Dictionary<string, MemberInfo>();
            foreach (MemberInfo member in members)
            {
                MembersByName[nameMapper.ToTs(member.Name)] = member;
            }
        }

        public object CreateInstance() => Activator.CreateInstance(type);

        public Dictionary<string, object> GetGlobalsToAdd(string name)
        {
            return new Dictionary<string, object>
            {
                { InternalConstructorName(name), ConstructorConstructorWrapper }
            };
        }

        public string GetJsClassDeclaration(string name)
        {
            const string tab = "\t";
            const string internalName = "this.internal";
            
            StringBuilder classBuilder = new StringBuilder();
            classBuilder.AppendLine($"class {name} {{");
            classBuilder.AppendLine(tab + $"constructor() {{ {internalName} = {InternalConstructorName(name)}(...arguments); }}");
            
            foreach (KeyValuePair<string, MemberInfo> kvp in MembersByName)
            {
                switch (kvp.Value.MemberType)
                {
                    case MemberTypes.Field:
                    case MemberTypes.Property:
                        classBuilder.AppendLine(tab + $"get {kvp.Key}() {{ return {internalName}.{kvp.Key}; }}");
                        classBuilder.AppendLine(tab + $"set {kvp.Key}(value) {{ {internalName}.{kvp.Key} = value; }}");
                        continue;
                    case MemberTypes.Method:
                        MethodInfo method = kvp.Value as MethodInfo;
                        Assert.IsNotNull(method);
                        string returnStatement = method.ReturnType == typeof(void) ? "" : "return";
                        classBuilder.AppendLine(tab + $"{kvp.Key}() {{ {returnStatement} {internalName}.{kvp.Key}(...arguments); }}");
                        continue;
                    case MemberTypes.Constructor:
                        continue; // ignore
                }
            }
            classBuilder.AppendLine("}");
            return classBuilder.ToString();
        }

        private static string InternalConstructorName(string name) => $"make_{name}";
    }
}