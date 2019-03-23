using Mathematica.Controls;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace Mathematica.Behaviors
{
    public partial class FocusChildBehavior
    {
        private static void OnEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is MathBox mathBox)) return;
            if (e.NewValue == e.OldValue) return;

            if (e.NewValue is true)
            {
                mathBox.PreviewKeyDown += HandleKeyDown;
                mathBox.SelectionChanged += HandleSelectionChanged;
            }
            else
            {
                mathBox.PreviewKeyDown -= HandleKeyDown;
                mathBox.SelectionChanged -= HandleSelectionChanged;
            }
        }

        private static void HandleSelectionChanged(object sender, RoutedEventArgs e)
        {
            var mathBox = (MathBox)sender;
            if (Equals(e.OriginalSource, sender))
            {
                TextPointer caret = mathBox.CaretPosition;
                InlineUIContainer forwardUiElement = GetUIContainerRelativeToCaret(caret, LogicalDirection.Forward);
                InlineUIContainer backwardUiElement = GetUIContainerRelativeToCaret(caret, LogicalDirection.Backward);

                mathBox.ForwardUiElement = forwardUiElement;
                mathBox.BackwardUiElement = backwardUiElement;
            }

        }

        private static InlineUIContainer GetUIContainerRelativeToCaret(TextPointer caret, LogicalDirection direction)
        {
            TextPointerContext context = caret.GetPointerContext(direction);
            InlineUIContainer result;
            TextPointerContext skippingCase = direction == LogicalDirection.Backward ? TextPointerContext.ElementStart : TextPointerContext.ElementEnd;

            if (context == skippingCase)
            {
                result =
                    caret.GetNextContextPosition(direction)
                    ?.GetAdjacentElement(direction) as InlineUIContainer;
            }
            else if (context == TextPointerContext.ElementEnd)
            {
                result = caret.GetAdjacentElement(direction) as InlineUIContainer;
            }
            else result = null;

            return result;
        }

        private static void HandleKeyDown(object sender, KeyEventArgs e)
        {
            if (!Equals(e.OriginalSource, sender)) return;
            var mathBox = (MathBox)sender;
            if (!TryGetElementAndDirection(mathBox, e.Key, out var direction, out var mathElementControl)) return;

            FocusMathElement(mathElementControl, direction);
            e.Handled = true;
        }

        private static bool TryGetElementAndDirection(
            MathBox mathBox, Key key, out LogicalDirection direction,
            out MathElementControl mathElementControl
        )
        {
            direction = LogicalDirection.Forward;

            InlineUIContainer forwardElement = mathBox.ForwardUiElement;
            InlineUIContainer backwardElement = mathBox.BackwardUiElement;

            InlineUIContainer inlineUiContainer = null;
            if (key == Key.Right && forwardElement != null)
            {
                inlineUiContainer = forwardElement;
                direction = LogicalDirection.Forward;
            }
            else if (key == Key.Left && backwardElement != null)
            {
                inlineUiContainer = backwardElement;
                direction = LogicalDirection.Backward;
            }

            if (!(inlineUiContainer?.Child is MathElementControl control))
            {
                mathElementControl = null;
                return false;
            }

            mathElementControl = control;
            return true;
        }

        private static void FocusMathElement(MathElementControl element, LogicalDirection direction)
        {
            if (direction == LogicalDirection.Forward) element.FocusFirst();
            else element.FocusLast();
        }
    }
}