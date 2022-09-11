using System;
using System.Linq;
using System.Reflection;
using Jint;
using Jint.Runtime.Interop;
using pbuddy.TypeScriptingUtility.EditorScripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public class ExecutionContext
    {
        public Engine Engine => engine;
        private readonly Engine engine;

        public ExecutionContext(Engine engine)
        {
            this.engine = engine;
        }

        public void AddFunction<TFunction>(string name, TFunction function)
        {
            engine.SetValue(name, function);
        }
        
        public void AddVariable<T>(string name, T item)
        {
            engine.SetValue(name, item);
        }
        
        /*
         * class TypescriptClass {
         *  internal: any;
         * 
         *  constructor(arg1, arg2) {
         *      this.internal = make_internal(arg1, arg2);
         *  }
         *
         *  someFunction(arg1) {
         *      internal.someFunction(arg1);
         *  }
         * }
         */

        public void AddType(string name, Type type, IClrToTsNameMapper nameMapper)
        {
            ConstructorInfo[] constructors = type.GetConstructors();
            if (constructors.Length > 1)
            {
                var error = $"In order to add a type to an ${nameof(API<object>)}, it can only have a single public declared constructor.";
                var specifics = $"The type {type.Name} had ${constructors.Length} public declared constructors";
                throw new Exception($"${error} ${specifics}");
            }

            ConstructorInfo constructor = constructors.Length == 1 ? constructors[0] : type.GetConstructor(Type.EmptyTypes);

            Delegate constructorDelegate = new ConstructorWrapper(constructor, nameMapper).Delegate;
            var obj = constructorDelegate.DynamicInvoke(4);
            engine.SetValue($"make_{name}", constructorDelegate);
        }

        public void ApplyAPI<T>(API<T> api)
        {
            ILink[] links = api.Links;
            foreach (ILink link in links)
            {
                string name = api.NameMapper.MapToTs(link.TsType.Name);
                link.TsType.Match(new TsType.Matcher.Action
                {
                    OnVariable = () => AddVariable(name, link.NonSpecificClrObject.Wrap(api.NameMapper)),
                    OnClass = () => AddType(name, link.ClrType, api.NameMapper),
                    OnFunction = () => AddFunction(name, link.NonSpecificClrObject.Wrap(api.NameMapper))
                });
            } 
        }
    }
}