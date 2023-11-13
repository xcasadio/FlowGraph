using System.Windows;
using System.Windows.Media;

namespace FlowGraphUI;

public static class DependencyObjectHelper
{
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
        DependencyObject parentObject = GetParentObject(child);

        if (parentObject == null)
        {
            return null;
        }

        if (parentObject is T parent)
        {
            return parent;
        }

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

        if (child is FrameworkElement frameworkElement)
        {
            DependencyObject parent = frameworkElement.Parent;
            if (parent != null)
            {
                return parent;
            }
        }

        return VisualTreeHelper.GetParent(child);
    }

    /// <summary>
    /// DependencyObjectHelper to search up the VisualTree
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