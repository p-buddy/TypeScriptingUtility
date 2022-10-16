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
        private static readonly string Name = ClrToTsNameMapper.PascalToCamelCase.ToTs(nameof(MethodOnVariable));
        
        public int State { get; private set; }
        public int GetBack(int x) => x;
        public int GetSum(params int[] x) => x.Sum();
        public void LogThis(string y) => Debug.Log(y);

        public class API: APIBase<API.Domain>
        {
            public new struct Domain
            {
                public Shared<APITester.AssertEqualDelegate> Assert;
                public Shared<MethodOnVariable> Internal;
            }
            
            protected override Domain Define() => new()
            {
                Internal = TsType.Variable(Name, new MethodOnVariable()),
                Assert = APITester.AssertEqual
            };
        }

        private static (string, Action<API.Domain>)[] Cases => new(string, Action<API.Domain>)[]
        {
            (Name.CallAndCheckForEquality(nameof(GetBack), 4, 4), null),
            (Name.CallAndCheckForEquality(nameof(GetSum), new []{2, 3}, 5), null),
            (Name.Call(nameof(LogThis), "hello world"), _ =>
            {
                LogAssert.Expect(LogType.Log, "hello world");
            }),
            (Name.Set(nameof(State), 123456), domain =>
            {
                Assert.AreEqual(domain.Internal.ClrObject.State, 123456);
            }),
        };
        
        [Test, TestCaseSource(nameof(Cases))]
        public void Test((string, Action<API.Domain>) testCase)
        {
            (string code, Action<API.Domain> assertion) = testCase;
            new API().Test(code, assertion);
            Debug.Log(new API().Generate());
        }
    }
}