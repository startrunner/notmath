using Mathematica.Controls;
using System.Windows;
using System.Windows.Documents;

namespace Mathematica.Behaviors
{
    public class AutoSizeBehavior
    {
        public static readonly DependencyProperty IsAutoSizeEnabledProperty =
            DependencyProperty.RegisterAttached("IsAutoSizeEnabled", typeof(bool),
                typeof(AutoSizeBehavior), new PropertyMetadata(false, OnAutoSizeChanged));

        public static bool GetIsAutoSizeEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsAutoSizeEnabledProperty);
        }

        public static void SetIsAutoSizeEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsAutoSizeEnabledProperty, value);
        }

        private static void OnAutoSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mathBox = d as MathBox;
            if (mathBox == null) return;
            mathBox.TextChanged += (s, _) => Resize(s as MathBox);
        }

        private static void Resize(MathBox mathBox)
        {
            mathBox.Width = 9999;
            mathBox.Arrange(new Rect(0, 0, 9999, 9999));
            Rect firstRect = mathBox.Document.ContentStart.GetCharacterRect(LogicalDirection.Forward);
            Rect lastRect = mathBox.Document.ContentEnd.GetCharacterRect(LogicalDirection.Forward);
            mathBox.Width = lastRect.Right + mathBox.Document.PagePadding.Left + mathBox.Document.PagePadding.Right + 10;
        }
    }
}
