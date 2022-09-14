using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public readonly struct ConstructorWrapper
    {
        private static readonly MethodInfo AsMethod;
		private static readonly MethodInfo[] InvokeMethods;
		private static readonly MethodInfo[] InvokeMethodsWithParams;
		static ConstructorWrapper()
        {
            AsMethod = typeof(JsToClrConverter).GetMethod(nameof(JsToClrConverter.As));
			Type self = typeof(ConstructorWrapper);
			InvokeMethods = self.GetMatchingMethods(nameof(Invoke));
			InvokeMethodsWithParams = self.GetMatchingMethods(nameof(InvokeWithParams), true);
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

			bool containsParams = parameters.Length > 0 && parameters[^1].UsesParams();
			Type methodType = containsParams
				? FunctionTypes.ObjectFunctionsWithParams[parameters.Length]
				: FunctionTypes.ObjectFunctions[parameters.Length];
			MethodInfo method = containsParams
				? InvokeMethodsWithParams[parameters.Length]
				: InvokeMethods[parameters.Length];
			
            NewExpression construct = Expression.New(info, castedParameters);
            MethodCallExpression wrapped = Expression.Call(WrapperFactory.WrapObjectMethod, Expression.Convert(construct, typeof(object)), mapperConstant);
            LambdaExpression lambda = Expression.Lambda(wrapped, true, expressionParameters);
            lambdaDelegate = lambda.Compile();
			Delegate = Delegate.CreateDelegate(methodType, this, method);
		}
        
#region Generated Content

		public object Invoke() => lambdaDelegate.DynamicInvoke();
		public object Invoke(object a) => lambdaDelegate.DynamicInvoke(a);
		public object Invoke(object a, object b) => lambdaDelegate.DynamicInvoke(a, b);
		public object Invoke(object a, object b, object c) => lambdaDelegate.DynamicInvoke(a, b, c);
		public object Invoke(object a, object b, object c, object d) => lambdaDelegate.DynamicInvoke(a, b, c, d);
		public object Invoke(object a, object b, object c, object d, object e) => lambdaDelegate.DynamicInvoke(a, b, c, d, e);
		public object Invoke(object a, object b, object c, object d, object e, object f) => lambdaDelegate.DynamicInvoke(a, b, c, d, e, f);
		public object Invoke(object a, object b, object c, object d, object e, object f, object g) => lambdaDelegate.DynamicInvoke(a, b, c, d, e, f, g);
		public object Invoke(object a, object b, object c, object d, object e, object f, object g, object h) => lambdaDelegate.DynamicInvoke(a, b, c, d, e, f, g, h);
		public object Invoke(object a, object b, object c, object d, object e, object f, object g, object h, object i) => lambdaDelegate.DynamicInvoke(a, b, c, d, e, f, g, h, i);
		public object Invoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j) => lambdaDelegate.DynamicInvoke(a, b, c, d, e, f, g, h, i, j);
		public object Invoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k) => lambdaDelegate.DynamicInvoke(a, b, c, d, e, f, g, h, i, j, k);
		public object Invoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l) => lambdaDelegate.DynamicInvoke(a, b, c, d, e, f, g, h, i, j, k, l);
		public object Invoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m) => lambdaDelegate.DynamicInvoke(a, b, c, d, e, f, g, h, i, j, k, l, m);
		public object Invoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n) => lambdaDelegate.DynamicInvoke(a, b, c, d, e, f, g, h, i, j, k, l, m, n);
		public object Invoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n, object o) => lambdaDelegate.DynamicInvoke(a, b, c, d, e, f, g, h, i, j, k, l, m, n, o);
		public object Invoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n, object o, object p) => lambdaDelegate.DynamicInvoke(a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p);

		public object InvokeWithParams() => lambdaDelegate.DynamicInvoke();
		public object InvokeWithParams(params object[] a) => lambdaDelegate.DynamicInvoke(a);
		public object InvokeWithParams(object a, params object[] b) => lambdaDelegate.DynamicInvoke(a, b);
		public object InvokeWithParams(object a, object b, params object[] c) => lambdaDelegate.DynamicInvoke(a, b, c);
		public object InvokeWithParams(object a, object b, object c, params object[] d) => lambdaDelegate.DynamicInvoke(a, b, c, d);
		public object InvokeWithParams(object a, object b, object c, object d, params object[] e) => lambdaDelegate.DynamicInvoke(a, b, c, d, e);
		public object InvokeWithParams(object a, object b, object c, object d, object e, params object[] f) => lambdaDelegate.DynamicInvoke(a, b, c, d, e, f);
		public object InvokeWithParams(object a, object b, object c, object d, object e, object f, params object[] g) => lambdaDelegate.DynamicInvoke(a, b, c, d, e, f, g);
		public object InvokeWithParams(object a, object b, object c, object d, object e, object f, object g, params object[] h) => lambdaDelegate.DynamicInvoke(a, b, c, d, e, f, g, h);
		public object InvokeWithParams(object a, object b, object c, object d, object e, object f, object g, object h, params object[] i) => lambdaDelegate.DynamicInvoke(a, b, c, d, e, f, g, h, i);
		public object InvokeWithParams(object a, object b, object c, object d, object e, object f, object g, object h, object i, params object[] j) => lambdaDelegate.DynamicInvoke(a, b, c, d, e, f, g, h, i, j);
		public object InvokeWithParams(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, params object[] k) => lambdaDelegate.DynamicInvoke(a, b, c, d, e, f, g, h, i, j, k);
		public object InvokeWithParams(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, params object[] l) => lambdaDelegate.DynamicInvoke(a, b, c, d, e, f, g, h, i, j, k, l);
		public object InvokeWithParams(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, params object[] m) => lambdaDelegate.DynamicInvoke(a, b, c, d, e, f, g, h, i, j, k, l, m);
		public object InvokeWithParams(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, params object[] n) => lambdaDelegate.DynamicInvoke(a, b, c, d, e, f, g, h, i, j, k, l, m, n);
		public object InvokeWithParams(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n, params object[] o) => lambdaDelegate.DynamicInvoke(a, b, c, d, e, f, g, h, i, j, k, l, m, n, o);
		public object InvokeWithParams(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n, object o, params object[] p) => lambdaDelegate.DynamicInvoke(a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p);

		#endregion Generated Content
		
		#region Generation Code (Typescript)
/*
const indent1 = "\n\t\t";
const alphabet = 'abcdefghijklmnopqrstuvwxyz'.split("").slice(0, 16);

const makeSignature = (opener: string, params: string, invocations: string) =>
    `${opener}(${params}) => lambdaDelegate.DynamicInvoke(${invocations});`;

const generateMethods = (opener: string, useParams: boolean = false) => {
    const withParams = alphabet.map((_, index) => {
        const length = index + 1;
        const all: string[] = [...alphabet].splice(0, length);
        const params = all.map((l, i) => i === length - 1 && useParams ? `params object[] ${l}` : `object ${l}`).join(", ");
        const invocations = all.join(", ");
        return makeSignature(opener, params, invocations);
    });
    return [makeSignature(opener, "", ""), ...withParams].join(indent1);
};

const invocation = 'public object Invoke';
const paramsInvocation = 'public object InvokeWithParams';


const content = `
${indent1}#region Generated Content
${indent1}${generateMethods(invocation)}
${indent1}${generateMethods(paramsInvocation, true)}
${indent1}#endregion Generated Content
`;

console.log(content);
*/
		#endregion
	}
}