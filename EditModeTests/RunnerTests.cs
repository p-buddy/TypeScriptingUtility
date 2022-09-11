using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using pbuddy.TypeScriptingUtility.RuntimeScripts;
using Debug = UnityEngine.Debug;
using Random = System.Random;

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

        private (string, long) CompileAndGetAverageMilliseconds(string code)
        {
            string result = JsRunner.CompileTs(code); // Run once so JS Runner initialize

            Stopwatch stopwatch = new Stopwatch();
            const int iterations = 10;
            for (int i = 0; i < iterations; i++)
            {
                stopwatch.Start();
                JsRunner.CompileTs(code);
                stopwatch.Stop();
            }

            return (result, stopwatch.ElapsedMilliseconds / iterations);
        }

        [Test]
        public void CompileSmallTypescript()
        {
            const string sample = "const x: string = \"hello\"";

            (string result, long elapsedMs) = CompileAndGetAverageMilliseconds(sample);
            Debug.Log(result);
            Debug.Log($"Average execution time: {elapsedMs}ms");
        }

        private struct Globals
        {
            public List<int> Numbers;
            public Shared<Action<int>> AppendNumber;
            public Shared<Action<int>> AppendNumberLambda;
        }

        private class MyAPI : API<Globals>
        {
            protected override Globals Define()
            {
                List<int> numbers = new List<int>();
                return new Globals
                {
                    Numbers = numbers,
                    AppendNumber = TsType.Function<Action<int>>("append", numbers.Add),
                    AppendNumberLambda = TsType.Function<Action<int>>("lambda", i => numbers.Add(i)),
                };
            }
        }

        [Test]
        public void APISimple()
        {
            var random = new Random();
            var testValues = new [] { random.Next(), random.Next() };
            
            string testString = @$"
append({testValues[0]});
lambda({testValues[1]})";
            MyAPI api = new MyAPI();
            
            JsRunner.ExecuteString(testString, context =>
            {
                context.ApplyAPI(api);
            });
            
            Assert.AreEqual(testValues[0], api.Domain.Numbers[0]);
            Assert.AreEqual(testValues[1], api.Domain.Numbers[1]);
        }

        private struct Powers
        {
            public int Root;
            public int Exponent { get; set; }
        }
        
        private struct Real
        {
            public List<int> ClrTally;
            public Shared<List<int>> JsTally;
            public Shared<Func<Powers, int>> SquareAndCube;
            public Shared<Test> Test;
            public Shared<Action<int[]>> TakeArr;
            public Shared<Type> TestType;
        }
        
        private struct Test
        {
            public Test(int x)
            {
                this.x = x;
            }
            
            public int x;
            public int Bab
            {
                get
                {
                    Debug.Log("hi");
                    return 5;   
                }
                set => Debug.Log("oh");
            }
        }

        private class RealisticAPI : API<Real>
        {
            public override IClrToTsNameMapper NameMapper => ClrToTsNameMapper.PascalToCamelCase;

            protected override Real Define()
            {
                var clrTally = new List<int>();
                return new Real
                {
                    ClrTally = clrTally,
                    JsTally = TsType.Variable("tally", new List<int>()),
                    Test = TsType.Variable("test", new Test()),
                    TestType = TsType.Class<Test>(nameof(Test)),
                    SquareAndCube = TsType.Function<Func<Powers, int>>("eval", powers =>
                    {
                        int result = (int)Math.Pow(powers.Root, powers.Exponent);
                        clrTally.Add(result);
                        return result;
                    }),
                    TakeArr = TsType.Function<Action<int[]>>("take", (int[] arr) =>
                    {
                        
                    })
                };
            }
        }
        
        
        
        [Test]
        public void APIComplex()
        {
            var random = new Random();
            var testValues = new [] { random.Next(), random.Next() };

            string testString = @$"
take([3,2]);
test.x = 3;
const y = make_test(11);
console.log(y.x);
tally.add(eval({{ root: 2, exponent: 4 }}));
take([1]);
console.log(tally[1]);
test.x = 5;
test.bab = 2;
";
            
            new RealisticAPI().Test(testString,
                                    (Real domain) =>
                                    {
                                        Debug.Log(domain.ClrTally[0]);
                                        //Assert.AreEqual(domain.ClrTally[0], domain.JsTally.ClrObject[0]);
                                    });
        }
    }
}