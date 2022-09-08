using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public abstract class API<TExecutionDomain> : IAPI
    {
        private bool defined;
        private TExecutionDomain globals;

        public TExecutionDomain Domain
        {
            get
            {
                globals = defined ? globals : Define();
                defined = true;
                return globals;
            }
        }

        public virtual IClrToTsNameMapper NameMapper => ClrToTsNameMapper.Default;
        public ILink[] Links => RetrieveTsRootTypes(Domain);
        protected abstract TExecutionDomain Define();

        private ILink[] RetrieveTsRootTypes(TExecutionDomain obj)
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
                return info.MemberType switch
                {
                    MemberTypes.Field => (info as FieldInfo)?.GetValue(obj) as ILink,
                    MemberTypes.Property => (info as PropertyInfo)?.GetValue(obj) as ILink,
                    _ => throw new Exception()
                };
            }
            #endregion
        }
    }
    
    
}