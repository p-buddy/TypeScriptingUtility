using System.Linq;
using UnityEngine;

namespace pbuddy.TypeScriptingUtility.RuntimeScripts
{
    /// <summary>
    /// 
    /// </summary>
    public readonly struct JsLikeConsole
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messages"></param>
        public void Log(params object[] messages) => messages.ToList().ForEach(Debug.Log);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messages"></param>
        public void Error(params object[] messages) => messages.ToList().ForEach(Debug.LogError);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messages"></param>
        public void Warn(params object[] messages) => messages.ToList().ForEach(Debug.LogWarning);
    }
}