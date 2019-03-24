using Mathematica.Controls;
using System.Windows;
using System.Windows.Documents;

namespace Mathematica.Behaviors
{
    public static class AutoSizeBehavior
    {
        public static readonly DependencyProperty EnableAutoSizeProperty =
            DependencyProperty.RegisterAttached("EnableAutoSize", typeof(bool),
                typeof(AutoSizeBehavior), new PropertyMetadata(false, OnEnableAutoSizeChanged));

        private static void OnEnableAutoSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mathBox = d as MathBox;
            if (mathBox == null) return;
            mathBox.TextChanged += (s, _) => Resize(s as MathBox);
        }

        public static void Resize(this MathBox mathBox)
        {
            mathBox.Width = 9999;
            mathBox.Arrange(new Rect(0, 0, 9999, 9999));
            Rect firstRect = mathBox.Document.ContentStart.GetCharacterRect(LogicalDirection.Forward);
            Rect lastRect = mathBox.Document.ContentEnd.GetCharacterRect(LogicalDirection.Forward);
            mathBox.Width = lastRect.Right + mathBox.Document.PagePadding.Left + mathBox.Document.PagePadding.Right + 10;
        }
    }
}
