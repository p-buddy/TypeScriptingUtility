using System;
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
		
		private static readonly MethodInfo[] FuncMemberMethods;
        private static readonly MethodInfo[] ActionMemberMethods;

        static MethodWrapper()
        {
            static Type[] ArgTypes(int count) => Enumerable.Repeat(typeof(object), count).ToArray();
            static MethodInfo GetMethodForArgCount(string name, int count) =>
                typeof(MethodWrapper).GetMethod(name, ArgTypes(count));
            static MethodInfo[] GetMethods(string name) => Enumerable.Range(0, FunctionTypes.Count)
                                                               .Select(i => GetMethodForArgCount(name, i))
                                                               .ToArray();

            ActionMemberMethods = GetMethods(nameof(ActionCastAndInvoke));
            FuncMemberMethods = GetMethods(nameof(FuncCastAndInvoke));
        }
        
        private readonly Delegate wrappedDelegate;
        private readonly Type[] parameterTypes;
		private readonly IClrToTsNameMapper mapper;

        public MethodWrapper(object source, MethodInfo methodInfo, IClrToTsNameMapper mapper) : this()
		{
			ParameterInfo[] parameters = methodInfo.GetParameters();
			parameterTypes = parameters.Select(parameter => parameter.ParameterType).ToArray();
			int paramsLength = parameterTypes.Length;
			bool isAction = methodInfo.ReturnType == typeof(void);
			// bool usesParams = paramsLength != 0 && UsesParams(parameters[paramsLength - 1]); TODO
			Type methodType = isAction
				? paramsLength == 0 ? FunctionTypes.GenericActions[paramsLength] : FunctionTypes.GenericActions[paramsLength].MakeGenericType(parameterTypes)
				: FunctionTypes.GenericFunctions[paramsLength].MakeGenericType(parameterTypes.Append(methodInfo.ReturnType).ToArray());
			wrappedDelegate = Delegate.CreateDelegate(methodType, source, methodInfo);
			Type memberType = isAction ? FunctionTypes.ObjectActions[paramsLength] : FunctionTypes.ObjectFunctions[paramsLength];
			MethodInfo method = isAction ? ActionMemberMethods[paramsLength] : FuncMemberMethods[paramsLength];
			this.mapper = mapper;
			Delegate = Delegate.CreateDelegate(memberType, this, method);
		}

		private static bool UsesParams(ParameterInfo parameter) => parameter.GetCustomAttributes(typeof(ParamArrayAttribute), false).Length > 0;

		#region Generated Content

		#region Actions
		// public void ActionCastAndInvoke(params object[] a) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper)); TODO
		public void ActionCastAndInvoke() => wrappedDelegate.DynamicInvoke();
		public void ActionCastAndInvoke(object a) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper));
		public void ActionCastAndInvoke(object a, object b) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper));
		public void ActionCastAndInvoke(object a, object b, object c) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper));
		public void ActionCastAndInvoke(object a, object b, object c, object d) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper));
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper));
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper));
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper));
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper));
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper));
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper));
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper));
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper));
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper));
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper), n.As(parameterTypes[13], mapper));
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n, object o) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper), n.As(parameterTypes[13], mapper), o.As(parameterTypes[14], mapper));
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n, object o, object p) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper), n.As(parameterTypes[13], mapper), o.As(parameterTypes[14], mapper), p.As(parameterTypes[15], mapper));

		#endregion Actions

		#region Funcs

		public object FuncCastAndInvoke() => wrappedDelegate.DynamicInvoke();
		public object FuncCastAndInvoke(object a) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper));
		public object FuncCastAndInvoke(object a, object b) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper));
		public object FuncCastAndInvoke(object a, object b, object c) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper));
		public object FuncCastAndInvoke(object a, object b, object c, object d) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper));
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper));
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper));
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper));
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper));
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper));
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper));
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper));
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper));
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper));
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper), n.As(parameterTypes[13], mapper));
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n, object o) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper), n.As(parameterTypes[13], mapper), o.As(parameterTypes[14], mapper));
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n, object o, object p) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper), n.As(parameterTypes[13], mapper), o.As(parameterTypes[14], mapper), p.As(parameterTypes[15], mapper));

		#endregion Funcs

		#endregion Generated Content
		
		#region Generation Code (Typescript)
/*
const indent1 = "\n\t\t";
const alphabet = 'abcdefghijklmnopqrstuvwxyz'.split("").slice(0, 16);

const makeSignature = (opener: string, params: string, invocations: string) =>
    `${opener}(${params}) => wrappedDelegate.DynamicInvoke(${invocations});`;

const generateMethod = (opener: string) => {
    const withParams = alphabet.map((_, index) => {
        const all: string[] = [...alphabet].splice(0, index + 1);
        const params = all.map(l => `object ${l}`).join(", ");
        const invocations = all.map((l, i) => `${l}.As(parameterTypes[${i}], mapper)`).join(", ");
        return makeSignature(opener, params, invocations);
    });
    return [makeSignature(opener, "", ""), ...withParams].join(indent1);
};

const actionMethod = 'public void ActionCastAndInvoke';
const funcMethod = 'public object FuncCastAndInvoke';

const content = `
${indent1}#region Generated Content
${indent1}#region Actions
${indent1}${generateMethod(actionMethod)}
${indent1}#endregion Actions
${indent1}#region Funcs
${indent1}${generateMethod(funcMethod)}
${indent1}#endregion Funcs
${indent1}#endregion Generated Content
`;

console.log(content);
*/
		#endregion

	}
}