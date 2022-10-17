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

        public TypeWrapper(Type type, IClrToTsNameMapper nameMapper): this()
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
            Dictionary<string, object> dictionary = new()
            {
                { InternalConstructorName(name), ConstructorWrapper }
            };
            // TODO collect all members and wrap funcs for all conversions
            Func<object, int> x = api.ConvertTo<int>;
            dictionary["convertTo_Keyframe"] = x.Wrap();
            return dictionary;
        }

        public static string InternalConstructorName(string name) => $"make_{name}";
        public static string ConvertString(Type type) => $"convertTo_{type.Name}";

    }
}