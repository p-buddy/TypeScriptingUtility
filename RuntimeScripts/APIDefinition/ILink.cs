using System;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public interface ILink
    {
        TsType TsType { get; }
        Type ClrType { get; }
        object NonSpecificClrObject { get; }
    }
}