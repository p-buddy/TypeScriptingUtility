using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public abstract class API<TExecutionDomain> : IAPI
    {
        private bool defined;
        private TExecutionDomain domain;

        public TExecutionDomain Domain
        {
            get
            {
                domain = defined ? domain : Define();
                defined = true;
                return domain;
            }
        }

        public virtual IClrToTsNameMapper NameMapper => ClrToTsNameMapper.Default;
        public IShared[] Links => RetrieveTsRootTypes(Domain);
        protected abstract TExecutionDomain Define();

        private IShared[] RetrieveTsRootTypes(TExecutionDomain obj)
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
                      .ToArray()
                      .ValidateSharedValues();

            #region Local Functions
            static bool IsFieldOrProperty(MemberInfo info) =>
                info.MemberType == MemberTypes.Field || info.MemberType == MemberTypes.Property;

            static bool IsLink(MemberInfo info) =>
                typeof(IShared).IsAssignableFrom(info.MemberType == MemberTypes.Field
                                                   ? (info as FieldInfo)?.FieldType
                                                   : (info as PropertyInfo)?.PropertyType);

            IShared AsLink(MemberInfo info)
            {
                return info.MemberType switch
                {
                    MemberTypes.Field => (info as FieldInfo)?.GetValue(obj) as IShared,
                    MemberTypes.Property => (info as PropertyInfo)?.GetValue(obj) as IShared,
                    _ => throw new Exception()
                };
            }
            #endregion
        }
    }
    
    
}