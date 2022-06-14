using System;
using System.IO;
using Jint;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public static class JsRunner
    {
        public static void ExecuteString(string js, Action<ExecutionContext> decorator = null)
        {
            var engine = Construct();
            decorator?.Invoke(new ExecutionContext(engine));
            engine.Execute(js);
        }

        public static void ExecuteFile(string path, Action<ExecutionContext> decorator = null)
        {
            ExecuteString(File.ReadAllText(path), decorator);
        }

        static Engine Construct()
        {
            var engine = new Engine();
            var context = new ExecutionContext(engine);
            //context.AddVariable("console", new Logger().Wrap()); // Doesn't work yet!! TODO
            engine.SetValue("console", new Logger());
            return engine;
        }
    }
}
