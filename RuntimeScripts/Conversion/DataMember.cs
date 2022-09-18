using System;
using System.Reflection;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public readonly struct DataMember
    {
        public Type Type { get; }
        public string Name => member.Name;
            
        private readonly MemberInfo member;
        private readonly Action<object, object> internalSetValue;
            
        public DataMember(PropertyInfo propertyInfo)
        {
            Type = propertyInfo.PropertyType;
            internalSetValue = propertyInfo.SetValue;
            member = propertyInfo;
        }
            
        public DataMember(FieldInfo fieldInfo)
        {
            Type = fieldInfo.FieldType;
            internalSetValue = fieldInfo.SetValue;
            member = fieldInfo;
        }
        
        public DataMember(MemberInfo memberInfo)
        {
            switch (memberInfo)
            {
                case FieldInfo fieldInfo:
                    Type = fieldInfo.FieldType;
                    internalSetValue = fieldInfo.SetValue;
                    break;
                case PropertyInfo propertyInfo:
                    Type = propertyInfo.PropertyType;
                    internalSetValue = propertyInfo.SetValue;
                    break;
                default:
                    throw new Exception("Must be field or property");
            }
            member = memberInfo;
        }

        public void SetValue(object obj, object value) => internalSetValue.Invoke(obj, value);
    }
}