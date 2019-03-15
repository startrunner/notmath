using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Mathematica.Controls;
using Mathematica.Extensions;

namespace Mathematica.Behaviors
{
    public partial class MathBoxBehaviors
    {
        public static readonly DependencyProperty ArrowNavigationProperty =
            DependencyProperty.RegisterAttached("ArrowNavigation", typeof(bool),
                typeof(MathBoxBehaviors), new PropertyMetadata(false, OnIsEnabledChanged));

        public static bool GetArrowNavigation(DependencyObject obj)
        {
            return (bool)obj.GetValue(ArrowNavigationProperty);
        }

        public static void SetArrowNavigation(DependencyObject obj, bool value)
        {
            obj.SetValue(ArrowNavigationProperty, value);
        }

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = d as MathBox;
            if (box == null) return;
            box.PreviewKeyDown += (sender, eventArgs) =>
            {
                if (eventArgs.Key == Key.Right && IsCaretAtBoxEnd(box))
                {
                    var traversalRequest = new TraversalRequest(FocusNavigationDirection.Next);
                    box.MoveFocus(traversalRequest);
                    var scope = FocusManager.GetFocusScope(d);
                    if (FocusManager.GetFocusedElement(scope) is MathBox newFocus)
                    {
                        newFocus.CaretPosition = newFocus.Document.ContentStart;
                        eventArgs.Handled = true;
                    }
                }

                if (eventArgs.Key == Key.Left && IsCaretAtBoxStart(box))
                {
                    var traversalRequest = new TraversalRequest(FocusNavigationDirection.Previous);
                    box.MoveFocus(traversalRequest);
                    var scope = FocusManager.GetFocusScope(d);
                    if (FocusManager.GetFocusedElement(scope) is MathBox newFocus)
                    {
                        newFocus.CaretPosition = newFocus.Document.ContentEnd;
                        eventArgs.Handled = true;
                    }
                }

                if (eventArgs.Key == Key.Up)
                {
                    var traversalRequest = new TraversalRequest(FocusNavigationDirection.Up);
                    box.MoveFocus(traversalRequest);
                }

                if (eventArgs.Key == Key.Down)
                {
                    var traversalRequest = new TraversalRequest(FocusNavigationDirection.Down);
                    box.MoveFocus(traversalRequest);
                }
            };
        }

        private static bool IsCaretAtBoxStart(MathBox box)
        {
            int offsetToStart = box.CaretPosition.GetOffsetToPosition(box.Document.ContentStart);
            return offsetToStart == -2;
        }

        private static bool IsCaretAtBoxEnd(MathBox box)
        {
            int offsetToEnd = box.CaretPosition.GetOffsetToPosition(box.Document.ContentEnd);
            return offsetToEnd == 2;
        }
    }
}