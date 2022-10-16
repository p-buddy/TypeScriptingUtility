using System;
using System.Collections.Generic;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public readonly struct TsVariable: ITsThing
    {
        public string Declaration { get; }
        public string Reference { get; }

        public TsVariable(IShared shared, in TypeReferenceMap referenceMap)
        {
            string name = shared.TsType.Name;
            Type type = shared.ClrType;
            Declaration = $"{TsGenerator.TsIgnore}{Environment.NewLine}{TsGenerator.ExportConst} {name}: {referenceMap.GetReference(type)} = {TsType.Internalize(name)};";
            Reference = shared.TsType.Name;
        }
    }
}