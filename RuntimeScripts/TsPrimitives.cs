using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
  public static class TsPrimitives
  {
    public static bool IsTsPrimitive(this Type type) => ClrToJs.ContainsKey(type);
    public static bool TryGetTsName(Type type, out string name)
    {
      Type queryType = type.IsArray ? type.GetElementType() : type;
      Assert.IsNotNull(queryType);
      
      if (ClrToJs.TryGetValue(queryType, out Type jsType))
      {
        name = type.IsArray ? $"{NameByJsType[jsType]}[]" : NameByJsType[jsType];
        return true;
      }

      name = null;
      return false;
    }

    private static readonly Dictionary<Type, Type> ClrToJs;
    private static readonly Dictionary<Type, Type[]> JsToClr;
    private static readonly Dictionary<Type, string> NameByJsType;
    
    static TsPrimitives()
    {
      ClrToJs = new Dictionary<Type, Type>
      {
        { typeof(bool), typeof(bool) },
        { typeof(byte), typeof(double) },
        { typeof(char), typeof(double) },
        { typeof(Decimal), typeof(double) },
        { typeof(short), typeof(double) },
        { typeof(int), typeof(double) },
        { typeof(long), typeof(double) },
        { typeof(sbyte), typeof(double) },
        { typeof(float), typeof(double) },
        { typeof(uint), typeof(double) },
        { typeof(ushort), typeof(double) },
        { typeof(ulong), typeof(double) },
        { typeof(string), typeof(string) },
        { typeof(void), typeof(void) }
      };
      
      Dictionary<Type, List<Type>> temp = new Dictionary<Type, List<Type>>();
      foreach (KeyValuePair<Type, Type> kvp in ClrToJs)
      {
        if (temp.TryGetValue(kvp.Value, out List<Type> types))
        {
          types.Add(kvp.Key);
          continue;
        }
        
        temp.Add(kvp.Value, new List<Type>{kvp.Key});
      }

      JsToClr = new Dictionary<Type, Type[]>(temp.Count);
      foreach (KeyValuePair<Type, List<Type>> kvp in temp)
      {
        JsToClr.Add(kvp.Key, kvp.Value.ToArray());
      }

      NameByJsType = new Dictionary<Type, string>(JsToClr.Count)
      {
        [typeof(double)] = "number",
        [typeof(void)] = "void",
        [typeof(string)] = "string",
        [typeof(bool)] = "boolean"
      };

      foreach (Type jsType in JsToClr.Keys)
      {
        
      }
    }
  }
}