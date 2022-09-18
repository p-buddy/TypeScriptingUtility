using System;
using System.Collections.Generic;
using pbuddy.TypeScriptingUtility.RuntimeScripts;

namespace pbuddy.TypeScriptingUtility.EditorScripts
{
    public readonly struct TsClass
    {
        public string Declaration { get; }
        public TsClass(IShared shared, Dictionary<Type, TsDeclaration> declarations)
        {
            Declaration = "";
        }
    }
}