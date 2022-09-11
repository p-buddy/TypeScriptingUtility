using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public readonly struct ConstructorWrapper
    {
        private static readonly MethodInfo AsMethod;
        private static readonly MethodInfo WrapMethod;
		private static readonly MethodInfo[] InvokeMethods;
		
		private const int CastMethodsCount = 2;

		static ConstructorWrapper()
        {
            AsMethod = typeof(JsToClrConverter).GetMethod(nameof(JsToClrConverter.As));
            WrapMethod = typeof(WrapperFactory).GetMethod(nameof(WrapperFactory.Wrap));
			
			static Type[] ArgTypes(int count) => Enumerable.Repeat(typeof(object), count).ToArray();
			static MethodInfo GetMethodForArgCount(string name, int count) =>
				typeof(ConstructorWrapper).GetMethod(name, ArgTypes(count));
			static MethodInfo[] GetMethods(string name) => Enumerable.Range(0, CastMethodsCount)
																	 .Select(i => GetMethodForArgCount(name, i))
																	 .ToArray();

			InvokeMethods = GetMethods(nameof(Invoke));
        }

		public Delegate Delegate { get; }
		private readonly Delegate lambdaDelegate;

		public ConstructorWrapper(ConstructorInfo info, IClrToTsNameMapper mapper): this()
        {
            ParameterInfo[] parameters = info.GetParameters();
            Expression mapperConstant = Expression.Convert(Expression.Constant(mapper), typeof(IClrToTsNameMapper));

            ParameterExpression[] expressionParameters = new ParameterExpression[parameters.Length];
            Expression[] castedParameters = new Expression[parameters.Length];
            
            for (var index = 0; index < parameters.Length; index++)
            {
                Type type = parameters[index].ParameterType;
                ParameterExpression expressionParameter = Expression.Parameter(typeof(object));
                Expression[] callParams = { expressionParameter, Expression.Constant(type), mapperConstant };
                expressionParameters[index] = expressionParameter;
                castedParameters[index] = Expression.Convert(Expression.Call(AsMethod, callParams), type);
				
            }
            
            NewExpression construct = Expression.New(info, castedParameters);
            MethodCallExpression wrapped = Expression.Call(WrapMethod, Expression.Convert(construct, typeof(object)), mapperConstant);
            LambdaExpression lambda = Expression.Lambda(wrapped, true, expressionParameters);
            lambdaDelegate = lambda.Compile();
			Delegate = Delegate.CreateDelegate(lambdaDelegate.GetType(), this, InvokeMethods[parameters.Length]);
		}
        
        #region Funcs

		public object Invoke() => lambdaDelegate.DynamicInvoke();
		public object Invoke(object a) => lambdaDelegate.DynamicInvoke(a);
		#endregion Funcs
        
    }
}