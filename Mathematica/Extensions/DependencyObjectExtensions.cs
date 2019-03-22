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
    }
}