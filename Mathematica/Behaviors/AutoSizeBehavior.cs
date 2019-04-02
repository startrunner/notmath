using Mathematica.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Mathematica.Behaviors
{
    public static class AutoSizeBehavior
    {
        const double ManyMuch = 99999;

        private static void OnEnableAutoSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mathBox = d as MathBox;
            if (mathBox == null) return;

            if (mathBox.EnableAutoSize)
            {
                PrepareMathBox(mathBox);
                AttachEvents(mathBox);
            }
            else
            {
                DetachEvents(mathBox);
            }
        }

        private static void AttachEvents(MathBox mathBox)
        {
            mathBox.Loaded += HandleLoaded;
            mathBox.TextChanged += HandleTextChanged;
            mathBox.SizeChanged += HandleSizeChanged;
            AttachResized(mathBox, HandleResized);
        }

        private static void DetachEvents(MathBox mathBox)
        {
            mathBox.Loaded -= HandleLoaded;
            mathBox.TextChanged -= HandleTextChanged;
            mathBox.SizeChanged -= HandleSizeChanged;
            DetachResized(mathBox, HandleResized);
        }

        private static void HandleLoaded(object sender, RoutedEventArgs e)
        {
            var mathBox = sender as MathBox;
            if (mathBox == null) return;
            PrepareMathBox(mathBox);
        }

        private static void HandleSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var mathBox = sender as MathBox;
            mathBox.RaiseEvent(new RoutedEventArgs(ResizedEvent, mathBox));
        }

        private static void HandleResized(object sender, RoutedEventArgs e)
        {
            if (sender == e.OriginalSource) return;
            if (sender is MathBox mathBox)
                Resize(sender as MathBox);
        }

        private static void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender != e.OriginalSource) return;
            if (sender is MathBox mathBox)
                Resize(sender as MathBox);
        }

        private static void PrepareMathBox(MathBox mathBox)
        {
            mathBox.Document.PagePadding = new Thickness(0);
            mathBox.Document.PageWidth = ManyMuch;
            mathBox.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
        }

        public static void Resize(this MathBox mathBox)
        {
            if (mathBox.Visibility != Visibility.Visible) return;

            mathBox.Width = ManyMuch;
            mathBox.Arrange(new Rect(0, 0, ManyMuch, ManyMuch));

            Rect lastRect = mathBox.Document.ContentEnd.GetCharacterRect(LogicalDirection.Forward);
            if (lastRect == Rect.Empty) return;
            mathBox.Width = lastRect.Right + mathBox.Document.PagePadding.Left + mathBox.Document.PagePadding.Right;
        }

        public static readonly RoutedEvent ResizedEvent =
            EventManager.RegisterRoutedEvent("Resized", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MathBox));
        private static void AttachResized(MathBox mathBox, RoutedEventHandler handler)
        {
            mathBox.AddHandler(ResizedEvent, handler);
        }
        private static void DetachResized(MathBox mathBox, RoutedEventHandler handler)
        {
            mathBox.RemoveHandler(ResizedEvent, handler);
        }

        public static readonly DependencyProperty EnableAutoSizeProperty =
            DependencyProperty.RegisterAttached("EnableAutoSize", typeof(bool),
                typeof(AutoSizeBehavior), new PropertyMetadata(false, OnEnableAutoSizeChanged));
    }
}
