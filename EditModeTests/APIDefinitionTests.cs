using System;
using NUnit.Framework;
using pbuddy.TypeScriptingUtility.EditorScripts;
using pbuddy.TypeScriptingUtility.RuntimeScripts;
using UnityEngine;

namespace pbuddy.TypeScriptingUtility.EditModeTests
{
    public class APIDefinitionTests
    {
        public void XX(int x)
        {
            
        }
        
        public struct CR
        {
            public Shared<JsLikeConsole> Logger;
            public Shared<Type> LoggerClass;
            public Shared<Action<int, int>> Init;
        }

        private class MyAPI : API<CR>
        {
            protected override CR Define()
            {
                return new CR
                {
                    Logger = TsType.Variable("console", new JsLikeConsole()),
                    LoggerClass = TsType.Class<JsLikeConsole>(),
                    Init = TsType.Function<Action<int, int>>("init",
                                                             (x, y) =>
                                                             {
                                                                 var z = x + x;
                                                             }),
                };
            }
        }

        [Test]
        public void Define()
        {
            var api = new MyAPI();
            var content = TsGenerator.Content(api);
            Debug.Log(content);
            
        }
    }
}