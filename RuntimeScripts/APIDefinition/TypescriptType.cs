using System;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public abstract class TypescriptType
    {
        public abstract Type ClrType { get; }
        public abstract Specification Spec { get; }
        public abstract string Name { get; }
        public abstract string[] ParameterNames { get; }
        
        public enum Specification
        {
            Interface,
            Class,
            Function
        }
    }
}