using System;
using pbuddy.TypeScriptingUtility.RuntimeScripts;

namespace pbuddy.TypeScriptingUtility.EditModeTests
{
    public static class APITester
    {
        public static void Test<TAPI, TDomain>(this TAPI api,
                                               string testCode,
                                               Action<TDomain> validate) where TAPI : API<TDomain>, new()
        {
            JsRunner.ExecuteString(testCode, context =>
            {
                context.ApplyAPI(api);
            });
            validate(api.Domain);
        }
    }
}