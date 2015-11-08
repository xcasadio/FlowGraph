using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlowGraphBase.Logger
{
	/// <summary>
	/// Interface for all Logger
	/// </summary>
	public interface ILog
	{
		/// <summary>
		/// 
		/// </summary>
		void Close();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args_"></param>
        void Write(FlowGraphBase.Logger.LogVerbosity verbose_, string msg_);
	}
}
