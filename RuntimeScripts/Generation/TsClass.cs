using System;
using System.Collections.Generic;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public readonly struct TsClass: ITsThing
    {
        public string Declaration { get; }
        public string Reference { get; }

        public TsClass(IShared shared, Dictionary<Type, TsReference> typeMap)
        {
            Reference = shared.TsType.Name;
            Declaration = "";
        }
    }
}