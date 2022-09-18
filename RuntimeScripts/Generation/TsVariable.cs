using System;
using System.Collections.Generic;
using pbuddy.TypeScriptingUtility.RuntimeScripts;

namespace pbuddy.TypeScriptingUtility.EditorScripts
{
    public readonly struct TsVariable
    {
        public string Declaration { get; }
        
        public TsVariable(IShared shared, Dictionary<Type, TsDeclaration> declarations)
        {
            string name = shared.TsType.Name;
            Type type = shared.ClrType;
            Declaration = $"export const {name}: {declarations[type].Reference} = {TsType.Internalize(name)};";
        }
    }
}