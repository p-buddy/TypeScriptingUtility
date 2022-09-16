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
        
        private readonly Type[] parameterTypes;
		private readonly IClrToTsNameMapper mapper;
		private readonly MethodInfo methodInfo;
		private readonly object source;

		public MethodWrapper(object source, MethodInfo methodInfo, IClrToTsNameMapper mapper) : this()
		{
			this.source = source;
			this.methodInfo = methodInfo;
			this.mapper = mapper;

			ParameterInfo[] parameters = methodInfo.GetParameters();
			parameterTypes = parameters.Select(parameter => parameter.ParameterType).ToArray();
			
			int paramsLength = parameterTypes.Length;
			bool isAction = methodInfo.ReturnType == typeof(void);
			bool usesParams = paramsLength != 0 && parameters[paramsLength - 1].UsesParams();
			
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
			
			Type memberType = objectMethodCollection[paramsLength];
			MethodInfo method = memberMethodCollection[paramsLength];
			Delegate = Delegate.CreateDelegate(memberType, this, method);
		}
		
		#region Generated Content

		#region Actions

		public void ActionCastAndInvoke() => methodInfo.Invoke(source, null);
		public void ActionCastAndInvoke(object a) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper)});
		public void ActionCastAndInvoke(object a, object b) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper)});
		public void ActionCastAndInvoke(object a, object b, object c) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper)});
		public void ActionCastAndInvoke(object a, object b, object c, object d) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper)});
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper)});
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper)});
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper)});
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper)});
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper)});
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper)});
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper)});
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper)});
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper)});
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper), n.As(parameterTypes[13], mapper)});
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n, object o) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper), n.As(parameterTypes[13], mapper), o.As(parameterTypes[14], mapper)});
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n, object o, object p) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper), n.As(parameterTypes[13], mapper), o.As(parameterTypes[14], mapper), p.As(parameterTypes[15], mapper)});

		#endregion Actions

		#region Funcs

		public object FuncCastAndInvoke() => methodInfo.Invoke(source, null);
		public object FuncCastAndInvoke(object a) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper)});
		public object FuncCastAndInvoke(object a, object b) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper)});
		public object FuncCastAndInvoke(object a, object b, object c) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper)});
		public object FuncCastAndInvoke(object a, object b, object c, object d) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper)});
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper)});
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper)});
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper)});
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper)});
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper)});
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper)});
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper)});
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper)});
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper)});
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper), n.As(parameterTypes[13], mapper)});
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n, object o) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper), n.As(parameterTypes[13], mapper), o.As(parameterTypes[14], mapper)});
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n, object o, object p) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper), n.As(parameterTypes[13], mapper), o.As(parameterTypes[14], mapper), p.As(parameterTypes[15], mapper)});

		#endregion Funcs

		#region Actions with Params

		public void ParamsActionCastAndInvoke() => methodInfo.Invoke(source, null);
		public void ParamsActionCastAndInvoke(params object[] a) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper)});
		public void ParamsActionCastAndInvoke(object a, params object[] b) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper)});
		public void ParamsActionCastAndInvoke(object a, object b, params object[] c) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper)});
		public void ParamsActionCastAndInvoke(object a, object b, object c, params object[] d) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper)});
		public void ParamsActionCastAndInvoke(object a, object b, object c, object d, params object[] e) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper)});
		public void ParamsActionCastAndInvoke(object a, object b, object c, object d, object e, params object[] f) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper)});
		public void ParamsActionCastAndInvoke(object a, object b, object c, object d, object e, object f, params object[] g) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper)});
		public void ParamsActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, params object[] h) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper)});
		public void ParamsActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, params object[] i) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper)});
		public void ParamsActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, params object[] j) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper)});
		public void ParamsActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, params object[] k) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper)});
		public void ParamsActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, params object[] l) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper)});
		public void ParamsActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, params object[] m) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper)});
		public void ParamsActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, params object[] n) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper), n.As(parameterTypes[13], mapper)});
		public void ParamsActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n, params object[] o) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper), n.As(parameterTypes[13], mapper), o.As(parameterTypes[14], mapper)});
		public void ParamsActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n, object o, params object[] p) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper), n.As(parameterTypes[13], mapper), o.As(parameterTypes[14], mapper), p.As(parameterTypes[15], mapper)});

		#endregion Actions with Params

		#region Funcs with Params

		public object ParamsFuncCastAndInvoke() => methodInfo.Invoke(source, null);
		public object ParamsFuncCastAndInvoke(params object[] a) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper)});
		public object ParamsFuncCastAndInvoke(object a, params object[] b) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper)});
		public object ParamsFuncCastAndInvoke(object a, object b, params object[] c) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper)});
		public object ParamsFuncCastAndInvoke(object a, object b, object c, params object[] d) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper)});
		public object ParamsFuncCastAndInvoke(object a, object b, object c, object d, params object[] e) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper)});
		public object ParamsFuncCastAndInvoke(object a, object b, object c, object d, object e, params object[] f) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper)});
		public object ParamsFuncCastAndInvoke(object a, object b, object c, object d, object e, object f, params object[] g) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper)});
		public object ParamsFuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, params object[] h) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper)});
		public object ParamsFuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, params object[] i) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper)});
		public object ParamsFuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, params object[] j) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper)});
		public object ParamsFuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, params object[] k) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper)});
		public object ParamsFuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, params object[] l) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper)});
		public object ParamsFuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, params object[] m) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper)});
		public object ParamsFuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, params object[] n) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper), n.As(parameterTypes[13], mapper)});
		public object ParamsFuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n, params object[] o) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper), n.As(parameterTypes[13], mapper), o.As(parameterTypes[14], mapper)});
		public object ParamsFuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n, object o, params object[] p) => methodInfo.Invoke(source, new []{a.As(parameterTypes[0], mapper), b.As(parameterTypes[1], mapper), c.As(parameterTypes[2], mapper), d.As(parameterTypes[3], mapper), e.As(parameterTypes[4], mapper), f.As(parameterTypes[5], mapper), g.As(parameterTypes[6], mapper), h.As(parameterTypes[7], mapper), i.As(parameterTypes[8], mapper), j.As(parameterTypes[9], mapper), k.As(parameterTypes[10], mapper), l.As(parameterTypes[11], mapper), m.As(parameterTypes[12], mapper), n.As(parameterTypes[13], mapper), o.As(parameterTypes[14], mapper), p.As(parameterTypes[15], mapper)});

		#endregion Funcs with Params

		#endregion Generated Content
		
		#region Generation Code (Typescript)
/*
const indent1 = "\n\t\t";
const alphabet = 'abcdefghijklmnopqrstuvwxyz'.split("").slice(0, 16);

const makeSignature = (opener: string, params: string, invocations: string) =>
    `${opener}(${params}) => methodInfo.Invoke(${invocations});`;

const generateMethod = (opener: string, useParams: boolean = false) => {
    const withParams = alphabet.map((_, index) => {
        const all: string[] = [...alphabet].splice(0, index + 1);
        const params = all.map((l, i) => (!useParams || i !== index) ? `object ${l}` : `params object[] ${l}`).join(", ");
        const invocations = all.map((l, i) => `${l}.As(parameterTypes[${i}], mapper)`).join(", ");
        return makeSignature(opener, params, `source, new []{${invocations}}`);
    });
    return [makeSignature(opener, "", "source, null"), ...withParams].join(indent1);
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