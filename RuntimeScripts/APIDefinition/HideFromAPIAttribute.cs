using System.Reflection;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public class HideFromAPIAttribute: System.Attribute
    {
        public static bool IsHidden(MemberInfo info) => info.GetCustomAttributes(typeof(HideFromAPIAttribute), false).Length > 0;
        public static bool IsNotHidden(MemberInfo info) => !IsHidden(info);
    }
}