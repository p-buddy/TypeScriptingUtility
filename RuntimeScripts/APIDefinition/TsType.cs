using System;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public class TsType 
    {
        public static string Internalize(string name) => $"internalize_{name}";

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
        public static Shared<TFunc> Function<TFunc>(string name, TFunc func)
            where TFunc : MulticastDelegate =>
            new Shared<TFunc>(func, new TsType(Specification.Function, name));
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <returns></returns>
        public static Shared<Type> Class<TType>() =>
            new Shared<Type>(typeof(TType), new TsType(Specification.Class));
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="item"></param>
        /// <typeparam name="TType"></typeparam>
        /// <returns></returns>
        public static Shared<TType> Variable<TType>(string name, TType item) =>
            new Shared<TType>(item, new TsType(Specification.Variable, name));
        
        public void Match(Action onVariable, Action onClass, Action onFunction)
        {
            switch (Spec)
            {
                case Specification.Class:
                    onClass();
                    return;
                case Specification.Function:
                    onFunction();
                    return;
                case Specification.Variable:
                    onVariable();
                    return;
            }
            throw new ArgumentException($"Unhandled spec type: {Spec}");
        }

        public TReturn Match<TReturn>(Func<TReturn> onVariable, Func<TReturn> onClass, Func<TReturn> onFunction)
        {
            switch (Spec)
            {
                case Specification.Class:
                    return onClass();
                case Specification.Function:
                    return onFunction();
                case Specification.Variable:
                    return onVariable();
            }
            throw new ArgumentException($"Unhandled spec type: {Spec}");
        }
    }
}