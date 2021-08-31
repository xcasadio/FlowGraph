using System.Collections.Generic;
using FlowGraphBase.Logger;

namespace FlowGraphBase.Script
{
    #region class GlobalVar

    /// <summary>
    /// 
    /// </summary>
    static public class GlobalVar
    {
        #region Fields

        static private Dictionary<string, object> m_Vars = new Dictionary<string, object>();

        #endregion // Fields

        #region Methods

        /// <summary>
        /// Set a value associate to a key
        /// </summary>
        /// <param name="key_"></param>
        /// <param name="val_"></param>
        static public void Set(string key_, object val_)
        {
            if (m_Vars.ContainsKey(key_))
            {
                m_Vars[key_] = val_;
            }
            else
            {
                m_Vars.Add(key_, val_);
            }
        }

        /// <summary>
        /// Obtain a value by the associate key
        /// </summary>
        /// <param name="key_"></param>
        /// <returns>the value by the associate key else returns null</returns>
        static public object Get(string key_)
        {
            if (m_Vars.ContainsKey(key_) == false)
            {
                LogManager.Instance.WriteLine(LogVerbosity.Error, "GlobalVar.Get() : can't find the field with the key '{0}'", key_);
                return null;
            }

            return m_Vars[key_];
        }

        /// <summary>
        /// Determines whether the GlobalVar contains the specified key.
        /// </summary>
        /// <param name="key_"></param>
        /// <returns>true if the key exist else returns false</returns>
        static public bool ContainsKey(string key_)
        {
            return m_Vars.ContainsKey(key_);
        }

        #endregion // Methods
    }

    #endregion // class GlobalVar

    #region class Sequence

    /// <summary>
    /// 
    /// </summary>
    static public class ScriptSequence
    {
        #region Fields

        static volatile private uint m_Sequence = 0;


        #endregion // Fields

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns>an unique uint</returns>
        static public uint GetNext()
        {
            return m_Sequence++;
        }

        #endregion // Methods
    }

    #endregion // class Sequence
}
