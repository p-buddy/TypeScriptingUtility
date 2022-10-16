using System;
using System.Collections.Generic;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public readonly struct TsTuple: ITsThing
    {
        private static readonly HashSet<Type> ValueTupleTypes = new (new []
        {
            typeof(ValueTuple<>),
            typeof(ValueTuple<,>),
            typeof(ValueTuple<,,>),
            typeof(ValueTuple<,,,>),
            typeof(ValueTuple<,,,,>),
            typeof(ValueTuple<,,,,,>),
            typeof(ValueTuple<,,,,,,>),
            typeof(ValueTuple<,,,,,,,>)
        });
        
        public static bool IsTuple(Type type) => type.IsGenericType && ValueTupleTypes.Contains(type.GetGenericTypeDefinition());
        public string Declaration => null;
        public string Reference { get; }
    }
}