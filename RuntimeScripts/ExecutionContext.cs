using System.Collections.Generic;
using Jint;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public readonly struct ExecutionContext
    {
        public Engine Engine => engine;
        private readonly Engine engine;

        public ExecutionContext(Engine engine)
        {
            this.engine = engine;
        }

        public void AddFunction<TFunction>(string name, TFunction function)
        {
            engine.SetValue(TsType.Internalize(name), function);
        }
        
        public void AddVariable<T>(string name, T item)
        {
            engine.SetValue(TsType.Internalize(name), item);
        }

        public void AddType(string name, TypeWrapper wrappedType, IAPI api)
        {
            foreach (KeyValuePair<string, object> global in wrappedType.GetGlobalsToAdd(name, api))
            {
                engine.SetValue(global.Key, global.Value);
            }
        }

        public void ApplyAPI<T>(APIBase<T> api)
        {
            ExecutionContext self = this;
            IShared[] links = api.Shared;
            foreach (IShared link in links)
            {
                string name = api.NameMapper.MapToTs(link.TsType.Name);
                link.TsType.Match(new TsType.Matcher.Action
                {
                    OnVariable = () => self.AddVariable(name, link.NonSpecificClrObject.Wrap(api.NameMapper)),
                    OnClass = () => self.AddType(link.TsType.Name, link.ClrType.Wrap(api.NameMapper), api),
                    OnFunction = () => self.AddFunction(name, link.NonSpecificClrObject.Wrap(api.NameMapper))
                });
            } 
        }
    }
}