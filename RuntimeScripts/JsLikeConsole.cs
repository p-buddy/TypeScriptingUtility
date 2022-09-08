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
        // ReSharper disable once InconsistentNaming
        public void log(params object[] messages) => messages.ToList().ForEach(Debug.Log);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messages"></param>
        // ReSharper disable once InconsistentNaming
        public void error(params object[] messages) => messages.ToList().ForEach(Debug.LogError);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messages"></param>
        // ReSharper disable once InconsistentNaming
        public void warn(params object[] messages) => messages.ToList().ForEach(Debug.LogWarning);
    }
}