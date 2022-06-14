using System;
using NUnit.Framework;
using pbuddy.TypeScriptingUtility.EditorScripts;
using pbuddy.TypeScriptingUtility.RuntimeScripts;
using UnityEngine;
using Logger = pbuddy.TypeScriptingUtility.RuntimeScripts.Logger;

namespace pbuddy.TypeScriptingUtility.EditModeTests
{
    public class APIDefinitionTests
    {
        public void XX(int x)
        {
            
        }
        
        public struct CR
        {
            public Interlink<Logger> Logger;
            public Interlink<Type> LoggerClass;
            public Interlink<Action<int, int>> Init;
        }

        private class MyAPI : API<CR>
        {
            protected override CR Make()
            {
                return new CR
                {
                    Logger = TsType.Variable("console", new Logger()),
                    LoggerClass = TsType.Class<Logger>(),
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
            var api = TsGenerator.Content(new MyAPI());
            Debug.Log(api);
        }
    }
}