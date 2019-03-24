using System.Collections.Generic;
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

        public static IEnumerable<T> GetChildren<T>(this DependencyObject obj)
        where T : DependencyObject
        {
            var children = LogicalTreeHelper.GetChildren(obj);
            foreach (var child in children)
            {
                if (child is DependencyObject dependencyObject)
                    foreach (var subChild in GetChildren<T>(dependencyObject))
                        yield return subChild;
                if (child is T typedChild)
                    yield return typedChild;
            }
        }
    }
}