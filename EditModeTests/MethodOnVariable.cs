using System;
using System.Linq;
using NUnit.Framework;
using pbuddy.TypeScriptingUtility.RuntimeScripts;
using UnityEngine;
using UnityEngine.TestTools;
using Random = System.Random;

namespace pbuddy.TypeScriptingUtility.EditModeTests
{
    public class MethodOnVariable
    {
        public int State { get; private set; }
        public int GetBack(int x) => x;
        public int GetSum(params int[] x) => x.Sum();
        public void LogThis(string y) => Debug.Log(y);

        public class API: APIBase<API.Domain>
        {
            public new struct Domain
            {
                public Shared<MethodOnVariable> Internal;
            }
            
            protected override Domain Define() => new()
            {
                Internal = TsType.Variable(nameof(MethodOnVariable), new MethodOnVariable()),
            };
        }

        private static (string, Action<API.Domain>)[] Cases => new(string, Action<API.Domain>)[]
        {
            (nameof(MethodOnVariable).CallAndCheckForEquality(nameof(GetBack), 4, 4), null),
            (nameof(MethodOnVariable).CallAndCheckForEquality(nameof(GetSum), new []{2, 3}, 5), null),
            (nameof(MethodOnVariable).Call(nameof(LogThis), "hello world"), _ =>
            {
                LogAssert.Expect(LogType.Log, "hello world");
            }),
            (nameof(MethodOnVariable).Set(nameof(State), 123456), domain =>
            {
                Assert.AreEqual(domain.Internal.ClrObject.State, 123456);
            }),
        };
        
        [Test, TestCaseSource(nameof(Cases))]
        public void Test((string, Action<API.Domain>) testCase)
        {
            (string code, Action<API.Domain> assertion) = testCase;
            new API().Test(code, assertion);
        }
    }
}