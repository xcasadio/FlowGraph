using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace Utils
{
    /// <summary>
    /// This class contains helper functions for dealing with WPF.
    /// </summary>
    public static class WpfUtils
    {
        /// <summary>
        /// Search up the element tree to find the Parent window for 'element'.
        /// Returns null if the 'element' is not attached to a window.
        /// </summary>
        public static Window FindParentWindow(FrameworkElement element)
        {
            if (element.Parent == null)
            {
                return null;
            }

            if (element.Parent is Window window)
            {
                return window;
            }

            if (element.Parent is FrameworkElement parentElement)
            {
                return FindParentWindow(parentElement);
            }

            return null;
        }

        public static FrameworkElement FindParentWithDataContextAndName<DataContextT>(FrameworkElement childElement, string name)
            where DataContextT : class
        {
            FrameworkElement parent = (FrameworkElement)childElement.Parent;
            if (parent != null)
            {
                if (parent.DataContext is DataContextT data)
                {
                    if (parent.Name == name)
                    {
                        return parent;
                    }
                }

                parent = FindParentWithDataContextAndName<DataContextT>(parent, name);
                if (parent != null)
                {
                    return parent;
                }
            }

            parent = (FrameworkElement)childElement.TemplatedParent;
            if (parent != null)
            {
                if (parent.DataContext is DataContextT data)
                {
                    if (parent.Name == name)
                    {
                        return parent;
                    }
                }

                parent = FindParentWithDataContextAndName<DataContextT>(parent, name);
                if (parent != null)
                {
                    return parent;
                }
            }

            return null;
        }

        public static FrameworkElement FindParentWithDataContext<DataContextT>(FrameworkElement childElement)
            where DataContextT : class
        {
            if (childElement.Parent != null)
            {
                if (((FrameworkElement)childElement.Parent).DataContext is DataContextT data)
                {
                    return (FrameworkElement)childElement.Parent;
                }

                FrameworkElement parent = FindParentWithDataContext<DataContextT>((FrameworkElement)childElement.Parent);
                if (parent != null)
                {
                    return parent;
                }
            }

            if (childElement.TemplatedParent != null)
            {
                if (((FrameworkElement)childElement.TemplatedParent).DataContext is DataContextT data)
                {
                    return (FrameworkElement)childElement.TemplatedParent;
                }

                FrameworkElement parent = FindParentWithDataContext<DataContextT>((FrameworkElement)childElement.TemplatedParent);
                if (parent != null)
                {
                    return parent;
                }
            }

            return null;
        }

        public static ParentT FindVisualParentWithType<ParentT>(FrameworkElement childElement)
            where ParentT : class
        {
            FrameworkElement parentElement = (FrameworkElement)VisualTreeHelper.GetParent(childElement);
            if (parentElement != null)
            {
                if (parentElement is ParentT parent)
                {
                    return parent;
                }

                return FindVisualParentWithType<ParentT>(parentElement);
            }

            return null;
        }

        public static ParentT FindParentWithType<ParentT>(FrameworkElement childElement)
            where ParentT : class
        {
            if (childElement.Parent != null)
            {
                if (childElement.Parent is ParentT parent)
                {
                    return parent;
                }

                parent = FindParentWithType<ParentT>((FrameworkElement)childElement.Parent);
                if (parent != null)
        {
                    return parent;
                }
            }

            if (childElement.TemplatedParent != null)
            {
                if (childElement.TemplatedParent is ParentT parent)
                {
                    return parent;
                }

                parent = FindParentWithType<ParentT>((FrameworkElement)childElement.TemplatedParent);
                if (parent != null)
                {
                    return parent;
                }
            }

            FrameworkElement parentElement = (FrameworkElement)VisualTreeHelper.GetParent(childElement);
            if (parentElement != null)
            {
                if (parentElement is ParentT parent)
                {
                    return parent;
                }

                return FindParentWithType<ParentT>(parentElement);
            }

            return null;
        }

        public static ParentT FindParentWithTypeAndDataContext<ParentT>(FrameworkElement childElement, object dataContext)
            where ParentT : FrameworkElement
        {
            if (childElement.Parent != null)
            {
                if (childElement.Parent is ParentT parent)
                {
                    if (parent.DataContext == dataContext)
                    {
                        return parent;
                    }
                }

                parent = FindParentWithTypeAndDataContext<ParentT>((FrameworkElement)childElement.Parent, dataContext);
                if (parent != null)
                {
                    return parent;
                }
            }

            if (childElement.TemplatedParent != null)
            {
                if (childElement.TemplatedParent is ParentT parent)
                {
                    if (parent.DataContext == dataContext)
                    {
                        return parent;
                    }
                }

                parent = FindParentWithTypeAndDataContext<ParentT>((FrameworkElement)childElement.TemplatedParent, dataContext);
                if (parent != null)
                {
                    return parent;
                }
            }

            FrameworkElement parentElement = (FrameworkElement)VisualTreeHelper.GetParent(childElement);
            if (parentElement != null)
            {
                if (parentElement is ParentT parent)
                {
                    return parent;
                }

                return FindParentWithType<ParentT>(parentElement);
            }

            return null;
        }

        /// <summary>
        /// Hit test against the specified element for a child that has a data context
        /// of the specified type.
        /// Returns 'null' if nothing was 'hit'.
        /// Return the highest level element that matches the hit test.
        /// </summary>
        public static T HitTestHighestForDataContext<T>(FrameworkElement rootElement, Point point)
            where T : class
        {
            FrameworkElement hitFrameworkElement = null;
            return HitTestHighestForDataContext<T>(rootElement, point, out hitFrameworkElement);
        }

        /// <summary>
        /// Hit test against the specified element for a child that has a data context
        /// of the specified type.
        /// Returns 'null' if nothing was 'hit'.
        /// Return the highest level element that matches the hit test.
        /// </summary>
        public static T HitTestHighestForDataContext<T>(FrameworkElement rootElement,
                                                  Point point, out FrameworkElement hitFrameworkElement)
            where T : class
        {
            hitFrameworkElement = null;

            FrameworkElement hitElement = null;
            T hitData = HitTestForDataContext<T, FrameworkElement>(rootElement, point, out hitElement);
            if (hitData == null)
            {
                return null;
            }

            hitFrameworkElement = hitElement;

            //
            // Find the highest level parent below root element that still matches the data context.
            while (hitElement != null && hitElement != rootElement &&
                   hitElement.DataContext == hitData)
            {
                hitFrameworkElement = hitElement;

                if (hitElement.Parent != null)
                {
                    hitElement = hitElement.Parent as FrameworkElement;
                    continue;
                }

                if (hitElement.TemplatedParent != null)
                {
                    hitElement = hitElement.TemplatedParent as FrameworkElement;
                    continue;
            }

                break;
        }

            return hitData;
        }


        /// <summary>
        /// Hit test for a specific data context and name.
        /// </summary>
        public static DataContextT HitTestForDataContextAndName<DataContextT, ElementT>(FrameworkElement rootElement, 
                                          Point point, string name, out ElementT hitFrameworkElement)
            where DataContextT : class
            where ElementT : FrameworkElement
        {
            DataContextT data = null;
            hitFrameworkElement = null;
            ElementT frameworkElement = null;

            VisualTreeHelper.HitTest(
                    rootElement,
                    // Hit test filter.
                    null,
                    // Hit test result.
                    delegate(HitTestResult result)
                    {
                        frameworkElement = result.VisualHit as ElementT;
                        if (frameworkElement != null)
                        {
                            data = frameworkElement.DataContext as DataContextT;
                            if (data != null)
        {
                                if (frameworkElement.Name == name)
            {
                                    return HitTestResultBehavior.Stop;
                                }
                            }
                        }

                        return HitTestResultBehavior.Continue;
                    },

                    new PointHitTestParameters(point));

            hitFrameworkElement = frameworkElement;
            return data;
            }


        /// <summary>
        /// Hit test against the specified element for a child that has a data context
        /// of the specified type.
        /// Returns 'null' if nothing was 'hit'.
        /// </summary>
        public static DataContextT HitTestForDataContext<DataContextT, ElementT>(FrameworkElement rootElement, 
                                          Point point, out ElementT hitFrameworkElement)
            where DataContextT : class
            where ElementT : FrameworkElement
        {
            DataContextT data = null;
            hitFrameworkElement = null;
            ElementT frameworkElement = null;

            VisualTreeHelper.HitTest(
                    rootElement,
                    // Hit test filter.
                    null,
                    // Hit test result.
                    delegate(HitTestResult result)
                    {
                        frameworkElement = result.VisualHit as ElementT;
                        if (frameworkElement != null)
            {
                            data = frameworkElement.DataContext as DataContextT;
                            return data != null ? HitTestResultBehavior.Stop : HitTestResultBehavior.Continue;
            }

                        return HitTestResultBehavior.Continue;
                    },

                    new PointHitTestParameters(point));

            hitFrameworkElement = frameworkElement;
            return data;
        }

        /// <summary>
        /// Find the ancestor of a particular element based on the type of the ancestor.
        /// </summary>
        public static T FindAncestor<T>(FrameworkElement element) where T : class
        {
            if (element.Parent != null)
            {
                if (element.Parent is T ancestor)
                {
                    return ancestor;
                }

                if (element.Parent is FrameworkElement parent)
                {
                    return FindAncestor<T>(parent);
                }
            }

            if (element.TemplatedParent != null)
            {
                if (element.TemplatedParent is T ancestor)
                {
                    return ancestor;
                }

                if (element.TemplatedParent is FrameworkElement parent)
                    {
                    return FindAncestor<T>(parent);
                    }
                }

            DependencyObject visualParent = VisualTreeHelper.GetParent(element);
            if (visualParent != null)
            {
                if (visualParent is T visualAncestor)
                {
                    return visualAncestor;
            }

                if (visualParent is FrameworkElement visualElement)
            {
                    return FindAncestor<T>(visualElement);
                }
            }

                return null;
            }

        /// <summary>
        /// Transform a point to an ancestors coordinate system.
        /// </summary>
        public static Point TransformPointToAncestor<T>(FrameworkElement element, Point point) where T : Visual
        {
            T ancestor = FindAncestor<T>(element);
            if (ancestor == null)
            {
                throw new ApplicationException("Find to find '" + typeof(T).Name + "' for element '" + element.GetType().Name + "'.");
            }

            return TransformPointToAncestor(ancestor, element, point);
        }

        /// <summary>
        /// Transform a point to an ancestors coordinate system.
        /// </summary>
        public static Point TransformPointToAncestor(Visual ancestor, FrameworkElement element, Point point)
        {
            return element.TransformToAncestor(ancestor).Transform(point);
        }

        /// <summary>
        /// Find the framework element with the specified name.
        /// </summary>
        public static ElementT FindElementWithName<ElementT>(Visual rootElement, string name)
            where ElementT : FrameworkElement
        {
            if (rootElement is FrameworkElement rootFrameworkElement)
            {
                rootFrameworkElement.UpdateLayout();
            }

            int numChildren = VisualTreeHelper.GetChildrenCount(rootElement);
            for (int i = 0; i < numChildren; ++i)
            {
                Visual childElement = (Visual)VisualTreeHelper.GetChild(rootElement, i);

                if (childElement is ElementT typedChildElement)
                {
                    if (typedChildElement.Name == name)
                {
                        return typedChildElement;
                    }
                }

                ElementT foundElement = FindElementWithName<ElementT>(childElement, name);
                if (foundElement != null)
                {
                    return foundElement;
                }
            }

            return null;
        }

        /// <summary>
        /// Find the framework element for the specified connector.
        /// </summary>
        public static ElementT FindElementWithDataContextAndName<DataContextT, ElementT>(Visual rootElement, DataContextT data, string name)
            where DataContextT : class
            where ElementT : FrameworkElement
        {
            Trace.Assert(rootElement != null);

            if (rootElement is FrameworkElement rootFrameworkElement)
        {
                rootFrameworkElement.UpdateLayout();
        }

            int numChildren = VisualTreeHelper.GetChildrenCount(rootElement);
            for (int i = 0; i < numChildren; ++i)
            {
                Visual childElement = (Visual)VisualTreeHelper.GetChild(rootElement, i);

                if (childElement is ElementT typedChildElement &&
                    typedChildElement.DataContext == data)
                {
                    if (typedChildElement.Name == name)
                {
                        return typedChildElement;
                    }
                }

                ElementT foundElement = FindElementWithDataContextAndName<DataContextT, ElementT>(childElement, data, name);
                if (foundElement != null)
                {
                    return foundElement;
                }
            }

            return null;
        }

        /// <summary>
        /// Find the framework element for the specified connector.
        /// </summary>
        public static ElementT FindElementWithType<ElementT>(Visual rootElement)
            where ElementT : FrameworkElement
        {
            if (rootElement == null)
            {
                throw new ArgumentNullException("rootElement");
            }

            if (rootElement is FrameworkElement rootFrameworkElement)
        {
                rootFrameworkElement.UpdateLayout();
            }

            //
            // Check each child.
            //
            int numChildren = VisualTreeHelper.GetChildrenCount(rootElement);
            for (int i = 0; i < numChildren; ++i)
            {
                Visual childElement = (Visual)VisualTreeHelper.GetChild(rootElement, i);

                if (childElement is ElementT typedChildElement)
                    {
                    return typedChildElement;
                    }
                }

            //
            // Check sub-trees.
            //
            for (int i = 0; i < numChildren; ++i)
            {
                Visual childElement = (Visual)VisualTreeHelper.GetChild(rootElement, i);

                ElementT foundElement = FindElementWithType<ElementT>(childElement);
                if (foundElement != null)
                {
                    return foundElement;
                }
            }

            return null;
        }

        /// <summary>
        /// Find the framework element for the specified connector.
        /// </summary>
        public static ElementT FindElementWithDataContext<DataContextT, ElementT>(Visual rootElement, DataContextT data) 
            where DataContextT : class
            where ElementT : FrameworkElement
        {
            if (rootElement == null)
            {
                throw new ArgumentNullException("rootElement");
            }

            if (rootElement is FrameworkElement rootFrameworkElement)
            {
                rootFrameworkElement.UpdateLayout();
            }

            int numChildren = VisualTreeHelper.GetChildrenCount(rootElement);
            for (int i = 0; i < numChildren; ++i)
            {
                Visual childElement = (Visual)VisualTreeHelper.GetChild(rootElement, i);

                if (childElement is ElementT typedChildElement &&
                    typedChildElement.DataContext == data)
                {
                    return typedChildElement;
                }

                ElementT foundElement = FindElementWithDataContext<DataContextT, ElementT>(childElement, data);
                if (foundElement != null)
                {
                    return foundElement;
                }
            }

            return null;
        }

        /// <summary>
        /// Walk up the visual tree and find a template for the specified type.
        /// Returns null if none was found.
        /// </summary>
        public static DataTemplateT FindTemplateForType<DataTemplateT>(Type type, FrameworkElement element)
            where DataTemplateT : class
                    {
            object resource = element.TryFindResource(new DataTemplateKey(type));
            if (resource is DataTemplateT dataTemplate)
                        {
                return dataTemplate;
                        }

            if (type.BaseType != null &&
                type.BaseType != typeof(object))
            {
                dataTemplate = FindTemplateForType<DataTemplateT>(type.BaseType, element);
                if (dataTemplate != null)
                {
                    return dataTemplate;
                    }
                }

            foreach (Type interfaceType in type.GetInterfaces())
            {
                dataTemplate = FindTemplateForType<DataTemplateT>(interfaceType, element);
                if (dataTemplate != null)
                {
                    return dataTemplate;
                }
            }

            return null;
        }

        /// <summary>
        /// Search the visual tree for template and instance it.
        /// </summary>
        public static FrameworkElement CreateVisual(Type type, FrameworkElement element, object dataContext)
        {
            DataTemplate template = FindTemplateForType<DataTemplate>(type, element);
            if (template == null)
            {
                throw new ApplicationException("Failed to find DataTemplate for type " + type.Name);
            }

            FrameworkElement visual = (FrameworkElement)template.LoadContent();
            visual.Resources = element.Resources;
            visual.DataContext = dataContext;
            return visual;
        }

        /// <summary>
        /// Layout, measure and arrange the specified element.
        /// </summary>
        public static void InitaliseElement(FrameworkElement element)
        {
            element.UpdateLayout();
            element.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            element.Arrange(new Rect(0, 0, element.DesiredSize.Width, element.DesiredSize.Height));
        }


        /// <summary>
        /// Finds a particular type of UI element int he visual tree that has the specified data context.
        /// </summary>
        public static ICollection<T> FindTypedElements<T>(DependencyObject rootElement) where T : DependencyObject
        {
            List<T> foundElements = new List<T>();
            FindTypedElements(rootElement, foundElements);
            return foundElements;
        }

        /// <summary>
        /// Finds a particular type of UI element int he visual tree that has the specified data context.
        /// </summary>
        private static void FindTypedElements<T>(DependencyObject rootElement, List<T> foundElements) where T : DependencyObject
        {
            int numChildren = VisualTreeHelper.GetChildrenCount(rootElement);
            for (int i = 0; i < numChildren; ++i)
            {
                DependencyObject childElement = VisualTreeHelper.GetChild(rootElement, i);
                if (childElement is T element)
                {
                    foundElements.Add(element);
                    continue;
                }

                FindTypedElements(childElement, foundElements);
                }
            }

        /// <summary>
        /// Recursively dump out all elements in the visual tree.
        /// </summary>
        public static void DumpVisualTree(Visual root)
        {
            DumpVisualTree(root, 0);
        }

        /// <summary>
        /// Recursively dump out all elements in the visual tree.
        /// </summary>
        private static void DumpVisualTree(Visual root, int indentLevel)
        {
            string indentStr = new string(' ', indentLevel * 2);
            Trace.Write(indentStr);
            Trace.Write(root.GetType().Name);

            if (root is FrameworkElement rootElement)
        {
                if (rootElement.DataContext != null)
            {
                    Trace.Write(" [");
                    Trace.Write(rootElement.DataContext.GetType().Name);
                    Trace.Write("]");
            }
            }

            Trace.WriteLine("");

            int numChildren = VisualTreeHelper.GetChildrenCount(root);
            if (numChildren > 0)
            {
                Trace.Write(indentStr);
                Trace.WriteLine("{");

                for (int i = 0; i < numChildren; ++i)
                {
                    Visual child = (Visual)VisualTreeHelper.GetChild(root, i);
                    DumpVisualTree(child, indentLevel + 1);
            }

                Trace.Write(indentStr);
                Trace.WriteLine("}");
            }
        }
    }
}
