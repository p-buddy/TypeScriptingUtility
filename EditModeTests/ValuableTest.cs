using System;
using System.Collections.Generic;
using NUnit.Framework;
using pbuddy.TypeScriptingUtility.EditorScripts;
using pbuddy.TypeScriptingUtility.RuntimeScripts;
using UnityEngine;

namespace pbuddy.TypeScriptingUtility.EditModeTests
{
    public class ValuableTest
    {
        public enum Dummy
        {
            Howdy = 0,
            Hi = 1,
            Hello = 2,
        }
        
        public readonly struct MyStruct: IEquatable<MyStruct>
            {
                public float A { get; }
                public int B { get; }
                public Dummy C { get; }

                public MyStruct(float a, int b, Dummy c)
                {
                    A = a;
                    B = b;
                    C = c;
                }
                
                public string Declaration => $"{{ a: {A}, b: {B}, c: {(int)C} }}";

                public bool Equals(MyStruct other)
                {
                    return A.Equals(other.A) && B == other.B && C == other.C;
                }

                public override bool Equals(object obj)
                {
                    return obj is MyStruct other && Equals(other);
                }

                public override int GetHashCode()
                {
                    return HashCode.Combine(A, B, (int)C);
                }

                public override string ToString() => Declaration;
            }
            
            public readonly struct Valuable<T>: IValuable<T>, IEquatable<Valuable<T>> where T : IEquatable<T>, new()
            {
                public T Value { get; }
                public Dummy Auxiliary { get; }
                public bool Flag { get; }
                
                public Valuable(T value)
                {
                    Value = value;
                    Auxiliary = default;
                    Flag = default;
                }
                
                public Valuable(T value, Dummy auxiliary, bool flag)
                {
                    Value = value;
                    Auxiliary = auxiliary;
                    Flag = flag;
                }
                
                public string ValueOnlyDeclaration => $"{Value}";
                public string Declaration => $"{{ value: {Value}, auxiliary: {Auxiliary}, flag: {Flag} }}";

                public bool Equals(Valuable<T> other)
                {
                    return EqualityComparer<T>.Default.Equals(Value, other.Value) && Auxiliary == other.Auxiliary && Flag == other.Flag;
                }

                public override bool Equals(object obj)
                {
                    return obj is Valuable<T> other && Equals(other);
                }

                public override int GetHashCode()
                {
                    return HashCode.Combine(Value, (int)Auxiliary, Flag);
                }
            }
            
        public class API: APIBase<API.Domain>
        {
            public override IClrToTsNameMapper NameMapper => ClrToTsNameMapper.PascalToCamelCase;


            public new struct Domain
            {
                public float Primitive;
                public Shared<Action<Valuable<float>, Valuable<MyStruct>>> Function;
            }

            protected override Domain Define() => new()
            {
                Function = TsType.Function<Action<Valuable<float>, Valuable<MyStruct>>>(TestA.name,
                                           (primitive, @struct) =>
                                           {
                                               Assert.AreEqual(primitive, TestA.primitive);
                                               Assert.AreEqual(@struct, TestA.@struct);
                                           })
            };
        }

        private static readonly (string name, Valuable<float> primitive, Valuable<MyStruct> @struct) TestA = (
            "testA", new Valuable<float>(1.5f), new Valuable<MyStruct>(new MyStruct(3.4f, 5, Dummy.Hello)));

        private static (string, Action<API.Domain>)[] Cases => new(string, Action<API.Domain>)[]
        {
            ($"{TestA.name}( {TestA.primitive.ValueOnlyDeclaration}, {TestA.@struct.ValueOnlyDeclaration})", (domain) =>
            {
            }),
            ($"{TestA.name}({{ value: {TestA.primitive.ValueOnlyDeclaration} }}, {TestA.@struct.ValueOnlyDeclaration})", (domain) =>
            {
                
            }),
            ($"{TestA.name}( {TestA.primitive.ValueOnlyDeclaration}, {{ value: {TestA.@struct.ValueOnlyDeclaration} }} )", (domain) =>
            {
                
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