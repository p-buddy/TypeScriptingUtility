using System;
using System.Collections.Generic;
using pbuddy.TypeScriptingUtility.RuntimeScripts;

namespace pbuddy.TypeScriptingUtility.EditorScripts
{
    public readonly struct TsVariable: ITsThing
    {
        public string Declaration { get; }
        public string Reference { get; }

        public TsVariable(IShared shared, Dictionary<Type, ITsThing> typeMap)
        {
            string name = shared.TsType.Name;
            Type type = shared.ClrType;
            Declaration = $"{TsGenerator.ExportConst} {name}: {typeMap[type].Reference} = {TsType.Internalize(name)};";
            Reference = shared.TsType.Name;
        }
    }
}