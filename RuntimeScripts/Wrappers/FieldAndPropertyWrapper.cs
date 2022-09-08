using System;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    internal readonly struct FieldAndPropertyWrapper
    {
        private readonly object parent;
        private readonly FieldInfo field;
        private readonly PropertyInfo property;
        private readonly Type type;
        private readonly string name;
        private readonly IClrToTsNameMapper mapper;
        private object CurrentValue =>
            field?.GetValue(parent) ??
            (property.GetIndexParameters().Any()
                ? null // TODO: Handle indexers...somehow? with numbered keys? Numbered keys work, but obviously inefficient
                : property.GetValue(parent));

        private FieldAndPropertyWrapper(object parent,
                                        ExpandoObject expando,
                                        FieldInfo field,
                                        PropertyInfo property,
                                        IClrToTsNameMapper mapper)
        {
            this.parent = parent;
            this.property = property;
            this.field = field;
            this.mapper = mapper;
            name = this.mapper.MapToTs(this.field?.Name ?? this.property.Name);
            type = this.field?.FieldType ?? this.property.PropertyType;
            expando.Add(name, CurrentValue);
            ((INotifyPropertyChanged)expando).PropertyChanged += OnChange;
        }

        public FieldAndPropertyWrapper(object parent, FieldInfo field, ExpandoObject expando, IClrToTsNameMapper mapper) :
            this(parent, expando, field, null, mapper) { }
        
        public FieldAndPropertyWrapper(object parent, PropertyInfo property, ExpandoObject expando, IClrToTsNameMapper mapper) :
            this(parent, expando, null, property, mapper) { }

        private void OnChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != name) return;
            object value = (sender as ExpandoObject)?.Get(name);
            field?.SetValue(parent, value.As(type, mapper));
            property?.SetValue(parent, value.As(type, mapper));
        }
    }
}