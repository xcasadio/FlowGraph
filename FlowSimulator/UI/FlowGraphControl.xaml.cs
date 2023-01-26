using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using CustomNode;
using FlowGraph;
using FlowGraph.Logger;
using FlowGraph.Node;
using FlowGraph.Node.StandardActionNode;
using FlowGraph.Node.StandardVariableNode;
using FlowGraph.Process;
using FlowGraph.Script;
using NetworkModel;
using NetworkUI;
using ZoomAndPan;

namespace FlowSimulator.UI
{
    /// <summary>
    /// Interaction logic for FlowGraphControl.xaml
    /// </summary>
    public partial class FlowGraphControl : UserControl
    {
        /// <summary>
        /// Use to copy/paste nodes (shared with all graphs)
        /// </summary>
        private static readonly List<NodeViewModel> ClipboardNodes = new List<NodeViewModel>(10);

        private bool _isContextMenuCreated;


        /// <summary>
        /// An event raised when the nodes selected in the NetworkView has changed.
        /// </summary>
        public event SelectionChangedEventHandler SelectionChanged;

        /// <summary>
        /// Convenient accessor for the view-model.
        /// </summary>
        public FlowGraphControlViewModel ViewModel => (FlowGraphControlViewModel)DataContext;

        /// <summary>
        /// 
        /// </summary>
        public FlowGraphControl()
        {
            DataContextChanged += OnDataContextChanged;
            InitializeComponent();

            Loaded += OnLoaded;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            FlowGraphControlViewModel fgcvm;

            if (e.OldValue is FlowGraphControlViewModel)
            {
                fgcvm = DataContext as FlowGraphControlViewModel;
                fgcvm.ContextMenuOpened -= OnContextMenuOpened;
            }

            if (e.NewValue is FlowGraphControlViewModel)
            {
                fgcvm = DataContext as FlowGraphControlViewModel;
                fgcvm.ContextMenuOpened += OnContextMenuOpened;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (_isContextMenuCreated == false)
            {
                _isContextMenuCreated = true;

                IEnumerable<Type> classes = AppDomain.CurrentDomain.GetAssemblies()
                           .SelectMany(t => t.GetTypes())
                           .Where(t => t.IsClass
                               && t.IsGenericType == false
                               && t.IsInterface == false
                               && t.IsAbstract == false
                               && t.IsSubclassOf(typeof(SequenceNode)));

                foreach (Type type in classes)
                {
                    Attribute browsableAtt = Attribute.GetCustomAttribute(type, typeof(Visible), true);
                    if (browsableAtt != null
                        && ((Visible)browsableAtt).Value == false)
                    {
                        continue;
                    }

                    Attribute categAtt = Attribute.GetCustomAttribute(type, typeof(Category), true);
                    Attribute nameAtt = Attribute.GetCustomAttribute(type, typeof(Name), true);

                    if (nameAtt == null
                        || string.IsNullOrWhiteSpace(((Name)nameAtt).DisplayName))
                    {
                        LogManager.Instance.WriteLine(
                            LogVerbosity.Error,
                            "Can't create menu for the type '{0}' because the attribute Name is not specified",
                            type.FullName);
                        continue;
                    }

                    string categPath = categAtt == null ? "" : ((Category)categAtt).CategoryPath;

                    MenuItem parent = CreateParentMenuItemNode(categPath, menuItemCreateNode);
                    MenuItem item = new MenuItem
                    {
                        Header = ((Name)nameAtt).DisplayName,
                        Tag = type
                    };
                    item.Click += MenuItemCreateNode_Click;
                    parent.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categPath_"></param>
        /// <param name="name_"></param>
        MenuItem CreateParentMenuItemNode(string categPath, MenuItem parent)
        {
            if (string.IsNullOrWhiteSpace(categPath))
            {
                return parent;
            }

            string[] folders = categPath.Split('/');
            categPath = categPath.Remove(0, folders[0].Length);
            if (categPath.Length > 1) categPath = categPath.Remove(0, 1);

            foreach (MenuItem item in parent.Items)
            {
                if (folders[0].Equals(item.Header))
                {
                    return CreateParentMenuItemNode(categPath, item);
                }
            }

            MenuItem child = new MenuItem { Header = folders[0] };
            parent.Items.Add(child);

            return CreateParentMenuItemNode(categPath, child);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MenuItemCreateNode_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem item)
            {
                Type type = item.Tag as Type;
                CreateNode((SequenceNode)Activator.CreateInstance(type));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnContextMenuOpened(object sender, EventArgs e)
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
            bool connectionOk = true;

            ViewModel.QueryConnnectionFeedback(draggedOutConnector, draggedOverConnector, out feedbackIndicator, out connectionOk);

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
            Point curDragPoint = Mouse.GetPosition(networkControl);
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
                Point curZoomAndPanControlMousePoint = e.GetPosition(zoomAndPanControl);
                Vector dragOffset = curZoomAndPanControlMousePoint - _origZoomAndPanControlMouseDownPoint;
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
                Point curContentMousePoint = e.GetPosition(networkControl);
                Vector dragOffset = curContentMousePoint - _origContentMouseDownPoint;

                zoomAndPanControl.ContentOffsetX -= dragOffset.X;
                zoomAndPanControl.ContentOffsetY -= dragOffset.Y;

                e.Handled = true;
            }
            else if (_mouseHandlingMode == MouseHandlingMode.Zooming)
            {
                Point curZoomAndPanControlMousePoint = e.GetPosition(zoomAndPanControl);
                Vector dragOffset = curZoomAndPanControlMousePoint - _origZoomAndPanControlMouseDownPoint;
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
                    Point curContentMousePoint = e.GetPosition(networkControl);
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
                Point curContentMousePoint = e.GetPosition(networkControl);
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
                Point curContentMousePoint = e.GetPosition(networkControl);
                ZoomIn(curContentMousePoint);
            }
            else if (e.Delta < 0)
            {
                Point curContentMousePoint = e.GetPosition(networkControl);
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
                Point doubleClickPoint = e.GetPosition(networkControl);
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

            Rect actualContentRect = DetermineAreaOfNodes(nodes);

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
            NodeViewModel firstNode = (NodeViewModel)nodes[0];
            Rect actualContentRect = new Rect(firstNode.X, firstNode.Y, firstNode.Size.Width, firstNode.Size.Height);

            for (int i = 1; i < nodes.Count; ++i)
            {
                NodeViewModel node = (NodeViewModel)nodes[i];
                Rect nodeRect = new Rect(node.X, node.Y, node.Size.Width, node.Size.Height);
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
            //
            // Record the previous zoom level, so that we can jump back to it when the backspace key is pressed.
            //
            SavePrevZoomRect();

            //
            // Retreive the rectangle that the user draggged out and zoom in on it.
            //
            double contentX = Canvas.GetLeft(dragZoomBorder);
            double contentY = Canvas.GetTop(dragZoomBorder);
            double contentWidth = dragZoomBorder.Width;
            double contentHeight = dragZoomBorder.Height;
            zoomAndPanControl.AnimatedZoomTo(new Rect(contentX, contentY, contentWidth, contentHeight));

            FadeOutDragZoomRect();
        }

        //
        // Fade out the drag zoom rectangle.
        //
        private void FadeOutDragZoomRect()
        {
            AnimationHelper.StartAnimation(dragZoomBorder, OpacityProperty, 0.0, 0.1,
                delegate
                {
                    dragZoomCanvas.Visibility = Visibility.Collapsed;
                });
        }

        //
        // Record the previous zoom level, so that we can jump back to it when the backspace key is pressed.
        //
        private void SavePrevZoomRect()
        {
            _prevZoomRect = new Rect(zoomAndPanControl.ContentOffsetX, zoomAndPanControl.ContentOffsetY, zoomAndPanControl.ContentViewportWidth, zoomAndPanControl.ContentViewportHeight);
            _prevZoomScale = zoomAndPanControl.ContentScale;
            _prevZoomRectSet = true;
        }

        /// <summary>
        /// Clear the memory of the previous zoom level.
        /// </summary>
        private void ClearPrevZoomRect()
        {
            _prevZoomRectSet = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void networkControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is NetworkView view
                && view.IsUndoRegisterEnabled)
            {
                if (e.RemovedItems.Count > 0)
                {
                    List<NodeViewModel> list = new List<NodeViewModel>();

                    foreach (object node in e.RemovedItems)
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
                    List<NodeViewModel> list = new List<NodeViewModel>();

                    foreach (object node in e.AddedItems)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DropList_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.StringFormat) ||
                sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DropList_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                try
                {
                    string data = e.Data.GetData(DataFormats.StringFormat) as string;

                    if (data.StartsWith(FlowGraphDataControl.DragPrefixFunction))
                    {
                        string id = data.Split('#')[1];
                        SequenceFunction func = GraphDataManager.Instance.GetFunctionById(int.Parse(id));
                        CallFunctionNode seqNode = new CallFunctionNode(func);
                        ViewModel.CreateNode(seqNode, e.GetPosition(networkControl), false);
                    }
                    else if (data.StartsWith(FlowGraphDataControl.DragPrefixNamedVar))
                    {
                        string name = data.Split('#')[1];
                        NamedVariableNode seqNode = new NamedVariableNode(name);
                        ViewModel.CreateNode(seqNode, e.GetPosition(networkControl), false);
                    }
                    else if (data.StartsWith(FlowGraphDataControl.DragPrefixScriptElement))
                    {
                        string idStr = data.Split('#')[1];
                        int id = int.Parse(idStr);
                        ScriptElement el = GraphDataManager.Instance.GetScriptById(id);
                        ScriptNode seqNode = new ScriptNode(el);
                        ViewModel.CreateNode(seqNode, e.GetPosition(networkControl), false);
                    }
                }
                catch (System.Exception ex)
                {
                    LogManager.Instance.WriteException(ex);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FlowGraphPaste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                if (ClipboardNodes.Count > 0)
                {
                    networkControl.SelectedNodes.Clear();
                    networkControl.IsUndoRegisterEnabled = false;
                    IEnumerable<NodeViewModel> nodes = ViewModel.CopyNodes(ClipboardNodes);

                    foreach (NodeViewModel node in nodes)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FlowGraphUndo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.UndoRedoManager.Undo();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FlowGraphRedo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.UndoRedoManager.Redo();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Launch_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.CreateSequence();
            ProcessLauncher.Instance.LaunchSequence(ViewModel.Sequence, typeof(EventNodeTestStarted), 0, "test");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void networkControl_NodeDragStarted(object sender, NodeDragStartedEventArgs e)
        {
            ViewModel.OnNodeDragStarted(sender as NetworkView, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void networkControl_NodeDragCompleted(object sender, NodeDragCompletedEventArgs e)
        {
            ViewModel.OnNodeDragCompleted(sender as NetworkView, e);
        }
    }
}
