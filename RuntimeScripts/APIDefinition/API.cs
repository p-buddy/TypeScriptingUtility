using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public abstract class API<T> : IAPI
    {
        private bool defined;
        private T globals;

        public T Domain
        {
            get
            {
                globals = defined ? globals : Define();
                defined = true;
                return globals;
            }
        }

        public ILink[] Links => RetrieveTsRootTypes(Domain);

        protected abstract T Define();

        private ILink[] RetrieveTsRootTypes(T obj)
        {
            const BindingFlags flags = BindingFlags.Public |
                                       BindingFlags.NonPublic |
                                       BindingFlags.Instance |
                                       BindingFlags.DeclaredOnly;

            return obj.GetType()
                      .GetMembers(flags)
                      .Where(IsFieldOrProperty)
                      .Where(IsLink)
                      .Select(AsLink)
                      .ToArray();
            
            #region Local Functions
            static bool IsFieldOrProperty(MemberInfo info) =>
                info.MemberType == MemberTypes.Field || info.MemberType == MemberTypes.Property;

            static bool IsLink(MemberInfo info) =>
                typeof(ILink).IsAssignableFrom(info.MemberType == MemberTypes.Field
                                                   ? (info as FieldInfo)?.FieldType
                                                   : (info as PropertyInfo)?.PropertyType);

            ILink AsLink(MemberInfo info)
            {
                switch (info.MemberType)
                {
                    case MemberTypes.Field:
                        return (info as FieldInfo)?.GetValue(obj) as ILink;
                    case MemberTypes.Property:
                        return (info as PropertyInfo)?.GetValue(obj) as ILink;
                }

                throw new Exception();
            }
            #endregion
        }
    }
    
    
}