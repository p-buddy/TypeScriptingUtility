using Jint;
using Jint.Runtime.Interop;

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
    }
}