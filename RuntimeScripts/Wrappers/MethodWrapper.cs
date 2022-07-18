using System;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    internal readonly struct MethodWrapper
    {
        /// <summary>
        /// 
        /// </summary>
        public Delegate Delegate { get; }

        private const int CastMethodsCount = 3;

        private static readonly Type[] GenericFuncTypes = { typeof(Func<>), typeof(Func<,>), typeof(Func<,,>), typeof(Func<,,,>) };
        private static readonly Type[] GenericActionTypes = { typeof(Action), typeof(Action<>), typeof(Action<,>), typeof(Func<,,>), typeof(Func<,,,>) };

        private static readonly Type[] NonSpecificFuncTypes =
        {
            typeof(Func<object>),
            typeof(Func<object, object>),
            typeof(Func<object, object, object>),
            typeof(Func<object, object, object>),
        };
        
        private static readonly Type[] NonSpecificActionTypes =
        {
            typeof(Action),
            typeof(Action<object>),
            typeof(Action<object, object>),
            typeof(Action<object, object, object>),
            typeof(Action<object, object, object>),
        };

        private static readonly MethodInfo[] FuncMemberMethods;
        private static readonly MethodInfo[] ActionMemberMethods;

        static MethodWrapper()
        {
            static Type[] ArgTypes(int count) => Enumerable.Repeat(typeof(object), count).ToArray();
            static MethodInfo GetMethodForArgCount(string name, int count) =>
                typeof(MethodWrapper).GetMethod(name, ArgTypes(count));
            static MethodInfo[] GetMethods(string name) => Enumerable.Range(0, CastMethodsCount)
                                                               .Select(i => GetMethodForArgCount(name, i))
                                                               .ToArray();

            ActionMemberMethods = GetMethods(nameof(ActionCastAndInvoke));
            FuncMemberMethods = GetMethods(nameof(FuncCastAndInvoke));
        }
        
        private readonly Delegate wrappedDelegate;
        private readonly Type[] parameterTypes;

        public MethodWrapper(object source, MethodInfo methodInfo) : this()
        {
            parameterTypes = methodInfo.GetParameters().Select(parameter => parameter.ParameterType).ToArray();
            int paramsLength = parameterTypes.Length;
            bool isAction = methodInfo.ReturnType == typeof(void);
            Type methodType = isAction
                ? GenericActionTypes[paramsLength].MakeGenericType(parameterTypes)
                : GenericFuncTypes[paramsLength].MakeGenericType(parameterTypes.Append(methodInfo.ReturnType).ToArray());
            wrappedDelegate = Delegate.CreateDelegate(methodType, source, methodInfo);
            Type memberType = isAction ? NonSpecificActionTypes[paramsLength] : NonSpecificFuncTypes[paramsLength];
            MethodInfo method = isAction ? ActionMemberMethods[paramsLength] : FuncMemberMethods[paramsLength];
            Delegate = Delegate.CreateDelegate(memberType, this, method);
        }

        #region Actions
        public void ActionCastAndInvoke() => wrappedDelegate.DynamicInvoke();
        public void ActionCastAndInvoke(object a) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]));
        public void ActionCastAndInvoke(object a, object b) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]));
        #endregion

        #region Funcs
        public object FuncCastAndInvoke() => wrappedDelegate.DynamicInvoke();
        public object FuncCastAndInvoke(object a) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]));
        public object FuncCastAndInvoke(object a, object b) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]));

        #endregion

    }
}