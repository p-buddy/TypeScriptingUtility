using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public readonly struct Shared<T> : IShared
    {
        public T ClrObject => (T)clrObject;

        private readonly object clrObject;
        public TsType TsType { get; }
        public Type ClrType { get; }
        
        public object NonSpecificClrObject => clrObject;
        
        internal Shared(T obj, TsType tsType)
        {
            clrObject = obj;
            TsType = tsType;
            ClrType = obj is Type type ? type : obj.GetType();
        }
    }
}