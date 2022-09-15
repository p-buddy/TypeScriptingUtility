using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    internal readonly struct MethodWrapper
	{
        /// <summary>
        /// 
        /// </summary>
        public System.Delegate Delegate { get; }
		
		private static readonly MethodInfo[] FuncMemberMethods;
        private static readonly MethodInfo[] ActionMemberMethods;
		private static readonly MethodInfo[] FuncWithParamsMemberMethods;
		private static readonly MethodInfo[] ActionWithParamsMemberMethods;

        static MethodWrapper()
		{
			Type self = typeof(MethodWrapper);
			ActionMemberMethods = self.GetMatchingMethods(nameof(ActionCastAndInvoke));
            FuncMemberMethods = self.GetMatchingMethods(nameof(FuncCastAndInvoke));
			
			ActionWithParamsMemberMethods = self.GetMatchingMethods(nameof(ParamsActionCastAndInvoke), true);
			FuncWithParamsMemberMethods = self.GetMatchingMethods(nameof(ParamsFuncCastAndInvoke), true);
		}
        
        private readonly System.Delegate wrappedDelegate;
        private readonly Type[] parameterTypes;
		private readonly IClrToTsNameMapper mapper;

		public MethodWrapper(object source, MethodInfo methodInfo, IClrToTsNameMapper mapper) : this()
		{
			ParameterInfo[] parameters = methodInfo.GetParameters();
			parameterTypes = parameters.Select(parameter => parameter.ParameterType).ToArray();
			int paramsLength = parameterTypes.Length;
			bool isAction = methodInfo.ReturnType == typeof(void);
			bool usesParams = paramsLength != 0 && parameters[paramsLength - 1].UsesParams();

			Type[] genericMethodCollection = isAction ? FunctionTypes.GenericActions : FunctionTypes.GenericFunctions;
		
			Type[] objectMethodCollection = isAction
				? usesParams
					? FunctionTypes.ObjectActionsWithParams
					: FunctionTypes.ObjectActions
				: usesParams
					? FunctionTypes.ObjectFunctionsWithParams
					: FunctionTypes.ObjectFunctions;
		
			MethodInfo[] memberMethodCollection = isAction
				? usesParams
					? ActionWithParamsMemberMethods
					: ActionMemberMethods
				: usesParams
					? FuncWithParamsMemberMethods
					: FuncMemberMethods;

			Type[] genericParams = isAction ? parameterTypes : parameterTypes.Append(methodInfo.ReturnType).ToArray();
			Type methodType = isAction && paramsLength == 0
				? genericMethodCollection[paramsLength]
				: genericMethodCollection[paramsLength].MakeGenericType(genericParams);
			
			wrappedDelegate = System.Delegate.CreateDelegate(methodType, source, methodInfo);
			Type memberType = objectMethodCollection[paramsLength];
			MethodInfo method = memberMethodCollection[paramsLength];
			this.mapper = mapper;
			Delegate = System.Delegate.CreateDelegate(memberType, this, method);
		}
		#region Generated Content

		#region Actions

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

		#region Actions with Params

		public void ParamsActionCastAndInvoke() => wrappedDelegate.DynamicInvoke();
		public void ParamsActionCastAndInvoke(params object[] a) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper));
		public void ParamsActionCastAndInvoke(object a, params object[] b) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper));
		public void ParamsActionCastAndInvoke(object a, object b, params object[] c) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper));
		public void ParamsActionCastAndInvoke(object a, object b, object c, params object[] d) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper));
		public void ParamsActionCastAndInvoke(object a, object b, object c, object d, params object[] e) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper));
		public void ParamsActionCastAndInvoke(object a, object b, object c, object d, object e, params object[] f) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper));
		public void ParamsActionCastAndInvoke(object a, object b, object c, object d, object e, object f, params object[] g) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper));
		public void ParamsActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, params object[] h) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper));
		public void ParamsActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, params object[] i) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper));
		public void ParamsActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, params object[] j) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper));
		public void ParamsActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, params object[] k) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper));
		public void ParamsActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, params object[] l) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper));
		public void ParamsActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, params object[] m) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper));
		public void ParamsActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, params object[] n) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper), n.As(parameterTypes[13], mapper));
		public void ParamsActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n, params object[] o) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper), n.As(parameterTypes[13], mapper), o.As(parameterTypes[14], mapper));
		public void ParamsActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n, object o, params object[] p) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper), n.As(parameterTypes[13], mapper), o.As(parameterTypes[14], mapper), p.As(parameterTypes[15], mapper));

		#endregion Actions with Params

		#region Funcs with Params

		public object ParamsFuncCastAndInvoke() => wrappedDelegate.DynamicInvoke();
		public object ParamsFuncCastAndInvoke(params object[] a) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper));
		public object ParamsFuncCastAndInvoke(object a, params object[] b) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper));
		public object ParamsFuncCastAndInvoke(object a, object b, params object[] c) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper));
		public object ParamsFuncCastAndInvoke(object a, object b, object c, params object[] d) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper));
		public object ParamsFuncCastAndInvoke(object a, object b, object c, object d, params object[] e) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper));
		public object ParamsFuncCastAndInvoke(object a, object b, object c, object d, object e, params object[] f) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper));
		public object ParamsFuncCastAndInvoke(object a, object b, object c, object d, object e, object f, params object[] g) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper));
		public object ParamsFuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, params object[] h) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper));
		public object ParamsFuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, params object[] i) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper));
		public object ParamsFuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, params object[] j) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper));
		public object ParamsFuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, params object[] k) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper));
		public object ParamsFuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, params object[] l) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper));
		public object ParamsFuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, params object[] m) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper));
		public object ParamsFuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, params object[] n) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper), n.As(parameterTypes[13], mapper));
		public object ParamsFuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n, params object[] o) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper), n.As(parameterTypes[13], mapper), o.As(parameterTypes[14], mapper));
		public object ParamsFuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n, object o, params object[] p) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper), n.As(parameterTypes[13], mapper), o.As(parameterTypes[14], mapper), p.As(parameterTypes[15], mapper));

		#endregion Funcs with Params

		#endregion Generated Content
		
		#region Generation Code (Typescript)
/*
const indent1 = "\n\t\t";
const alphabet = 'abcdefghijklmnopqrstuvwxyz'.split("").slice(0, 16);

const makeSignature = (opener: string, params: string, invocations: string) =>
    `${opener}(${params}) => wrappedDelegate.DynamicInvoke(${invocations});`;

const generateMethod = (opener: string, useParams: boolean = false) => {
    const withParams = alphabet.map((_, index) => {
        const all: string[] = [...alphabet].splice(0, index + 1);
        const params = all.map((l, i) => (!useParams || i !== index) ? `object ${l}` : `params object[] ${l}`).join(", ");
        const invocations = all.map((l, i) => `${l}.As(parameterTypes[${i}], mapper)`).join(", ");
        return makeSignature(opener, params, invocations);
    });
    return [makeSignature(opener, "", ""), ...withParams].join(indent1);
};

const actionMethod = 'public void ActionCastAndInvoke';
const funcMethod = 'public object FuncCastAndInvoke';
const actionWithParamsMethod = 'public void ParamsActionCastAndInvoke';
const funcWithParamsMethod = 'public object ParamsFuncCastAndInvoke';

const content = `
${indent1}#region Generated Content
${indent1}#region Actions
${indent1}${generateMethod(actionMethod)}
${indent1}#endregion Actions
${indent1}#region Funcs
${indent1}${generateMethod(funcMethod)}
${indent1}#endregion Funcs
${indent1}#region Actions with Params
${indent1}${generateMethod(actionWithParamsMethod, true)}
${indent1}#endregion Actions with Params
${indent1}#region Funcs with Params
${indent1}${generateMethod(funcWithParamsMethod, true)}
${indent1}#endregion Funcs with Params
${indent1}#endregion Generated Content
`;

console.log(content);
*/
		#endregion

	}
}