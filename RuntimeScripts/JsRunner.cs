using System;
using System.IO;
using System.Linq;
using Jint;
using UnityEditor;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public static class JsRunner
    {
        private static readonly Engine TsToJsEngine;
        private const string TsToJsFile = "tsToJs.txt";
        static JsRunner()
        {
            TsToJsEngine = Construct();
            TsToJsEngine.SetValue("global", new object());
            TsToJsEngine.SetValue("console", new JsLikeConsole());
            TsToJsEngine.Execute(GetTsToJsSource());

            string GetTsToJsSource()
            {
                string path = AssetDatabase.FindAssets(Path.GetFileNameWithoutExtension(TsToJsFile))
                                           .ToList()
                                           .Select(AssetDatabase.GUIDToAssetPath)
                                           .First(path => path.EndsWith(TsToJsFile));
                return File.ReadAllText(path);
            }
        }

        public static string CompileTs(string code) => TsToJsEngine.Evaluate($"tsToJs(`{code}`)").AsString();
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
            engine.SetValue("console", new JsLikeConsole());
            return engine;
        }
    }
}
