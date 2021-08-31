using System.Collections.Generic;
using FlowGraphBase.Logger;

namespace FlowGraphBase.Script
{
    /// <summary>
    /// 
    /// </summary>
    public static class GlobalVar
    {
        private static readonly Dictionary<string, object> Vars = new Dictionary<string, object>();

        /// <summary>
        /// Set a value associate to a key
        /// </summary>
        /// <param name="key_"></param>
        /// <param name="val_"></param>
        public static void Set(string key, object val)
        {
            if (Vars.ContainsKey(key))
            {
                Vars[key] = val;
            }
            else
            {
                Vars.Add(key, val);
            }
        }

        /// <summary>
        /// Obtain a value by the associate key
        /// </summary>
        /// <param name="key_"></param>
        /// <returns>the value by the associate key else returns null</returns>
        public static object Get(string key)
        {
            if (Vars.ContainsKey(key) == false)
            {
                LogManager.Instance.WriteLine(LogVerbosity.Error, "GlobalVar.Get() : can't find the field with the key '{0}'", key);
                return null;
            }

            return Vars[key];
        }

        /// <summary>
        /// Determines whether the GlobalVar contains the specified key.
        /// </summary>
        /// <param name="key_"></param>
        /// <returns>true if the key exist else returns false</returns>
        public static bool ContainsKey(string key)
        {
            return Vars.ContainsKey(key);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ScriptSequence
    {
        private static volatile uint _sequence;


        /// <summary>
        /// 
        /// </summary>
        /// <returns>an unique uint</returns>
        public static uint GetNext()
        {
            return _sequence++;
        }
    }
}
