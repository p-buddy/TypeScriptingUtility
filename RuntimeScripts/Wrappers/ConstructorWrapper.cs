using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public readonly struct ConstructorWrapper
    {
		private static readonly MethodInfo[] InvokeMethods;
		private static readonly MethodInfo[] InvokeMethodsWithParams;
		static ConstructorWrapper()
        {
			Type self = typeof(ConstructorWrapper);
			InvokeMethods = self.GetMatchingMethods(nameof(Invoke));
			InvokeMethodsWithParams = self.GetMatchingMethods(nameof(InvokeWithParams), true);
		}

		public Delegate Delegate { get; }
		public ParameterInfo[] Parameters { get; }

		private readonly IClrToTsNameMapper mapper;
		private readonly ConstructorInfo constructorInfo;
		private readonly Type[] parameterTypes;
		
		public ConstructorWrapper(ConstructorInfo info, IClrToTsNameMapper mapper): this()
		{
			constructorInfo = info;
			Parameters = info.GetParameters();
			parameterTypes = Parameters.Select(parameter => parameter.ParameterType).ToArray();
			
			bool containsParams = Parameters.Length > 0 && Parameters[^1].UsesParams();
			Type methodType = containsParams
				? FunctionTypes.ObjectFunctionsWithParams[Parameters.Length]
				: FunctionTypes.ObjectFunctions[Parameters.Length];
			MethodInfo method = containsParams
				? InvokeMethodsWithParams[Parameters.Length]
				: InvokeMethods[Parameters.Length];

			this.mapper = mapper;
			Delegate = Delegate.CreateDelegate(methodType, this, method);
		}

		#region Generated Content
		public object Invoke() => constructorInfo.Invoke(null);
		public object Invoke(object a) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper)});
		public object Invoke(object a, object b) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper)});
		public object Invoke(object a, object b, object c) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper)});
		public object Invoke(object a, object b, object c, object d) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper)});
		public object Invoke(object a, object b, object c, object d, object e) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper)});
		public object Invoke(object a, object b, object c, object d, object e, object f) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper)});
		public object Invoke(object a, object b, object c, object d, object e, object f, object g) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper)});
		public object Invoke(object a, object b, object c, object d, object e, object f, object g, object h) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper)});
		public object Invoke(object a, object b, object c, object d, object e, object f, object g, object h, object i) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper)});
		public object Invoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper)});
		public object Invoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper)});
		public object Invoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper)});
		public object Invoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper)});
		public object Invoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper), n.As(parameterTypes[13], mapper)});
		public object Invoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n, object o) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper), n.As(parameterTypes[13], mapper), o.As(parameterTypes[14], mapper)});
		public object Invoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n, object o, object p) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper), n.As(parameterTypes[13], mapper), o.As(parameterTypes[14], mapper), p.As(parameterTypes[15], mapper)});

		public object InvokeWithParams() => constructorInfo.Invoke(null);
		public object InvokeWithParams(params object[] a) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper)});
		public object InvokeWithParams(object a, params object[] b) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper)});
		public object InvokeWithParams(object a, object b, params object[] c) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper)});
		public object InvokeWithParams(object a, object b, object c, params object[] d) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper)});
		public object InvokeWithParams(object a, object b, object c, object d, params object[] e) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper)});
		public object InvokeWithParams(object a, object b, object c, object d, object e, params object[] f) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper)});
		public object InvokeWithParams(object a, object b, object c, object d, object e, object f, params object[] g) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper)});
		public object InvokeWithParams(object a, object b, object c, object d, object e, object f, object g, params object[] h) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper)});
		public object InvokeWithParams(object a, object b, object c, object d, object e, object f, object g, object h, params object[] i) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper)});
		public object InvokeWithParams(object a, object b, object c, object d, object e, object f, object g, object h, object i, params object[] j) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper)});
		public object InvokeWithParams(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, params object[] k) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper)});
		public object InvokeWithParams(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, params object[] l) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper)});
		public object InvokeWithParams(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, params object[] m) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper)});
		public object InvokeWithParams(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, params object[] n) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper), n.As(parameterTypes[13], mapper)});
		public object InvokeWithParams(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n, params object[] o) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper), n.As(parameterTypes[13], mapper), o.As(parameterTypes[14], mapper)});
		public object InvokeWithParams(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n, object o, params object[] p) => constructorInfo.Invoke(new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper), n.As(parameterTypes[13], mapper), o.As(parameterTypes[14], mapper), p.As(parameterTypes[15], mapper)});

		#endregion Generated Content

		
		#region Generation Code (Typescript)
/*
const indent1 = "\n\t\t";
const alphabet = 'abcdefghijklmnopqrstuvwxyz'.split("").slice(0, 16);

const makeSignature = (opener: string, params: string, invocations: string) =>
    `${opener}(${params}) => constructorInfo.Invoke(${invocations});`;

const generateMethods = (opener: string, useParams: boolean = false) => {
    const As = (i: number): string => `.As(parameterTypes[${i}], mapper)`
    const withParams = alphabet.map((_, index) => {
        const length = index + 1;
        const all: string[] = [...alphabet].splice(0, length);
        const params = all.map((l, i) => i === length - 1 && useParams ? `params object[] ${l}` : `object ${l}`).join(", ");
        const asStatements = all.map((l,i) => `${l}${As(i)}`).join(", ");
        const invocations = `new []{${asStatements}}`;
        return makeSignature(opener, params, invocations);
    });
    return [makeSignature(opener, "", "null"), ...withParams].join(indent1);
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