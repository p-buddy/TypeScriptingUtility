using System;
using System.Collections.Generic;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
  public static class Conversions
  {
    public static bool IsConvertibleToJS(Type type) => clrToJs.ContainsKey(type);
    public static bool IsConvertibleToCLR(Type type) => jsToCLR.ContainsKey(type);
    
    private static Dictionary<Type, Type> clrToJs;
    private static Dictionary<Type, Type[]> jsToCLR;

    static Conversions()
    {
      clrToJs = new Dictionary<Type, Type>
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
      foreach (KeyValuePair<Type, Type> kvp in clrToJs)
      {
        if (temp.TryGetValue(kvp.Value, out List<Type> types))
        {
          types.Add(kvp.Key);
          continue;
        }
        
        temp.Add(kvp.Value, new List<Type>{kvp.Key});
      }

      jsToCLR = new Dictionary<Type, Type[]>(temp.Count);
      foreach (KeyValuePair<Type, List<Type>> kvp in temp)
      {
        jsToCLR.Add(kvp.Key, kvp.Value.ToArray());
      }
    }
  }
}