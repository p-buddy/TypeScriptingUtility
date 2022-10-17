using System;
using System.Collections.Generic;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public interface IAPI
    {
        IClrToTsNameMapper NameMapper { get; }
        IShared[] Shared { get; }
        Dictionary<Type, IShared> this[TsType.Specification spec] { get; }
        T ConvertTo<T>(object obj);
    }
}