using System;
using System.Runtime.CompilerServices;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public readonly struct Shared<T> : ILink
    {
        public T ClrObject { get; }
        public TsType TsType { get; }
        public Type ClrType { get; }
        
        public object NonSpecificClrObject => ClrObject;
        
        internal Shared(T obj, TsType tsType)
        {
            ClrObject = obj;
            TsType = tsType;
            ClrType = obj is Type type ? type : obj.GetType();
        }
        
        internal Shared(T obj, TsType tsType, object implementingObject)
        {
            ClrObject = obj;
            TsType = tsType;
            ClrType = obj.GetType();
        }
    }
}