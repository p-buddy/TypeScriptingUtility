using System;
using System.Collections.Generic;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public static class APIValidation
    {
        public static IShared[] ValidateSharedValues(this IShared[] allShared)
        {
            HashSet<string> names = new HashSet<string>(allShared.Length);
            
            foreach (IShared shared in allShared)
            {
                string name = shared.TsType.Name;
                if (names.Contains(name)) throw new Exception($"API has more than one shared value with the name '{name}'");
                names.Add(name);
            }

            return allShared;
        }
    }
}