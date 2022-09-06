using System;
using System.Collections.Generic;
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
        private string Name => field?.Name ?? property.Name;
        public object CurrentValue =>
            field?.GetValue(parent) ??
            (property.GetIndexParameters().Any()
                ? null // TODO: Handle indexers...somehow? with numbered keys? Numbered keys work, but obviously inefficient
                : property.GetValue(parent));

        private FieldAndPropertyWrapper(object parent, ExpandoObject expando, FieldInfo field, PropertyInfo property)
        {
            this.parent = parent;
            this.property = property;
            this.field = field;
            type = this.field?.FieldType ?? this.property.PropertyType;
            expando.Add(Name, CurrentValue);
            ((INotifyPropertyChanged)expando).PropertyChanged += OnChange;
        }

        public FieldAndPropertyWrapper(object parent, FieldInfo field, ExpandoObject expando) :
            this(parent, expando, field, null) { }
        
        public FieldAndPropertyWrapper(object parent, PropertyInfo property, ExpandoObject expando) :
            this(parent, expando, null, property) { }

        private void OnChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != Name) return;
            if (!(field is null)) return; // TODO: understand why fields update correctly (maybe the current value object is pointing to the correct spot?)
            var value = (sender as IDictionary<string, object>)?[Name];
            property.SetValue(parent, value.As(type));
        }
    }
}