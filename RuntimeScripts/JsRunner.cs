using System;
using System.IO;
using System.Linq;
using Jint;
using UnityEditor;
using UnityEngine;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public static class JsRunner
    {
        private static readonly Engine TsToJsEngine;
        static JsRunner()
        {
            TsToJsEngine = Construct();
            TsToJsEngine.Execute(GetTsToJsSource());
            TsToJsEngine.SetValue("console", new Logger());

            string GetTsToJsSource()
            {
                string path = AssetDatabase.FindAssets("tsToJs")
                                           .ToList()
                                           .Select(AssetDatabase.GUIDToAssetPath)
                                           .First(path => path.EndsWith("Assets/tsToJs.txt"));
                return File.ReadAllText(path);
            }
        }

        public static void CompileTs(string code) => TsToJsEngine.Execute($"console.log(tsToJs(`{code}`))");
        
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
            engine.SetValue("console", new Logger());
            return engine;
        }
    }
}
