using System;
using System.Collections.Generic;
using pbuddy.TypeScriptingUtility.RuntimeScripts;

namespace pbuddy.TypeScriptingUtility.EditorScripts
{
    public readonly struct TsClass: ITsThing
    {
        public string Declaration { get; }
        public string Reference { get; }

        public TsClass(IShared shared, Dictionary<Type, ITsThing> typeMap)
        {
            Reference = shared.TsType.Name;
            Declaration = "";
        }
    }
}