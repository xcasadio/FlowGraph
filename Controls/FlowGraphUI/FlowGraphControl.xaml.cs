﻿using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using FlowGraph;
using FlowGraph.Logger;
using FlowGraph.Nodes;
using FlowGraph.Nodes.Variables;
using Logger;
using NetworkModel;
using NetworkUI;
using ZoomAndPan;

namespace FlowGraphUI;

public partial class FlowGraphControl : UserControl
{
    private static readonly List<NodeViewModel> ClipboardNodes = new();

    private bool _isContextMenuCreated;

    public event SelectionChangedEventHandler SelectionChanged;

    public SequenceViewModel ViewModel => (SequenceViewModel)DataContext;

    public FlowGraphControl()
    {
        DataContextChanged += OnDataContextChanged;
        InitializeComponent();

        Loaded += OnLoaded;
    }

    void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue is SequenceViewModel oldSequenceViewModel)
        {
            oldSequenceViewModel.ContextMenuOpened -= OnContextMenuOpened;
        }

        if (e.NewValue is SequenceViewModel sequenceViewModel)
        {
            sequenceViewModel.ContextMenuOpened += OnContextMenuOpened;
        }
    }

    void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (_isContextMenuCreated)
        {
            return;
        }

        foreach (var nodesByCategory in NodeRegister.NodeTypesByCategory)
        {
            var parent = CreateParentMenuItemNode(nodesByCategory.Key, menuItemCreateNode);

            foreach (var nodeDescription in nodesByCategory.Value.OrderBy(x => x.name))
            {
                var item = new MenuItem
                {
                    Header = nodeDescription.name,
                    Tag = nodeDescription.type
                };
                item.Click += MenuItemCreateNode_Click;
                parent.Items.Add(item);
            }
        }

        _isContextMenuCreated = true;
    }

    MenuItem CreateParentMenuItemNode(string categPath, MenuItem parent)
    {
        if (string.IsNullOrWhiteSpace(categPath))
        {
            return parent;
        }

        var folders = categPath.Split('/');
        categPath = categPath.Remove(0, folders[0].Length);
        if (categPath.Length > 1)
        {
            categPath = categPath.Remove(0, 1);
        }

        foreach (MenuItem item in parent.Items)
        {
            if (folders[0].Equals(item.Header))
            {
                return CreateParentMenuItemNode(categPath, item);
            }
        }

        var child = new MenuItem { Header = folders[0] };
        parent.Items.Add(child);

        return CreateParentMenuItemNode(categPath, child);
    }

    void MenuItemCreateNode_Click(object sender, RoutedEventArgs e)
    {
        if (sender is MenuItem item)
        {
            var type = item.Tag as Type;
            CreateNode((SequenceNode)Activator.CreateInstance(type));
        }
    }

    void OnContextMenuOpened(object? sender, EventArgs e)
    {
        ContextMenu.IsOpen = true;
    }

    /// <summary>
    /// Event raised when the user has started to drag out a connection.
    /// </summary>
    private void networkControl_ConnectionDragStarted(object sender, ConnectionDragStartedEventArgs e)
    {
        var draggedOutConnector = (ConnectorViewModel)e.ConnectorDraggedOut;
        var curDragPoint = Mouse.GetPosition(networkControl);

        //
        // Delegate the real work to the view model.
        //
        var connection = ViewModel.ConnectionDragStarted(draggedOutConnector, curDragPoint);

        //
        // Must return the view-model object that represents the connection via the event args.
        // This is so that NetworkView can keep track of the object while it is being dragged.
        //
        e.Connection = connection;
    }

    /// <summary>
    /// Event raised, to query for feedback, while the user is dragging a connection.
    /// </summary>
    private void networkControl_QueryConnectionFeedback(object sender, QueryConnectionFeedbackEventArgs e)
    {
        var draggedOutConnector = (ConnectorViewModel)e.ConnectorDraggedOut;
        var draggedOverConnector = (ConnectorViewModel)e.DraggedOverConnector;
        object feedbackIndicator = null;
        var connectionOk = true;

        ViewModel.QueryConnectionFeedback(draggedOutConnector, draggedOverConnector, out feedbackIndicator, out connectionOk);

        //
        // Return the feedback object to NetworkView.
        // The object combined with the data-template for it will be used to create a 'feedback icon' to
        // display (in an adorner) to the user.
        //
        e.FeedbackIndicator = feedbackIndicator;

        //
        // Let NetworkView know if the connection is ok or not ok.
        //
        e.ConnectionOk = connectionOk;
    }

    /// <summary>
    /// Event raised while the user is dragging a connection.
    /// </summary>
    private void networkControl_ConnectionDragging(object sender, ConnectionDraggingEventArgs e)
    {
        var curDragPoint = Mouse.GetPosition(networkControl);
        var connection = (ConnectionViewModel)e.Connection;
        ViewModel.ConnectionDragging(curDragPoint, connection);
    }

    /// <summary>
    /// Event raised when the user has finished dragging out a connection.
    /// </summary>
    private void networkControl_ConnectionDragCompleted(object sender, ConnectionDragCompletedEventArgs e)
    {
        var connectorDraggedOut = (ConnectorViewModel)e.ConnectorDraggedOut;
        var connectorDraggedOver = (ConnectorViewModel)e.ConnectorDraggedOver;
        var newConnection = (ConnectionViewModel)e.Connection;
        ViewModel.ConnectionDragCompleted(newConnection, connectorDraggedOut, connectorDraggedOver);
    }

    /// <summary>
    /// Event raised to delete the selected node.
    /// </summary>
    private void DeleteSelectedNodes_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        networkControl.IsUndoRegisterEnabled = false;
        ViewModel.DeleteSelectedNodes();
        networkControl.IsUndoRegisterEnabled = true;
    }

    /// <summary>
    /// Event raised to delete a node.
    /// </summary>
    private void DeleteNode_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        var node = (NodeViewModel)e.Parameter;
        ViewModel.DeleteNode(node, true);
    }

    /// <summary>
    /// Event raised to delete a connection.
    /// </summary>
    private void DeleteConnection_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        var connection = (ConnectionViewModel)e.Parameter;
        ViewModel.DeleteConnection(connection, true);
    }

    /// <summary>
    /// Creates a new node in the NetworkViewModel at the current mouse location.
    /// </summary>
    private void CreateNode(SequenceNode node)
    {
        //var newNodePosition = Mouse.GetPosition(networkControl);
        ViewModel.CreateNode(node, _origContentMouseDownPoint, true);
    }

    /// <summary>
    /// Event raised when the size of a node has changed.
    /// </summary>
    private void Node_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        //
        // The size of a node, as determined in the UI by the node's data-template,
        // has changed.  Push the size of the node through to the view-model.
        //
        var element = (FrameworkElement)sender;
        var node = (NodeViewModel)element.DataContext;
        node.Size = new Size(element.ActualWidth, element.ActualHeight);
    }

    /// <summary>
    /// Specifies the current state of the mouse handling logic.
    /// </summary>
    private MouseHandlingMode _mouseHandlingMode = MouseHandlingMode.None;

    /// <summary>
    /// The point that was clicked relative to the ZoomAndPanControl.
    /// </summary>
    private Point _origZoomAndPanControlMouseDownPoint;

    /// <summary>
    /// The point that was clicked relative to the content that is contained within the ZoomAndPanControl.
    /// </summary>
    private Point _origContentMouseDownPoint;

    /// <summary>
    /// Records which mouse button clicked during mouse dragging.
    /// </summary>
    private MouseButton _mouseButtonDown;

    /// <summary>
    /// Saves the previous zoom rectangle, pressing the backspace key jumps back to this zoom rectangle.
    /// </summary>
    private Rect _prevZoomRect;

    /// <summary>
    /// Save the previous content scale, pressing the backspace key jumps back to this scale.
    /// </summary>
    private double _prevZoomScale;

    /// <summary>
    /// Set to 'true' when the previous zoom rect is saved.
    /// </summary>
    private bool _prevZoomRectSet;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    //         protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
    //         {
    //             if (networkControl.IsDragging == true)
    //             {
    //                 scrollViewer.DoMouseDown(e);
    //             }
    // 
    //             base.OnPreviewMouseDown(e);
    //         }

    protected override void OnPreviewMouseMove(MouseEventArgs e)
    {
        if (networkControl.IsDragging)
        {
            scrollViewer.DoMouseDown();
        }

        base.OnPreviewMouseMove(e);
    }

    protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
    {
        scrollViewer.DoMouseUp();
        base.OnPreviewMouseUp(e);
    }

    private void MouseDragScrollViewerDragHorizontal(double offset)
    {
        zoomAndPanControl.ContentOffsetX += offset;
    }

    private void MouseDragScrollViewerDragVertical(double offset)
    {
        zoomAndPanControl.ContentOffsetY += offset;
    }

    /// <summary>
    /// Event raised on mouse down in the NetworkView.
    /// </summary> 
    private void networkControl_MouseDown(object sender, MouseButtonEventArgs e)
    {
        networkControl.Focus();
        Keyboard.Focus(networkControl);

        _mouseButtonDown = e.ChangedButton;
        _origZoomAndPanControlMouseDownPoint = e.GetPosition(zoomAndPanControl);
        _origContentMouseDownPoint = e.GetPosition(networkControl);

        if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0 &&
            (e.ChangedButton == MouseButton.Left ||
             e.ChangedButton == MouseButton.Right))
        {
            // Shift + left- or right-down initiates zooming mode.
            _mouseHandlingMode = MouseHandlingMode.Zooming;
        }
        else if (_mouseButtonDown == MouseButton.Left &&
                 (Keyboard.Modifiers & ModifierKeys.Control) == 0)
        {
            //
            // Initiate panning, when control is not held down.
            // When control is held down left dragging is used for drag selection.
            // After panning has been initiated the user must drag further than the threshold value to actually start drag panning.
            //
            _mouseHandlingMode = MouseHandlingMode.Panning;
        }

        if (_mouseHandlingMode != MouseHandlingMode.None)
        {
            // Capture the mouse so that we eventually receive the mouse up event.
            networkControl.CaptureMouse();
            e.Handled = true;
        }
    }

    /// <summary>
    /// Event raised on mouse up in the NetworkView.
    /// </summary>
    private void networkControl_MouseUp(object sender, MouseButtonEventArgs e)
    {
        if (_mouseHandlingMode != MouseHandlingMode.None)
        {
            if (_mouseHandlingMode == MouseHandlingMode.Panning)
            {
                //
                // Panning was initiated but dragging was abandoned before the mouse
                // cursor was dragged further than the threshold distance.
                // This means that this basically just a regular left mouse click.
                // Because it was a mouse click in empty space we need to clear the current selection.
                //
            }
            else if (_mouseHandlingMode == MouseHandlingMode.Zooming)
            {
                if (_mouseButtonDown == MouseButton.Left)
                {
                    // Shift + left-click zooms in on the content.
                    ZoomIn(_origContentMouseDownPoint);
                }
                else if (_mouseButtonDown == MouseButton.Right)
                {
                    // Shift + left-click zooms out from the content.
                    ZoomOut(_origContentMouseDownPoint);
                }
            }
            else if (_mouseHandlingMode == MouseHandlingMode.DragZooming)
            {
                // When drag-zooming has finished we zoom in on the rectangle that was highlighted by the user.
                ApplyDragZoomRect();
            }

            //
            // Reenable clearing of selection when empty space is clicked.
            // This is disabled when drag panning is in progress.
            //
            networkControl.IsClearSelectionOnEmptySpaceClickEnabled = true;

            //
            // Reset the override cursor.
            // This is set to a special cursor while drag panning is in progress.
            //
            Mouse.OverrideCursor = null;

            networkControl.ReleaseMouseCapture();
            _mouseHandlingMode = MouseHandlingMode.None;
            e.Handled = true;
        }
    }

    /// <summary>
    /// Event raised on mouse move in the NetworkView.
    /// </summary>
    private void networkControl_MouseMove(object sender, MouseEventArgs e)
    {
        if (_mouseHandlingMode == MouseHandlingMode.Panning)
        {
            var curZoomAndPanControlMousePoint = e.GetPosition(zoomAndPanControl);
            var dragOffset = curZoomAndPanControlMousePoint - _origZoomAndPanControlMouseDownPoint;
            double dragThreshold = 10;
            if (Math.Abs(dragOffset.X) > dragThreshold ||
                Math.Abs(dragOffset.Y) > dragThreshold)
            {
                //
                // The user has dragged the cursor further than the threshold distance, initiate
                // drag panning.
                //
                _mouseHandlingMode = MouseHandlingMode.DragPanning;
                networkControl.IsClearSelectionOnEmptySpaceClickEnabled = false;
                Mouse.OverrideCursor = Cursors.ScrollAll;
            }

            e.Handled = true;
        }
        else if (_mouseHandlingMode == MouseHandlingMode.DragPanning)
        {
            //
            // The user is left-dragging the mouse.
            // Pan the viewport by the appropriate amount.
            //
            var curContentMousePoint = e.GetPosition(networkControl);
            var dragOffset = curContentMousePoint - _origContentMouseDownPoint;

            zoomAndPanControl.ContentOffsetX -= dragOffset.X;
            zoomAndPanControl.ContentOffsetY -= dragOffset.Y;

            e.Handled = true;
        }
        else if (_mouseHandlingMode == MouseHandlingMode.Zooming)
        {
            var curZoomAndPanControlMousePoint = e.GetPosition(zoomAndPanControl);
            var dragOffset = curZoomAndPanControlMousePoint - _origZoomAndPanControlMouseDownPoint;
            double dragThreshold = 10;
            if (_mouseButtonDown == MouseButton.Left &&
                (Math.Abs(dragOffset.X) > dragThreshold ||
                 Math.Abs(dragOffset.Y) > dragThreshold))
            {
                //
                // When Shift + left-down zooming mode and the user drags beyond the drag threshold,
                // initiate drag zooming mode where the user can drag out a rectangle to select the area
                // to zoom in on.
                //
                _mouseHandlingMode = MouseHandlingMode.DragZooming;
                var curContentMousePoint = e.GetPosition(networkControl);
                InitDragZoomRect(_origContentMouseDownPoint, curContentMousePoint);
            }

            e.Handled = true;
        }
        else if (_mouseHandlingMode == MouseHandlingMode.DragZooming)
        {
            //
            // When in drag zooming mode continously update the position of the rectangle
            // that the user is dragging out.
            //
            var curContentMousePoint = e.GetPosition(networkControl);
            SetDragZoomRect(_origContentMouseDownPoint, curContentMousePoint);

            e.Handled = true;
        }
    }

    /// <summary>
    /// Event raised by rotating the mouse wheel.
    /// </summary>
    private void networkControl_MouseWheel(object sender, MouseWheelEventArgs e)
    {
        e.Handled = true;

        if (e.Delta > 0)
        {
            var curContentMousePoint = e.GetPosition(networkControl);
            ZoomIn(curContentMousePoint);
        }
        else if (e.Delta < 0)
        {
            var curContentMousePoint = e.GetPosition(networkControl);
            ZoomOut(curContentMousePoint);
        }
    }

    /// <summary>
    /// Event raised when the user has double clicked in the zoom and pan control.
    /// </summary>
    private void networkControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if ((Keyboard.Modifiers & ModifierKeys.Shift) == 0)
        {
            var doubleClickPoint = e.GetPosition(networkControl);
            zoomAndPanControl.AnimatedSnapTo(doubleClickPoint);
        }
    }

    /// <summary>
    /// The 'ZoomIn' command (bound to the plus key) was executed.
    /// </summary>
    private void ZoomIn_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        var o = networkControl.SelectedNode;

        ZoomIn(new Point(zoomAndPanControl.ContentZoomFocusX, zoomAndPanControl.ContentZoomFocusY));
    }

    /// <summary>
    /// The 'ZoomOut' command (bound to the minus key) was executed.
    /// </summary>
    private void ZoomOut_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        ZoomOut(new Point(zoomAndPanControl.ContentZoomFocusX, zoomAndPanControl.ContentZoomFocusY));
    }

    /// <summary>
    /// The 'JumpBackToPrevZoom' command was executed.
    /// </summary>
    private void JumpBackToPrevZoo_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        JumpBackToPrevZoom();
    }

    /// <summary>
    /// Determines whether the 'JumpBackToPrevZoom' command can be executed.
    /// </summary>
    private void JumpBackToPrevZoo_CanExecuted(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = _prevZoomRectSet;
    }

    /// <summary>
    /// The 'Fill' command was executed.
    /// </summary>
    private void FitContent_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        IList nodes = null;

        if (networkControl.SelectedNodes.Count > 0)
        {
            nodes = networkControl.SelectedNodes;
        }
        else
        {
            nodes = ViewModel.Network.Nodes;
            if (nodes.Count == 0)
            {
                return;
            }
        }

        SavePrevZoomRect();

        var actualContentRect = DetermineAreaOfNodes(nodes);

        //
        // Inflate the content rect by a fraction of the actual size of the total content area.
        // This puts a nice border around the content we are fitting to the viewport.
        //
        actualContentRect.Inflate(networkControl.ActualWidth / 40, networkControl.ActualHeight / 40);

        zoomAndPanControl.AnimatedZoomTo(actualContentRect);
    }

    /// <summary>
    /// Determine the area covered by the specified list of nodes.
    /// </summary>
    private Rect DetermineAreaOfNodes(IList nodes)
    {
        var firstNode = (NodeViewModel)nodes[0];
        var actualContentRect = new Rect(firstNode.X, firstNode.Y, firstNode.Size.Width, firstNode.Size.Height);

        for (var i = 1; i < nodes.Count; ++i)
        {
            var node = (NodeViewModel)nodes[i];
            var nodeRect = new Rect(node.X, node.Y, node.Size.Width, node.Size.Height);
            actualContentRect = Rect.Union(actualContentRect, nodeRect);
        }
        return actualContentRect;
    }

    /// <summary>
    /// The 'Fill' command was executed.
    /// </summary>
    private void Fill_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        SavePrevZoomRect();

        zoomAndPanControl.AnimatedScaleToFit();
    }

    /// <summary>
    /// The 'OneHundredPercent' command was executed.
    /// </summary>
    private void OneHundredPercent_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        SavePrevZoomRect();

        zoomAndPanControl.AnimatedZoomTo(1.0);
    }

    /// <summary>
    /// Jump back to the previous zoom level.
    /// </summary>
    private void JumpBackToPrevZoom()
    {
        zoomAndPanControl.AnimatedZoomTo(_prevZoomScale, _prevZoomRect);

        ClearPrevZoomRect();
    }

    /// <summary>
    /// Zoom the viewport out, centering on the specified point (in content coordinates).
    /// </summary>
    private void ZoomOut(Point contentZoomCenter)
    {
        zoomAndPanControl.ZoomAboutPoint(zoomAndPanControl.ContentScale - 0.1, contentZoomCenter);
    }

    /// <summary>
    /// Zoom the viewport in, centering on the specified point (in content coordinates).
    /// </summary>
    private void ZoomIn(Point contentZoomCenter)
    {
        zoomAndPanControl.ZoomAboutPoint(zoomAndPanControl.ContentScale + 0.1, contentZoomCenter);
    }

    /// <summary>
    /// Initialize the rectangle that the use is dragging out.
    /// </summary>
    private void InitDragZoomRect(Point pt1, Point pt2)
    {
        SetDragZoomRect(pt1, pt2);

        dragZoomCanvas.Visibility = Visibility.Visible;
        dragZoomBorder.Opacity = 0.5;
    }

    /// <summary>
    /// Update the position and size of the rectangle that user is dragging out.
    /// </summary>
    private void SetDragZoomRect(Point pt1, Point pt2)
    {
        double x, y, width, height;

        //
        // Deterine x,y,width and height of the rect inverting the points if necessary.
        // 

        if (pt2.X < pt1.X)
        {
            x = pt2.X;
            width = pt1.X - pt2.X;
        }
        else
        {
            x = pt1.X;
            width = pt2.X - pt1.X;
        }

        if (pt2.Y < pt1.Y)
        {
            y = pt2.Y;
            height = pt1.Y - pt2.Y;
        }
        else
        {
            y = pt1.Y;
            height = pt2.Y - pt1.Y;
        }

        //
        // Update the coordinates of the rectangle that is being dragged out by the user.
        // The we offset and rescale to convert from content coordinates.
        //
        Canvas.SetLeft(dragZoomBorder, x);
        Canvas.SetTop(dragZoomBorder, y);
        dragZoomBorder.Width = width;
        dragZoomBorder.Height = height;
    }

    /// <summary>
    /// When the user has finished dragging out the rectangle the zoom operation is applied.
    /// </summary>
    private void ApplyDragZoomRect()
    {
        SavePrevZoomRect();

        var contentX = Canvas.GetLeft(dragZoomBorder);
        var contentY = Canvas.GetTop(dragZoomBorder);
        var contentWidth = dragZoomBorder.Width;
        var contentHeight = dragZoomBorder.Height;
        zoomAndPanControl.AnimatedZoomTo(new Rect(contentX, contentY, contentWidth, contentHeight));

        FadeOutDragZoomRect();
    }

    private void FadeOutDragZoomRect()
    {
        AnimationHelper.StartAnimation(dragZoomBorder, OpacityProperty, 0.0, 0.1,
            delegate
            {
                dragZoomCanvas.Visibility = Visibility.Collapsed;
            });
    }

    private void SavePrevZoomRect()
    {
        _prevZoomRect = new Rect(zoomAndPanControl.ContentOffsetX, zoomAndPanControl.ContentOffsetY, zoomAndPanControl.ContentViewportWidth, zoomAndPanControl.ContentViewportHeight);
        _prevZoomScale = zoomAndPanControl.ContentScale;
        _prevZoomRectSet = true;
    }

    private void ClearPrevZoomRect()
    {
        _prevZoomRectSet = false;
    }

    private void networkControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is NetworkView view
            && view.IsUndoRegisterEnabled)
        {
            if (e.RemovedItems.Count > 0)
            {
                List<NodeViewModel> list = new();

                foreach (var node in e.RemovedItems)
                {
                    if (node is NodeViewModel model)
                    {
                        list.Add(model);
                    }
                }

                if (list.Count > 0)
                {
                    ViewModel.OnNodesDeselectedChanged(networkControl, list);
                }
            }

            if (e.AddedItems.Count > 0)
            {
                List<NodeViewModel> list = new();

                foreach (var node in e.AddedItems)
                {
                    if (node is NodeViewModel model)
                    {
                        list.Add(model);
                    }
                }

                if (list.Count > 0)
                {
                    ViewModel.OnNodesSelectedChanged(networkControl, list);
                }
            }
        }

        SelectionChanged?.Invoke(this, new SelectionChangedEventArgs(Selector.SelectionChangedEvent, e.RemovedItems, e.AddedItems));
    }

    private void EditCustomVariable_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (e.OriginalSource is FrameworkElement fe)
        {
            if (fe.DataContext is VariableNode varNode)
            {
                {
                    LogManager.Instance.WriteLine(LogVerbosity.Warning, "Variable node => No custom editor for this type");
                }
            }
        }
    }

    private void DropList_DragEnter(object sender, DragEventArgs e)
    {
        if (!e.Data.GetDataPresent(DataFormats.StringFormat) ||
            sender == e.Source)
        {
            e.Effects = DragDropEffects.None;
        }
    }

    private void DropList_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.StringFormat))
        {
            try
            {
                var data = e.Data.GetData(DataFormats.StringFormat) as string;

                if (data.StartsWith((string)FlowGraphDragAndDropManager.DragPrefixFunction))
                {
                    var id = data.Split('#')[1];
                    Debugger.Break();
                    //SequenceFunction func = GraphDataManager.Instance.GetFunctionById(int.Parse(id));
                    //CallFunctionNode seqNode = new CallFunctionNode(func);
                    //ViewModel.CreateNode(seqNode, e.GetPosition(networkControl), false);
                }
                else if (data.StartsWith((string)FlowGraphDragAndDropManager.DragPrefixNamedVar))
                {
                    var name = data.Split('#')[1];
                    var seqNode = new NamedVariableNode(name);
                    ViewModel.CreateNode(seqNode, e.GetPosition(networkControl), false);
                }
            }
            catch (System.Exception ex)
            {
                LogManager.Instance.WriteException(ex);
            }
        }
    }

    private void FlowGraphCopy_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        try
        {
            if (networkControl.SelectedNodes.Count > 0)
            {
                ClipboardNodes.Clear();

                foreach (var node in networkControl.SelectedNodes)
                {
                    ClipboardNodes.Add(node as NodeViewModel);
                }
            }
        }
        catch (System.Exception ex)
        {
            LogManager.Instance.WriteException(ex);
        }
    }

    private void FlowGraphPaste_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        try
        {
            if (ClipboardNodes.Count > 0)
            {
                networkControl.SelectedNodes.Clear();
                networkControl.IsUndoRegisterEnabled = false;
                var nodes = ViewModel.CopyNodes(ClipboardNodes);

                foreach (var node in nodes)
                {
                    networkControl.SelectedNodes.Add(node);
                }

                networkControl.IsUndoRegisterEnabled = true;
            }
        }
        catch (System.Exception ex)
        {
            LogManager.Instance.WriteException(ex);
        }
    }

    private void FlowGraphUndo_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        ViewModel.UndoRedoManager.Undo();
    }

    private void FlowGraphRedo_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        ViewModel.UndoRedoManager.Redo();
    }

    private void networkControl_NodeDragStarted(object sender, NodeDragStartedEventArgs e)
    {
        ViewModel.OnNodeDragStarted(sender as NetworkView, e);
    }

    private void networkControl_NodeDragCompleted(object sender, NodeDragCompletedEventArgs e)
    {
        ViewModel.OnNodeDragCompleted(sender as NetworkView, e);
    }
}