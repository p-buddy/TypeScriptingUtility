using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine.Assertions;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TExecutionDomain"></typeparam>
    public abstract class APIBase<TExecutionDomain> : IAPI, IDisposable
    {
        private const BindingFlags Flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy |
                                           BindingFlags.NonPublic;

        private bool defined;
        private Delegate wrapDelegate;
        private MethodInfo genericConvertMethod;
        private MethodInfo wrapMethod;

        private TExecutionDomain domain;
        private IShared[] shared;
        private Dictionary<TsType.Specification, Dictionary<Type, IShared>> typesBySpecification;
        
        private readonly Dictionary<Type, Type> convertToTypeByType = new();
        private readonly Dictionary<Type, Delegate> convertToDelegateByType = new();

        public MethodInfo GenericConvertMethod
        {
            get
            {
                genericConvertMethod ??= typeof(APIBase<TExecutionDomain>).GetMethod(nameof(ConvertTo), Flags);
                Assert.IsNotNull(genericConvertMethod);
                return genericConvertMethod;
            }
        }
        
        public MethodInfo WrapMethod
        {
            get
            {
                wrapMethod ??= typeof(APIBase<TExecutionDomain>).GetMethod(nameof(Wrap), Flags);
                Assert.IsNotNull(wrapMethod);
                return wrapMethod;
            }
        }

        public Delegate WrapDelegate
        {
            get
            {
                wrapDelegate ??= Delegate.CreateDelegate(typeof(Func<object, object>), this, WrapMethod);
                return wrapDelegate;
            }
        }

        public TExecutionDomain Domain
        {
            get
            {
                domain = defined ? domain : Define();
                defined = true;
                return domain;
            }
        }
        
        public IShared[] Shared
        {
            get
            {
                shared ??= RetrieveTsRootTypes(Domain);
                return shared;
            }
        }

        public Dictionary<Type, IShared> this[TsType.Specification spec]
        {
            get
            {
                typesBySpecification ??= OrganizeShared(Shared);
                return typesBySpecification[spec];
            }
        }

        public virtual IClrToTsNameMapper NameMapper => ClrToTsNameMapper.Default;
        
        protected abstract TExecutionDomain Define();

        public object Wrap(object obj)
        {
            return obj.Wrap(NameMapper);
        }
        
        public Delegate MakeConvertToMethod(Type type)
        {
            if (convertToDelegateByType.TryGetValue(type, out Delegate del))
            {
                return del;
            }

            if (!convertToTypeByType.TryGetValue(type, out Type delegateType))
            {
                delegateType = typeof(Func<,>).MakeGenericType(typeof(object), type);
                convertToTypeByType[type] = delegateType;
            }
            
            MethodInfo convertToTypeMethod = GenericConvertMethod.MakeGenericMethod(type);
            del = Delegate.CreateDelegate(delegateType, this, convertToTypeMethod);
            convertToDelegateByType[type] = del;
            return del;
        }
        
        private T ConvertTo<T>(object obj)
        {
            return (T)obj.As(typeof(T), NameMapper);
        }

        private static IShared[] RetrieveTsRootTypes(TExecutionDomain obj)
        {
            // Add caching
            
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

        private static Dictionary<TsType.Specification, Dictionary<Type, IShared>> OrganizeShared(IShared[] shared)
        {
            Dictionary<TsType.Specification, Dictionary<Type, IShared>> typesBySpec = new ()
                {
                    {TsType.Specification.Class, new Dictionary<Type, IShared>()},
                    {TsType.Specification.Variable, new Dictionary<Type, IShared>()},
                    {TsType.Specification.Function, new Dictionary<Type, IShared>()}
                };

            foreach (IShared share in shared)
            {
                typesBySpec[share.TsType.Spec][share.ClrType] = share;
            }

            return typesBySpec;
        }

        public void Dispose()
        {
            domain = default;
            defined = false;
            shared = null;
            typesBySpecification = null;
        }
    }
    
    
}