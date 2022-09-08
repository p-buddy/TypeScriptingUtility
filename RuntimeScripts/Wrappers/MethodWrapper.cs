using System;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    internal readonly struct MethodWrapper
    {
        /// <summary>
        /// 
        /// </summary>
        public Delegate Delegate { get; }

		private const int CastMethodsCount = 17;

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
		private readonly IClrToTsNameMapper mapper;

        public MethodWrapper(object source, MethodInfo methodInfo, IClrToTsNameMapper mapper) : this()
        {
			parameterTypes = methodInfo.GetParameters().Select(parameter => parameter.ParameterType).ToArray();
			int paramsLength = parameterTypes.Length;
			bool isAction = methodInfo.ReturnType == typeof(void);
			Type methodType = isAction
				? paramsLength == 0 ? GenericActionTypes[paramsLength] : GenericActionTypes[paramsLength].MakeGenericType(parameterTypes)
				: GenericFuncTypes[paramsLength].MakeGenericType(parameterTypes.Append(methodInfo.ReturnType).ToArray());
			wrappedDelegate = Delegate.CreateDelegate(methodType, source, methodInfo);
			Type memberType = isAction ? NonSpecificActionTypes[paramsLength] : NonSpecificFuncTypes[paramsLength];
			MethodInfo method = isAction ? ActionMemberMethods[paramsLength] : FuncMemberMethods[paramsLength];
			this.mapper = mapper;
			Delegate = Delegate.CreateDelegate(memberType, this, method);
		}

		#region Generated Content

		private static readonly Type[] GenericActionTypes = {  typeof(Action),  typeof(Action<>),  typeof(Action<,>),  typeof(Action<,,>),  typeof(Action<,,,>),  typeof(Action<,,,,>),  typeof(Action<,,,,,>),  typeof(Action<,,,,,,>),  typeof(Action<,,,,,,,>),  typeof(Action<,,,,,,,,>),  typeof(Action<,,,,,,,,,>),  typeof(Action<,,,,,,,,,,>),  typeof(Action<,,,,,,,,,,,>),  typeof(Action<,,,,,,,,,,,,>),  typeof(Action<,,,,,,,,,,,,,>),  typeof(Action<,,,,,,,,,,,,,,>),  typeof(Action<,,,,,,,,,,,,,,,>) };

		private static readonly Type[] GenericFuncTypes = { typeof(Func<>), typeof(Func<,>), typeof(Func<,,>), typeof(Func<,,,>), typeof(Func<,,,,>), typeof(Func<,,,,,>), typeof(Func<,,,,,,>), typeof(Func<,,,,,,,>), typeof(Func<,,,,,,,,>), typeof(Func<,,,,,,,,,>), typeof(Func<,,,,,,,,,,>), typeof(Func<,,,,,,,,,,,>), typeof(Func<,,,,,,,,,,,,>), typeof(Func<,,,,,,,,,,,,,>), typeof(Func<,,,,,,,,,,,,,,>), typeof(Func<,,,,,,,,,,,,,,,>), typeof(Func<,,,,,,,,,,,,,,,,>) };

		private static readonly Type[] NonSpecificActionTypes = {  typeof(Action),  typeof(Action<object>),  typeof(Action<object,object>),  typeof(Action<object,object,object>),  typeof(Action<object,object,object,object>),  typeof(Action<object,object,object,object,object>),  typeof(Action<object,object,object,object,object,object>),  typeof(Action<object,object,object,object,object,object,object>),  typeof(Action<object,object,object,object,object,object,object,object>),  typeof(Action<object,object,object,object,object,object,object,object,object>),  typeof(Action<object,object,object,object,object,object,object,object,object,object>),  typeof(Action<object,object,object,object,object,object,object,object,object,object,object>),  typeof(Action<object,object,object,object,object,object,object,object,object,object,object,object>),  typeof(Action<object,object,object,object,object,object,object,object,object,object,object,object,object>),  typeof(Action<object,object,object,object,object,object,object,object,object,object,object,object,object,object>),  typeof(Action<object,object,object,object,object,object,object,object,object,object,object,object,object,object,object>),  typeof(Action<object,object,object,object,object,object,object,object,object,object,object,object,object,object,object,object>) };

		private static readonly Type[] NonSpecificFuncTypes = { typeof(Func<object>), typeof(Func<object,object>), typeof(Func<object,object,object>), typeof(Func<object,object,object,object>), typeof(Func<object,object,object,object,object>), typeof(Func<object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object,object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object,object,object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object,object,object,object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object,object,object,object,object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object,object,object,object,object,object,object,object,object,object,object>) };

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

		#endregion Generated Content

	}
}