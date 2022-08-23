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

        public MethodWrapper(object source, MethodInfo methodInfo) : this()
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
			Delegate = Delegate.CreateDelegate(memberType, this, method);
		}
        
		#region Generated Content

		private static readonly Type[] GenericActionTypes = {  typeof(Action),  typeof(Action<>),  typeof(Action<,>),  typeof(Action<,,>),  typeof(Action<,,,>),  typeof(Action<,,,,>),  typeof(Action<,,,,,>),  typeof(Action<,,,,,,>),  typeof(Action<,,,,,,,>),  typeof(Action<,,,,,,,,>),  typeof(Action<,,,,,,,,,>),  typeof(Action<,,,,,,,,,,>),  typeof(Action<,,,,,,,,,,,>),  typeof(Action<,,,,,,,,,,,,>),  typeof(Action<,,,,,,,,,,,,,>),  typeof(Action<,,,,,,,,,,,,,,>),  typeof(Action<,,,,,,,,,,,,,,,>) };

		private static readonly Type[] GenericFuncTypes = { typeof(Func<>), typeof(Func<,>), typeof(Func<,,>), typeof(Func<,,,>), typeof(Func<,,,,>), typeof(Func<,,,,,>), typeof(Func<,,,,,,>), typeof(Func<,,,,,,,>), typeof(Func<,,,,,,,,>), typeof(Func<,,,,,,,,,>), typeof(Func<,,,,,,,,,,>), typeof(Func<,,,,,,,,,,,>), typeof(Func<,,,,,,,,,,,,>), typeof(Func<,,,,,,,,,,,,,>), typeof(Func<,,,,,,,,,,,,,,>), typeof(Func<,,,,,,,,,,,,,,,>), typeof(Func<,,,,,,,,,,,,,,,,>) };

		private static readonly Type[] NonSpecificActionTypes = {  typeof(Action),  typeof(Action<object>),  typeof(Action<object,object>),  typeof(Action<object,object,object>),  typeof(Action<object,object,object,object>),  typeof(Action<object,object,object,object,object>),  typeof(Action<object,object,object,object,object,object>),  typeof(Action<object,object,object,object,object,object,object>),  typeof(Action<object,object,object,object,object,object,object,object>),  typeof(Action<object,object,object,object,object,object,object,object,object>),  typeof(Action<object,object,object,object,object,object,object,object,object,object>),  typeof(Action<object,object,object,object,object,object,object,object,object,object,object>),  typeof(Action<object,object,object,object,object,object,object,object,object,object,object,object>),  typeof(Action<object,object,object,object,object,object,object,object,object,object,object,object,object>),  typeof(Action<object,object,object,object,object,object,object,object,object,object,object,object,object,object>),  typeof(Action<object,object,object,object,object,object,object,object,object,object,object,object,object,object,object>),  typeof(Action<object,object,object,object,object,object,object,object,object,object,object,object,object,object,object,object>) };

		private static readonly Type[] NonSpecificFuncTypes = { typeof(Func<object>), typeof(Func<object,object>), typeof(Func<object,object,object>), typeof(Func<object,object,object,object>), typeof(Func<object,object,object,object,object>), typeof(Func<object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object,object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object,object,object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object,object,object,object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object,object,object,object,object,object,object,object,object,object>), typeof(Func<object,object,object,object,object,object,object,object,object,object,object,object,object,object,object,object,object>) };

		#region Actions

		public void ActionCastAndInvoke() => wrappedDelegate.DynamicInvoke();
		public void ActionCastAndInvoke(object a) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]));
		public void ActionCastAndInvoke(object a, object b) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]));
		public void ActionCastAndInvoke(object a, object b, object c) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]), c.As(parameterTypes[2]));
		public void ActionCastAndInvoke(object a, object b, object c, object d) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]), c.As(parameterTypes[2]), d.As(parameterTypes[3]));
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]), c.As(parameterTypes[2]), d.As(parameterTypes[3]), e.As(parameterTypes[4]));
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]), c.As(parameterTypes[2]), d.As(parameterTypes[3]), e.As(parameterTypes[4]), f.As(parameterTypes[5]));
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]), c.As(parameterTypes[2]), d.As(parameterTypes[3]), e.As(parameterTypes[4]), f.As(parameterTypes[5]), g.As(parameterTypes[6]));
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]), c.As(parameterTypes[2]), d.As(parameterTypes[3]), e.As(parameterTypes[4]), f.As(parameterTypes[5]), g.As(parameterTypes[6]), h.As(parameterTypes[7]));
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]), c.As(parameterTypes[2]), d.As(parameterTypes[3]), e.As(parameterTypes[4]), f.As(parameterTypes[5]), g.As(parameterTypes[6]), h.As(parameterTypes[7]), i.As(parameterTypes[8]));
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]), c.As(parameterTypes[2]), d.As(parameterTypes[3]), e.As(parameterTypes[4]), f.As(parameterTypes[5]), g.As(parameterTypes[6]), h.As(parameterTypes[7]), i.As(parameterTypes[8]), j.As(parameterTypes[9]));
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]), c.As(parameterTypes[2]), d.As(parameterTypes[3]), e.As(parameterTypes[4]), f.As(parameterTypes[5]), g.As(parameterTypes[6]), h.As(parameterTypes[7]), i.As(parameterTypes[8]), j.As(parameterTypes[9]), k.As(parameterTypes[10]));
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]), c.As(parameterTypes[2]), d.As(parameterTypes[3]), e.As(parameterTypes[4]), f.As(parameterTypes[5]), g.As(parameterTypes[6]), h.As(parameterTypes[7]), i.As(parameterTypes[8]), j.As(parameterTypes[9]), k.As(parameterTypes[10]), l.As(parameterTypes[11]));
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]), c.As(parameterTypes[2]), d.As(parameterTypes[3]), e.As(parameterTypes[4]), f.As(parameterTypes[5]), g.As(parameterTypes[6]), h.As(parameterTypes[7]), i.As(parameterTypes[8]), j.As(parameterTypes[9]), k.As(parameterTypes[10]), l.As(parameterTypes[11]), m.As(parameterTypes[12]));
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]), c.As(parameterTypes[2]), d.As(parameterTypes[3]), e.As(parameterTypes[4]), f.As(parameterTypes[5]), g.As(parameterTypes[6]), h.As(parameterTypes[7]), i.As(parameterTypes[8]), j.As(parameterTypes[9]), k.As(parameterTypes[10]), l.As(parameterTypes[11]), m.As(parameterTypes[12]), n.As(parameterTypes[13]));
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n, object o) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]), c.As(parameterTypes[2]), d.As(parameterTypes[3]), e.As(parameterTypes[4]), f.As(parameterTypes[5]), g.As(parameterTypes[6]), h.As(parameterTypes[7]), i.As(parameterTypes[8]), j.As(parameterTypes[9]), k.As(parameterTypes[10]), l.As(parameterTypes[11]), m.As(parameterTypes[12]), n.As(parameterTypes[13]), o.As(parameterTypes[14]));
		public void ActionCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n, object o, object p) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]), c.As(parameterTypes[2]), d.As(parameterTypes[3]), e.As(parameterTypes[4]), f.As(parameterTypes[5]), g.As(parameterTypes[6]), h.As(parameterTypes[7]), i.As(parameterTypes[8]), j.As(parameterTypes[9]), k.As(parameterTypes[10]), l.As(parameterTypes[11]), m.As(parameterTypes[12]), n.As(parameterTypes[13]), o.As(parameterTypes[14]), p.As(parameterTypes[15]));

		#endregion Actions

		#region Funcs

		public object FuncCastAndInvoke() => wrappedDelegate.DynamicInvoke();
		public object FuncCastAndInvoke(object a) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]));
		public object FuncCastAndInvoke(object a, object b) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]));
		public object FuncCastAndInvoke(object a, object b, object c) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]), c.As(parameterTypes[2]));
		public object FuncCastAndInvoke(object a, object b, object c, object d) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]), c.As(parameterTypes[2]), d.As(parameterTypes[3]));
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]), c.As(parameterTypes[2]), d.As(parameterTypes[3]), e.As(parameterTypes[4]));
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]), c.As(parameterTypes[2]), d.As(parameterTypes[3]), e.As(parameterTypes[4]), f.As(parameterTypes[5]));
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]), c.As(parameterTypes[2]), d.As(parameterTypes[3]), e.As(parameterTypes[4]), f.As(parameterTypes[5]), g.As(parameterTypes[6]));
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]), c.As(parameterTypes[2]), d.As(parameterTypes[3]), e.As(parameterTypes[4]), f.As(parameterTypes[5]), g.As(parameterTypes[6]), h.As(parameterTypes[7]));
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]), c.As(parameterTypes[2]), d.As(parameterTypes[3]), e.As(parameterTypes[4]), f.As(parameterTypes[5]), g.As(parameterTypes[6]), h.As(parameterTypes[7]), i.As(parameterTypes[8]));
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]), c.As(parameterTypes[2]), d.As(parameterTypes[3]), e.As(parameterTypes[4]), f.As(parameterTypes[5]), g.As(parameterTypes[6]), h.As(parameterTypes[7]), i.As(parameterTypes[8]), j.As(parameterTypes[9]));
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]), c.As(parameterTypes[2]), d.As(parameterTypes[3]), e.As(parameterTypes[4]), f.As(parameterTypes[5]), g.As(parameterTypes[6]), h.As(parameterTypes[7]), i.As(parameterTypes[8]), j.As(parameterTypes[9]), k.As(parameterTypes[10]));
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]), c.As(parameterTypes[2]), d.As(parameterTypes[3]), e.As(parameterTypes[4]), f.As(parameterTypes[5]), g.As(parameterTypes[6]), h.As(parameterTypes[7]), i.As(parameterTypes[8]), j.As(parameterTypes[9]), k.As(parameterTypes[10]), l.As(parameterTypes[11]));
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]), c.As(parameterTypes[2]), d.As(parameterTypes[3]), e.As(parameterTypes[4]), f.As(parameterTypes[5]), g.As(parameterTypes[6]), h.As(parameterTypes[7]), i.As(parameterTypes[8]), j.As(parameterTypes[9]), k.As(parameterTypes[10]), l.As(parameterTypes[11]), m.As(parameterTypes[12]));
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]), c.As(parameterTypes[2]), d.As(parameterTypes[3]), e.As(parameterTypes[4]), f.As(parameterTypes[5]), g.As(parameterTypes[6]), h.As(parameterTypes[7]), i.As(parameterTypes[8]), j.As(parameterTypes[9]), k.As(parameterTypes[10]), l.As(parameterTypes[11]), m.As(parameterTypes[12]), n.As(parameterTypes[13]));
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n, object o) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]), c.As(parameterTypes[2]), d.As(parameterTypes[3]), e.As(parameterTypes[4]), f.As(parameterTypes[5]), g.As(parameterTypes[6]), h.As(parameterTypes[7]), i.As(parameterTypes[8]), j.As(parameterTypes[9]), k.As(parameterTypes[10]), l.As(parameterTypes[11]), m.As(parameterTypes[12]), n.As(parameterTypes[13]), o.As(parameterTypes[14]));
		public object FuncCastAndInvoke(object a, object b, object c, object d, object e, object f, object g, object h, object i, object j, object k, object l, object m, object n, object o, object p) => wrappedDelegate.DynamicInvoke(a.As(parameterTypes[0]), b.As(parameterTypes[1]), c.As(parameterTypes[2]), d.As(parameterTypes[3]), e.As(parameterTypes[4]), f.As(parameterTypes[5]), g.As(parameterTypes[6]), h.As(parameterTypes[7]), i.As(parameterTypes[8]), j.As(parameterTypes[9]), k.As(parameterTypes[10]), l.As(parameterTypes[11]), m.As(parameterTypes[12]), n.As(parameterTypes[13]), o.As(parameterTypes[14]), p.As(parameterTypes[15]));

		#endregion Funcs

		#endregion Generated Content

	}
}