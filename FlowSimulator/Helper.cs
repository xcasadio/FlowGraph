using System;
using System.Windows;
using System.Windows.Media;

namespace FlowSimulator
{
    /// <summary>
    /// 
    /// </summary>
    static class Helper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        public static Exception GetFirstException(Exception ex)
        {
            Exception e = ex;

            //only the first exception is useful
            while (e.InnerException != null)
            {
                e = e.InnerException;
            }

            return e;
        }

        /// <summary>
        /// Finds a parent of a given item on the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="child">A direct or indirect child of the
        /// queried item.</param>
        /// <returns>The first parent item that matches the submitted
        /// type parameter. If not matching item can be found, a null
        /// reference is being returned.</returns>
        public static T TryFindParent<T>(this DependencyObject child)
            where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = GetParentObject(child);

            //we've reached the end of the tree
            if (parentObject == null)
            {
                return null;
            }

            //check if the parent matches the type we're looking for
            if (parentObject is T parent)
            {
                return parent;
            }

            //use recursion to proceed with next level
            return TryFindParent<T>(parentObject);
        }

        /// <summary>
        /// This method is an alternative to WPF's
        /// <see cref="VisualTreeHelper.GetParent"/> method, which also
        /// supports content elements. Keep in mind that for content element,
        /// this method falls back to the logical tree of the element!
        /// </summary>
        /// <param name="child">The item to be processed.</param>
        /// <returns>The submitted item's parent, if available. Otherwise
        /// null.</returns>
        public static DependencyObject GetParentObject(this DependencyObject child)
        {
            if (child == null)
            {
                return null;
            }

            //handle content elements separately
            if (child is ContentElement contentElement)
            {
                DependencyObject parent = ContentOperations.GetParent(contentElement);
                if (parent != null)
                {
                    return parent;
                }

                if (contentElement is FrameworkContentElement fce)
                {
                    return fce.Parent;
                }
            }

            //also try searching for parent in framework elements (such as DockPanel, etc)
            if (child is FrameworkElement frameworkElement)
            {
                DependencyObject parent = frameworkElement.Parent;
                if (parent != null)
                {
                    return parent;
                }
            }

            //if it's not a ContentElement/FrameworkElement, rely on VisualTreeHelper
            return VisualTreeHelper.GetParent(child);
        }

        /// <summary>
        /// Helper to search up the VisualTree
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="current"></param>
        /// <returns></returns>
        public static T FindAnchestor<T>(DependencyObject current)
            where T : DependencyObject
        {
            do
            {
                if (current is T anchestor)
                {
                    return anchestor;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }
    }
}
