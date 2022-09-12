using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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

        public void AddType(string name, TypeWrapper wrappedType)
        {
            foreach (KeyValuePair<string, object> global in wrappedType.GetGlobalsToAdd(name))
            {
                engine.SetValue(global.Key, global.Value);
            }
            engine.Execute(wrappedType.GetClassDeclaration(name));
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
                    OnClass = () => AddType(name, link.ClrType.Wrap(api.NameMapper)),
                    OnFunction = () => AddFunction(name, link.NonSpecificClrObject.Wrap(api.NameMapper))
                });
            } 
        }
    }
}