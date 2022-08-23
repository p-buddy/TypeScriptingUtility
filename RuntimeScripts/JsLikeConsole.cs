using System.Linq;
using UnityEngine;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    public class JsLikeConsole
    {
        public void log(params object[] msgs) => msgs.ToList().ForEach(Debug.Log);
        public void error(params object[] msgs) => msgs.ToList().ForEach(Debug.LogError);
        public void warn(params object[] msgs) => msgs.ToList().ForEach(Debug.LogWarning);
    }
}