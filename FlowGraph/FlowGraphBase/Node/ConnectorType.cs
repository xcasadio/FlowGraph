using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlowGraphBase.Node
{
    /// <summary>
    /// Defines the type of a connector (aka connection point).
    /// </summary>
    public enum ConnectorType
    {
        /// <summary>
        /// Used in UI when drag n drop
        /// </summary>
        Undefined,
        /// <summary>
        /// Can be attached to another output connector
        /// </summary>
        Input,
        /// <summary>
        /// Can be attached to another input connector
        /// </summary>
        Output,
        /// <summary>
        /// Can be attached to a node (variable slot) in order to read its value
        /// </summary>
        VariableInput,
        /// <summary>
        /// Can be attached to a node (variable slot) in order to set its value
        /// </summary>
        VariableOutput,
        /// <summary>
        /// Can be attached to a variable node in order to set/get its value
        /// </summary>
        VariableInputOutput,
    }
}
