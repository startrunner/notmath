using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Mathematica.Controls;
using Mathematica.Extensions;

namespace Mathematica.Behaviors
{
    public partial class MathBoxBehaviors
    {
        public static bool GetAutoSize(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoSizeProperty);
        }
        public static void SetAutoSize(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoSizeProperty, value);
        }

        public static readonly DependencyProperty AutoSizeProperty =
            DependencyProperty.RegisterAttached("AutoSize", typeof(bool),
                typeof(MathBoxBehaviors), new PropertyMetadata(false, OnAutoSizeChanged));

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
