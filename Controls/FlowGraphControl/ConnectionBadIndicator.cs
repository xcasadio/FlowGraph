﻿namespace FlowGraphUI
{
    /// <summary>
    /// This class is just a place holder for showing a 'bad connection' indicator in the graph.
    /// It is used when a connection is dragged out to a connector, to which a valid connection can't be made.
    /// </summary>
    public class ConnectionBadIndicator
    {
        /// <summary>
        /// 
        /// </summary>
        public string Message
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public ConnectionBadIndicator(string msg)
        {
            Message = msg;
        }
    }
}
