using System;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public class TsType<TType> : TypescriptType
    {
        public override Specification Spec { get; }
        public override Type ClrType { get; }
        public override string Name { get; }
        public override string[] ParameterNames { get; }

        private TsType(Specification specification, Type type, string name = null, string[] paramNames = null)
        {
            Spec = specification;
            ClrType = type;
            Name = name;
            ParameterNames = paramNames;
        }
        
        public static TsType<TType> Function(string name) => new TsType<TType>(Specification.Function, typeof(TType), name);
        public static TsType<TType> Class() => new TsType<TType>(Specification.Class, typeof(TType));
        public static TsType<TType> Variable(string name) => new TsType<TType>(Specification.Interface, typeof(TType), name);
    }
}