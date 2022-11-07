using System;
using System.Collections.Generic;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public readonly struct TypeReferenceMap
    {
        public IAPI API { get; }
        private readonly Dictionary<Type, TsReference> map;
        public int Count => map.Count;
        public Dictionary<Type, TsReference>.ValueCollection References => map.Values;
        
        public TypeReferenceMap(IAPI api)
        {
            map = new ();
            API = api;
            map.AddPrimitiveReferences();
            AddClassReferences();
        }

        public void Add(Type type, TsReference reference) => map[type] = reference;

        public bool Contains(Type type) => map.ContainsKey(type);
        
        public bool TryGetTsReference(Type type, out TsReference tsReference) => map.TryGetValue(type, out tsReference);

        public bool TryGetReferenceString(Type type, out string reference)
        {
            if (!TryGetTsReference(type, out TsReference tsReference))
            {
                reference = null;
                return false;
            };

            reference = tsReference.Reference;
            
            if (type.TryGetValuableInterface(out Type valuableInterface))
            {
                if (!TryGetReferenceString(valuableInterface.GenericTypeArguments[0], out string valueReference))
                {
                    return false;
                }
                
                reference = $"({reference} | {valueReference})";
            }
            
            return true;
        }
        
        public string GetReference(Type type)
        {
            if (type.IsGenericParameter) return type.Name;
            bool success = TryGetReferenceString(type, out string reference);
            return success ? reference : throw new Exception($"Could not retrieve reference to {type}");
        }
        
        private void AddClassReferences()
        {
            foreach ((Type classType, IShared shareClass) in API[TsType.Specification.Class])
            {
                map[classType] = TsReference.Class(shareClass.ClrType, shareClass.TsType.Name);
            }
        }
    }
}