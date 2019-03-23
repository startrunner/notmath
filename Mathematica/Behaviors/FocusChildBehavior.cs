using Mathematica.Controls;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using Mathematica.Extensions;

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
                InlineUIContainer forwardUiElement = caret.GetAdjacentUIContainer(LogicalDirection.Forward);
                InlineUIContainer backwardUiElement = caret.GetAdjacentUIContainer(LogicalDirection.Backward);

                mathBox.ForwardUiElement = forwardUiElement;
                mathBox.BackwardUiElement = backwardUiElement;
            }
        }

        private static void HandleKeyDown(object sender, KeyEventArgs e)
        {
            if (!Equals(e.OriginalSource, sender)) return;
            var mathBox = (MathBox)sender;
            if (!TryGetNotationAndDirection(mathBox, e.Key, out var direction, out var notation)) return;

            FocusMathElement(notation, direction);
            e.Handled = true;
        }

        private static bool TryGetNotationAndDirection(
            MathBox mathBox, Key key, out LogicalDirection direction,
            out NotationBase notation
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

            if (!(inlineUiContainer?.Child is NotationBase control))
            {
                notation = null;
                return false;
            }

            notation = control;
            return true;
        }

        private static void FocusMathElement(NotationBase notation, LogicalDirection direction)
        {
            if (direction == LogicalDirection.Forward) notation.FocusFirst();
            else notation.FocusLast();
        }
    }
}