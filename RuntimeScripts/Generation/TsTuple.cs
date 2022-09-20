using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pbuddy.TypeScriptingUtility.RuntimeScripts;

namespace pbuddy.TypeScriptingUtility.EditorScripts
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

        public TsTuple(Type type, IAPI containingAPI)
        {
            Reference = "null";
            //Reference = $"[{String.Join(", ", type.GetGenericArguments().Select(t => new TsInterface(t, containingAPI).Reference))}]";
        }
    }
}