using System;
using System.Linq;
using Jint;
using Jint.Runtime.Interop;
using pbuddy.TypeScriptingUtility.EditorScripts;
using UnityEditor;
using UnityEngine;

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

        public void AddType<T>(string name)
        {
            engine.SetValue(name, TypeReference.CreateTypeReference(engine, typeof(T)));
        }
        
        public void AddType(string name, Type type)
        {
            engine.SetValue(name, TypeReference.CreateTypeReference(engine, type));
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
                    OnClass = () => AddType(name, link.ClrType),
                    OnFunction = () => AddFunction(name, link.NonSpecificClrObject.Wrap(api.NameMapper))
                });
            } 
        }
    }
}