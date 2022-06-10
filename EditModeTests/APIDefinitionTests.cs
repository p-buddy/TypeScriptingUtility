using System;
using NUnit.Framework;
using pbuddy.TypeScriptingUtility.EditorScripts;
using pbuddy.TypeScriptingUtility.RuntimeScripts;

namespace pbuddy.TypeScriptingUtility.EditModeTests
{
    public class APIDefinitionTests
    {
        private class MyAPI : API
        {
            public override TypescriptType[] Define()
            {
                return new TypescriptType[]
                {
                    TsType<int>.Variable("count"),
                    TsType<Logger>.Variable("console"),
                    TsType<Logger>.Class(),
                    TsType<Func<int>>.Function("init")
                };
            }
        }
        
        [Test]
        public void Define()
        {
            TsGenerator.Content(new MyAPI());
        }
    }
}