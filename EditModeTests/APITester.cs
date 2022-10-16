using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using pbuddy.TypeScriptingUtility.RuntimeScripts;

namespace pbuddy.TypeScriptingUtility.EditModeTests
{
    public static class APITester
    {
        private static readonly string AssertEquality = "areEqual";
        public delegate void AssertEqualDelegate(object expected, object actual, string message, object[] args);

        public static readonly Shared<AssertEqualDelegate> AssertEqual = TsType.Function<AssertEqualDelegate>("areEqual", Assert.AreEqual);
        public static void Test<TAPI, TDomain>(this TAPI api,
                                               string testCode,
                                               Action<TDomain> validate) where TAPI : APIBase<TDomain>, new()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(api.Generate());
            builder.AppendLine(testCode);
            string fullCode = JsRunner.CompileTs(builder.ToString());
            
            JsRunner.ExecuteString(fullCode, context =>
            {
                context.Engine.SetValue("exports", new object());
                context.AddFunction("exports", new object());
                context.ApplyAPI(api);
            });
            validate?.Invoke(api.Domain);
        }

        public static string ToInput<T>(this T input)
        {
            switch (input)
            {
                case int:
                    return $"{input}";
                case string:
                    return $"\"{input}\"";
                case Array array:
                {
                    List<string> items = new List<string>();
                
                    foreach (object item in array)
                    {
                        items.Add($"{item}");
                    }

                    return String.Join(", ", items);
                }
                default:
                    return "";
            }
        }

        public static string Set<TInput>(this string variableName, string fieldName, TInput input)
        {
            return $"{variableName}.{fieldName} = {input.ToInput()};";
        }

        public static string Call<TInput>(this string variableName, string funcName, TInput input)
        {
            return $"{variableName}.{funcName}({input.ToInput()});";
        }
        
        public static string Call<TInput>(this string funcName, TInput input)
        {
            return $"{funcName}({input.ToInput()});";
        }
        
        public static string CallAndCheckForEquality<TInput, TExpected>(this string funcName,
                                                                        TInput input,
                                                                        TExpected expected)
        {
            return $"{AssertEquality}({funcName}({input.ToInput()}), {expected});";
        }
        
        public static string CallAndCheckForEquality<TInput, TExpected>(this string variableName,
                                                                        string memberMethod,
                                                                        TInput input,
                                                                        TExpected expected)
        {
            return $"{AssertEquality}({variableName}.{memberMethod}({input.ToInput()}), {expected});";
        }
    }
}