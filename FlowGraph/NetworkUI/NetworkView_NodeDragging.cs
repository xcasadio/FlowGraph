using System;
using System.Collections.Generic;

namespace NetworkUI
{
    /// <summary>
    /// Partial definition of the NetworkView class.
    /// This file only contains private members related to dragging nodes.
    /// </summary>
    public partial class NetworkView
    {
        #region Private Methods

        /// <summary>
        /// Event raised when the user starts to drag a node.
        /// </summary>
        private void NodeItem_DragStarted(object source, NodeDragStartedEventArgs e)
        {
            e.Handled = true;

            IsDragging = true;
            IsNotDragging = false;
            IsDraggingNode = true;
            IsNotDraggingNode = false;

            var eventArgs = new NodeDragStartedEventArgs(NodeDragStartedEvent, this, SelectedNodes);            
            RaiseEvent(eventArgs);

            e.Cancel = eventArgs.Cancel;
        }

        /// <summary>
        /// Event raised while the user is dragging a node.
        /// </summary>
        private void NodeItem_Dragging(object source, NodeDraggingEventArgs e)
        {
            e.Handled = true;

            //
            // Cache the NodeItem for each selected node whilst dragging is in progress.
            //
            if (cachedSelectedNodeItems == null)
            {
                cachedSelectedNodeItems = new List<NodeItem>();

                foreach (var selectedNode in SelectedNodes)
                {
                    NodeItem nodeItem = FindAssociatedNodeItem(selectedNode);
                    if (nodeItem == null)
                    {
                        throw new ApplicationException("Unexpected code path!");
                    }

                    cachedSelectedNodeItems.Add(nodeItem);
                }
            }

            // 
            // Update the position of the node within the Canvas.
            //
            foreach (var nodeItem in cachedSelectedNodeItems)
            {
                nodeItem.X += e.HorizontalChange;
                nodeItem.Y += e.VerticalChange;
            }

            var eventArgs = new NodeDraggingEventArgs(NodeDraggingEvent, this, SelectedNodes, e.HorizontalChange, e.VerticalChange);
            RaiseEvent(eventArgs);
        }

        /// <summary>
        /// Event raised when the user has finished dragging a node.
        /// </summary>
        private void NodeItem_DragCompleted(object source, NodeDragCompletedEventArgs e)
        {
            e.Handled = true;

            var eventArgs = new NodeDragCompletedEventArgs(NodeDragCompletedEvent, this, SelectedNodes);
            RaiseEvent(eventArgs);

            if (cachedSelectedNodeItems != null)
            {
                cachedSelectedNodeItems = null;
            }

            IsDragging = false;
            IsNotDragging = true;
            IsDraggingNode = false;
            IsNotDraggingNode = true;
        }

        #endregion Private Methods
    }
}
