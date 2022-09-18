using System;
using NUnit.Framework;
using pbuddy.TypeScriptingUtility.RuntimeScripts;


namespace pbuddy.TypeScriptingUtility.EditModeTests
{
    public class TestTemplate
    {
        public class API: APIBase<API.Domain>
        {
            public new struct Domain
            {
            }

            protected override Domain Define() => new()
            {
                
            };
        }
        
        private static (string, Action<API.Domain>)[] Cases => new(string, Action<API.Domain>)[]
        {
        };

        [Test, TestCaseSource(nameof(Cases))]
        public void Test((string, Action<API.Domain>) testCase)
        {
            (string code, Action<API.Domain> assertion) = testCase;
            new API().Test(code, assertion);
        }
    }
}