using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using pbuddy.TypeScriptingUtility.RuntimeScripts;
using UnityEngine;

namespace pbuddy.TypeScriptingUtility.EditModeTests
{
    public static class APITester
    {
        private static readonly string AssertEquality = "areEqual";
        public static void Test<TAPI, TDomain>(this TAPI api,
                                               string testCode,
                                               Action<TDomain> validate) where TAPI : APIBase<TDomain>, new()
        {
            JsRunner.ExecuteString(testCode, context =>
            {
                context.AddFunction<Action<object, object, string, object[]>>(AssertEquality, Assert.AreEqual);
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