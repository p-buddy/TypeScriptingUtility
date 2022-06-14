using System;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public readonly struct Interlink<T> : ILink
    {
        public T ClrObject { get; }
        public TsType TsType { get; }
        public Type ClrType { get; }
        
        public object NonSpecificClrObject => ClrObject;

        internal Interlink(T obj, TsType tsType)
        {
            ClrObject = obj;
            TsType = tsType;
            ClrType = obj.GetType();
        }
    }
}