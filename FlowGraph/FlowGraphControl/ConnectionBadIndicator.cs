using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlowGraphUI
{
    /// <summary>
    /// This class is just a place holder for showing a 'bad connection' indicator in the graph.
    /// It is used when a connection is dragged out to a connector, to which a valid connection can't be made.
    /// </summary>
    public class ConnectionBadIndicator
    {
        #region Fields

        #endregion //Fields

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public string Message
        {
            get;
            private set;
        }

        #endregion //Properties

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public ConnectionBadIndicator(string msg_)
        {
            Message = msg_;
        }

        #endregion //Constructors

        #region Methods

        #endregion //Methods
    }
}
