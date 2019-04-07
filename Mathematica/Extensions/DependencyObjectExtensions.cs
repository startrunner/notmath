using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Mathematica.Extensions
{
    public static class DependencyObjectExtensions
    {
        public static T FindParent<T>(this DependencyObject dependencyObject)
            where T : DependencyObject
        {
            DependencyObject parent = LogicalTreeHelper.GetParent(dependencyObject);

            if (parent == null)
                return null;
            if (parent is T typedParent)
                return typedParent;
            return FindParent<T>(parent);
        }

        public static List<T> FindChildren<T>(this DependencyObject obj, int maxDepth = int.MaxValue)
        where T:DependencyObject
        {
            var children = new List<T>();
            FindChildren(obj, maxDepth, 0, children);
            return children;
        }

        public static void FindChildren<T>(this DependencyObject obj,
            int maxDepth, int depth, List<T> output)
        where T : DependencyObject
        {
            if (maxDepth == depth) return;

            var children = LogicalTreeHelper.GetChildren(obj);
            foreach (var child in children)
            {
                var typedChild = child as T;
                int levelOffset = Convert.ToInt32(typedChild != null);

                if (child is DependencyObject dependencyChild)
                {
                    FindChildren(dependencyChild, maxDepth, depth + levelOffset, output);
                }

                if (typedChild != null)
                    output.Add(typedChild);
            }
        }
    }
}