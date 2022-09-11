using System;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public class TsType 
    {
        public static class Matcher
        {
            public delegate void ActionDelegate();
            public delegate TReturn FuncDelegate<out TReturn>();

            public class Action
            {
                public ActionDelegate OnVariable;
                public ActionDelegate OnClass;
                public ActionDelegate OnFunction;
            }
        
        
            public class Func<TReturn>
            {
                public FuncDelegate<TReturn> OnVariable;
                public FuncDelegate<TReturn> OnClass;
                public FuncDelegate<TReturn> OnFunction;
            }
        }
        
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
            where TFunc : MulticastDelegate => new(func, new TsType(Specification.Function, name));
        
        /// <summary>
        /// Handled completely on TS side?
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <returns></returns>
        public static Shared<Type> Class<TType>(string name) => new(typeof(TType), new TsType(Specification.Class, name));
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="item"></param>
        /// <typeparam name="TType"></typeparam>
        /// <returns></returns>
        public static Shared<TType> Variable<TType>(string name, TType item) => new(item, new TsType(Specification.Variable, name));

        public void Match(Matcher.Action matcher)
        {
            switch (Spec)
            {
                case Specification.Class:
                    matcher.OnClass();
                    return;
                case Specification.Function:
                    matcher.OnFunction();
                    return;
                case Specification.Variable:
                    matcher.OnVariable();
                    return;
            }
            throw new ArgumentException($"Unhandled spec type: {Spec}");
        }

        public TReturn Match<TReturn>(Matcher.Func<TReturn> matcher)
        {
            switch (Spec)
            {
                case Specification.Class:
                    return matcher.OnClass();
                case Specification.Function:
                    return matcher.OnFunction();
                case Specification.Variable:
                    return matcher.OnVariable();
            }
            throw new ArgumentException($"Unhandled spec type: {Spec}");
        }
    }
}