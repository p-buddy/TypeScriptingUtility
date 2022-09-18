using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine.Assertions;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TExecutionDomain"></typeparam>
    public abstract class APIBase<TExecutionDomain> : IAPI
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
        public IShared[] Shared => RetrieveTsRootTypes(Domain);
        protected abstract TExecutionDomain Define();

        private IShared[] RetrieveTsRootTypes(TExecutionDomain obj)
        {
            const BindingFlags flags = BindingFlags.Public |
                                       BindingFlags.NonPublic |
                                       BindingFlags.Instance |
                                       BindingFlags.DeclaredOnly;
            
            static bool IsCorrectMemberType(MemberInfo info)
            {
                return info.MemberType switch
                {
                    MemberTypes.Field => true,
                    MemberTypes.Property => true,
                    _ => false
                };
            }

            static bool Is<T>(MemberInfo info) => typeof(T).IsAssignableFrom(new DataMember(info).Type);
            
            T As<T>(MemberInfo info)
            {
                return info.MemberType switch
                {
                    MemberTypes.Field => (T)(info as FieldInfo)?.GetValue(obj),
                    MemberTypes.Property => (T)(info as PropertyInfo)?.GetValue(obj),
                    _ => throw new Exception()
                };
            }

            return obj.GetType()
                      .GetMembers(flags)
                      .Where(IsCorrectMemberType)
                      .Where(Is<IShared>)
                      .Select(As<IShared>)
                      .ToArray()
                      .ValidateSharedValues();
        }
    }
    
    
}