using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public readonly struct DataMember
    {
        public Type Type { get; }
        public ParameterInfo[] IndexParams { get; }

        public string Name => member.Name;
            
        private readonly MemberInfo member;
        private readonly Action<object, object> internalSetValue;
            
        public DataMember(PropertyInfo propertyInfo) : this(propertyInfo as MemberInfo)
        {
            
        }
            
        public DataMember(FieldInfo fieldInfo) : this(fieldInfo as MemberInfo)
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
                    IndexParams = null;
                    break;
                case PropertyInfo propertyInfo:
                    Type = propertyInfo.PropertyType;
                    internalSetValue = GetPropertySetter(propertyInfo);
                    IndexParams = propertyInfo.GetIndexParameters();
                    IndexParams = IndexParams.Length > 0 ? IndexParams : null;
                    break;
                default:
                    throw new Exception("Must be field or property");
            }
            member = memberInfo;
        }

        public void SetValue(object obj, object value) => internalSetValue.Invoke(obj, value);

        private static Action<object, object> GetPropertySetter(PropertyInfo propertyInfo) =>
            propertyInfo.CanWrite
                ? propertyInfo.SetValue
                : SketchilyGetBackingField(propertyInfo);
        
        private static Action<object, object> SketchilyGetBackingField(PropertyInfo propertyInfo)
        {
            var backingField = propertyInfo.DeclaringType
                                           .GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                                           .FirstOrDefault(field =>
                                                               field.Attributes.HasFlag(FieldAttributes.Private) &&
                                                               field.Attributes.HasFlag(FieldAttributes.InitOnly) &&
                                                               field.CustomAttributes.Any(attr => attr.AttributeType == typeof(CompilerGeneratedAttribute)) &&
                                                               (field.DeclaringType == propertyInfo.DeclaringType) &&
                                                               field.FieldType.IsAssignableFrom(propertyInfo.PropertyType) &&
                                                               field.Name.StartsWith("<" + propertyInfo.Name + ">")
                                                          );
            return backingField is null ? null : backingField.SetValue;
        }
    }
}