using Mathematica.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
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
            RemovePagePadding(mathBox);
            mathBox.Loaded += (s, _) => RemovePagePadding(s as MathBox);
            mathBox.TextChanged += HandleTextChanged;
            mathBox.SizeChanged += HandleSizeChanged;
            AttachResized(mathBox, HandleResized);
        }

        private static void HandleSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var mathBox = sender as MathBox;
            mathBox.RaiseEvent(new RoutedEventArgs(ResizedEvent, mathBox));
        }

        private static void HandleResized(object sender, RoutedEventArgs e)
        {
            if (sender == e.OriginalSource) return;
            var mathBox = sender as MathBox;
            Resize(mathBox);
        }

        private static void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender == e.OriginalSource)
            {
                Resize(sender as MathBox);
            }
        }

        private static void RemovePagePadding(MathBox mathBox) {
            mathBox.Document.PagePadding = new Thickness(0);
            mathBox.Document.PageWidth = 1000;
            mathBox.HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Disabled;
        }
        public static void Resize(this MathBox mathBox)
        {
            mathBox.Width = 9999;
            mathBox.Arrange(new Rect(0, 0, 9999, 9999));
            Rect firstRect = mathBox.Document.ContentStart.GetCharacterRect(LogicalDirection.Forward);
            Rect lastRect = mathBox.Document.ContentEnd.GetCharacterRect(LogicalDirection.Forward);
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
    }
}
