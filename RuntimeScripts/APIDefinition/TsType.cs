using System;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public class TsType 
    {
        public enum Specification
        {
            Variable,
            Class,
            Function
        }
        
        public Specification Spec { get; }
        public string Name { get; }

        private TsType(Specification specification, string name = null)
        {
            Spec = specification;
            Name = name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="func"></param>
        /// <typeparam name="TFunc"></typeparam>
        /// <returns></returns>
        public static Interlink<TFunc> Function<TFunc>(string name, TFunc func)
            where TFunc : MulticastDelegate =>
            new Interlink<TFunc>(func, new TsType(Specification.Function, name));
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <returns></returns>
        public static Interlink<Type> Class<TType>() =>
            new Interlink<Type>(typeof(TType), new TsType(Specification.Class));
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="item"></param>
        /// <typeparam name="TType"></typeparam>
        /// <returns></returns>
        public static Interlink<TType> Variable<TType>(string name, TType item) =>
            new Interlink<TType>(item, new TsType(Specification.Variable, name));
    }
}