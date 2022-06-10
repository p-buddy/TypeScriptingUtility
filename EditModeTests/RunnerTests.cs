using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using pbuddy.TypeScriptingUtility.RuntimeScripts;

namespace pbuddy.TypeScriptingUtility.EditModeTests
{
    public class RunnerTests
    {
        [Test]
        public void Log()
        {
            string testString = "This is a javascript log!";
            LogAssert.Expect(LogType.Log, testString);
            JsRunner.ExecuteString($"console.log(\"{testString}\")");
        }
        
        [Test]
        public void Warn()
        {
            string testString = "Be careful with javascript!";
            LogAssert.Expect(LogType.Warning, testString);
            JsRunner.ExecuteString($"console.warn(\"{testString}\")");
        }
        
        [Test]
        public void Error()
        {
            string testString = "Uh oh!! JS had an error";
            LogAssert.Expect(LogType.Error, testString);
            JsRunner.ExecuteString($"console.error(\"{testString}\")");
        }

        public struct O
        {
            public int Double(int x)
            {
                return x + x;
            }

            public int Other(O o, int x)
            {
                return o.Double(x);
            }
        }
        
        private struct Y
        {
            public int g;
        }

        private class X
        {
            public void Z(Y y)
            {
                
            }
            
            public void W(Action<int> z)
            {
                
            }
        }
        
        [Test]
        public void More()
        {
            string testString = @"function x() {return 3;}
console.log(x);";
            JsRunner.ExecuteString(testString, context =>
            {
                context.AddType<O>("o");
                context.AddType<X>("X");
            });
        }
    }
}