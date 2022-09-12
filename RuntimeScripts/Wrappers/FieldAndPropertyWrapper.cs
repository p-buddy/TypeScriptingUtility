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
        private readonly bool isIndexer;

        private object CurrentValue =>
            field?.GetValue(parent) ??
            (!isIndexer
                ? property.GetValue(parent)
                : throw new Exception($"{name}, an index-able property, can not be accessed directly. Use brackets ('[]') instead."));

        public object this[int i]
        {
            get => property.GetValue(parent, new object[] { i });
            set { property.SetValue(parent, value, new object[] { i }); }
        }

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
            type = this.field?.FieldType ?? this.property.PropertyType;
            isIndexer = this.property?.GetIndexParameters().Any() ?? false;
            if (!isIndexer)
            {
                name = this.mapper.MapToTs(this.field?.Name ?? this.property.Name);
                expando.Add(name, CurrentValue);
                ((INotifyPropertyChanged)expando).PropertyChanged += OnChange;
                return;
            }

            name = "indexer";
            // Overall, this code is bananas!! Hopefully not too many indexable types will need to be passed around.
            // NOTE: A limitation of this code is that, if what can be indexed changes (i.e. the length of a list increases),
            // then they won't be able to be indexed.
            for (int i = 0; i < Int32.MaxValue; i++)
            {
                try { expando.Add($"{i}", property.GetValue(parent, new object[] { i })); } 
                catch { break; }
            }
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