using System.Collections.Generic;
using FlowGraphBase.Logger;

namespace FlowGraphBase.Script
{
    #region class GlobalVar

    /// <summary>
    /// 
    /// </summary>
    public static class GlobalVar
    {
        #region Fields

        private static readonly Dictionary<string, object> _Vars = new Dictionary<string, object>();

        #endregion // Fields

        #region Methods

        /// <summary>
        /// Set a value associate to a key
        /// </summary>
        /// <param name="key_"></param>
        /// <param name="val_"></param>
        public static void Set(string key_, object val_)
        {
            if (_Vars.ContainsKey(key_))
            {
                _Vars[key_] = val_;
            }
            else
            {
                _Vars.Add(key_, val_);
            }
        }

        /// <summary>
        /// Obtain a value by the associate key
        /// </summary>
        /// <param name="key_"></param>
        /// <returns>the value by the associate key else returns null</returns>
        public static object Get(string key_)
        {
            if (_Vars.ContainsKey(key_) == false)
            {
                LogManager.Instance.WriteLine(LogVerbosity.Error, "GlobalVar.Get() : can't find the field with the key '{0}'", key_);
                return null;
            }

            return _Vars[key_];
        }

        /// <summary>
        /// Determines whether the GlobalVar contains the specified key.
        /// </summary>
        /// <param name="key_"></param>
        /// <returns>true if the key exist else returns false</returns>
        public static bool ContainsKey(string key_)
        {
            return _Vars.ContainsKey(key_);
        }

        #endregion // Methods
    }

    #endregion // class GlobalVar

    #region class Sequence

    /// <summary>
    /// 
    /// </summary>
    public static class ScriptSequence
    {
        #region Fields

        private static volatile uint _Sequence;


        #endregion // Fields

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns>an unique uint</returns>
        public static uint GetNext()
        {
            return _Sequence++;
        }

        #endregion // Methods
    }

    #endregion // class Sequence
}
