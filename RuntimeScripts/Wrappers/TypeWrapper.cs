using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
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
        public Delegate ConstructorWrapper { get; }
        public ParameterInfo[] ConstructorParams { get; }

        private readonly MemberInfo[] wrappedMembers;

        public TypeWrapper(Type type, MemberInfo[] wrappedMembers, IClrToTsNameMapper nameMapper): this()
        {
            this.type = type;
            this.wrappedMembers = wrappedMembers;
            
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

            if (constructorInfo is null)
            {
                ConstructorWrapper = new MethodWrapper(this, CreateInstanceMethod, nameMapper).Delegate;
                ConstructorParams = null;
            }
            else
            {
                ConstructorWrapper constructorWrapper = new ConstructorWrapper(constructorInfo, nameMapper);
                ConstructorWrapper = constructorWrapper.Delegate;
                ConstructorParams = constructorWrapper.Parameters;
            }
        }

        public object CreateInstance() => Activator.CreateInstance(type);

        public Dictionary<string, object> GetGlobalsToAdd(string name, IAPI api)
        {
            Dictionary<string, object> dictionary = new(1 + wrappedMembers.Length)
            {
                { InternalConstructorName(name), ConstructorWrapper }
            };

            void AddType(Type toAdd)
            {
                string converter = InternalConverterName(toAdd);
                if (dictionary.ContainsKey(converter)) return;
                dictionary[converter] = api.MakeConvertToMethod(toAdd);
            }

            foreach (MemberInfo member in wrappedMembers)
            {
                switch (member.MemberType)
                {
                    case MemberTypes.Field:
                        var field = member as FieldInfo;
                        Assert.IsNotNull(field);
                        AddType(field.FieldType);
                        break;
                    case MemberTypes.Property:
                        var property = member as PropertyInfo;
                        Assert.IsNotNull(property);
                        AddType(property.PropertyType);
                        break;
                    case MemberTypes.Method:
                        var method = member as MethodInfo;
                        Assert.IsNotNull(method);
                        foreach (ParameterInfo parameter in method.GetCachedParameters())
                        {
                            AddType(parameter.ParameterType);
                        }
                        break;
                }
            }
            
            return dictionary;
        }

        public static string InternalConstructorName(string name) => $"make_{name}";
        public static string InternalConverterName(Type type) => $"convertTo_{type.Name.Replace("[]", "s").Replace("`", "_")}";
        
        public const string InternalWrapName = "wrap";

    }
}