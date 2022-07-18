using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using pbuddy.TypeScriptingUtility.RuntimeScripts;

namespace pbuddy.TypeScriptingUtility.EditModeTests
{
    public class RunnerTests
    {
        private readonly struct EqualityAsserter
        {
            public static string Name => "checkEqual";

            public static void Apply(ExecutionContext context)
            {
                context.AddFunction<Action<object, object>>(Name, Assert.AreEqual);
            }
        }
        
        [Test]
        public void Log()
        {
            string testString = "This is a javascript log!";
            LogAssert.Expect(LogType.Log, testString);
            JsRunner.ExecuteString($"console.log([\"{testString}\"])");
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

        private struct Calculator
        {
            public float Divide(Quotient q)
            {
                float expected = q.answer;
                float actual = (float)q.numerator / q.denominator;
                Assert.AreEqual(expected, actual);
                return actual;
            }
        }
        
        private struct Quotient 
        {
            public int numerator { get; set; }
            public int denominator;
            public float answer;

            public Quotient(int n, int d)
            {
                numerator = n;
                denominator = d;
                answer = (float)n / d;
            }
            
            public string JsObjectDeclaration =>
                $"{{{nameof(numerator)}: {numerator}, {nameof(denominator)}: {denominator}, {nameof(answer)}: {answer}}}";

            public static string JsDestructure => $"const {{ {nameof(numerator)}, {nameof(denominator)}, {nameof(answer)} }}";
            public static string MemberNames = string.Join(", ", nameof(numerator), nameof(denominator), nameof(answer));
        }

        [Test]
        public void ClrFunction()
        {
            Calculator calc = new Calculator();
            
            var quotient = new Quotient
            {
                numerator = 9,
                denominator = 4,
                answer = (float)9 / 4
            };
            
            string testString = @$"
const {nameof(Quotient)} = {quotient.JsObjectDeclaration};
const result = {nameof(calc)}.{nameof(calc.Divide)}({nameof(Quotient)});
{EqualityAsserter.Name}(result, {quotient.answer});
console.log(result);
"; 
            JsRunner.ExecuteString(testString, context =>
            {
                context.AddVariable(nameof(calc), calc.Wrap());
                EqualityAsserter.Apply(context);
            });
        }

        [Test]
        public void ReturnNonPrimitive()
        {
            var quotient = new Quotient(10, 20);
            
            string funcName = "get";
            string testString = @$"
{Quotient.JsDestructure} = {funcName}();
{EqualityAsserter.Name}({nameof(Quotient.numerator)}, {quotient.numerator});
{EqualityAsserter.Name}({nameof(Quotient.denominator)}, {quotient.denominator});
{EqualityAsserter.Name}({nameof(Quotient.answer)}, {quotient.answer});
console.log({Quotient.MemberNames});
";
            JsRunner.ExecuteString(testString, context =>
            {
                context.AddFunction<Func<Quotient>>(funcName, () => quotient);
                EqualityAsserter.Apply(context);
            });
        }

        private struct Globals
        {
            public List<int> Numbers;
            public Interlink<Action<int>> AppendNumber;
        }

        private class MyAPI : API<Globals>
        {
            protected override Globals Make()
            {
                List<int> numbers = new List<int>();
                return new Globals
                {
                    Numbers = new List<int>(),
                    AppendNumber = TsType.Function<Action<int>>("append", i => numbers.Add(i))
                };
            }
        }

        [Test]
        public void API()
        {
            string testString = @$"";
            var api = new MyAPI();
            
            JsRunner.CompileTs("const x: string = \"hello\"");

            JsRunner.ExecuteString(testString, context =>
            {
                context.ApplyAPI(api);
            });
        }
    }
}