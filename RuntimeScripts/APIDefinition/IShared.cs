using System;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public interface IShared
    {
        TsType TsType { get; }
        Type ClrType { get; }
        object NonSpecificClrObject { get; }
    }
}