using Mathematica.Controls;
using Mathematica.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Mathematica.Behaviors
{
    public partial class FocusChildOnArrowBehavior
    {
        private static void OnFocusChildOnArrowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MathBox mathBox = d as MathBox;
            if (mathBox == null) return;
            if (e.NewValue == e.OldValue) return;

            if ((bool)e.NewValue)
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
            MathBox mathBox = (MathBox)sender;
            if (Equals(e.OriginalSource, sender))
            {
                TextPointer caret = mathBox.CaretPosition;

                var forwardContext = caret.GetPointerContext(LogicalDirection.Forward);
                var backwardContext = caret.GetPointerContext(LogicalDirection.Backward);

                InlineUIContainer forwardUiElement = null;
                InlineUIContainer backwardUiElement = null;
                if (backwardContext == TextPointerContext.ElementStart)
                {
                    backwardUiElement = caret.GetNextContextPosition(LogicalDirection.Backward)?
                        .GetAdjacentElement(LogicalDirection.Backward) as InlineUIContainer;
                }
                if (backwardContext == TextPointerContext.ElementEnd)
                {
                    backwardUiElement = caret.GetAdjacentElement(LogicalDirection.Backward) as InlineUIContainer;
                }

                if (forwardContext == TextPointerContext.ElementEnd)
                {
                    forwardUiElement = caret.GetNextContextPosition(LogicalDirection.Forward)?
                        .GetAdjacentElement(LogicalDirection.Forward) as InlineUIContainer;
                }

                if (forwardContext == TextPointerContext.ElementStart)
                {
                    forwardUiElement = caret.GetAdjacentElement(LogicalDirection.Forward) as InlineUIContainer;
                }

                SetForwardUiElement(mathBox, forwardUiElement);
                SetBackwardUiElement(mathBox, backwardUiElement);
            }

        }

        private static void HandleKeyDown(object sender, KeyEventArgs e)
        {
            if (Equals(e.OriginalSource, sender))
            {
                MathBox mathBox = (MathBox)sender;
                if (TryGetElementAndDirection(mathBox, e.Key, out var direction, out var mathElementControl)) return;

                FocusMathElement(mathElementControl, direction);
                e.Handled = true;
            }
        }

        private static bool TryGetElementAndDirection(MathBox mathBox, Key key, out LogicalDirection direction,
            out MathElementControl mathElementControl)
        {
            mathElementControl = null;
            direction = LogicalDirection.Forward;

            InlineUIContainer forwardElement = GetForwardUiElement(mathBox);
            InlineUIContainer backwardElement = GetBackwardUiElement(mathBox);

            InlineUIContainer inlineUiContainer = null;

            if (key == Key.Right && forwardElement != null)
            {
                inlineUiContainer = forwardElement;
                direction = LogicalDirection.Forward;
            }

            if (key == Key.Left && backwardElement != null)
            {
                inlineUiContainer = backwardElement;
                direction = LogicalDirection.Backward;
            }

            if (!(inlineUiContainer?.Child is MathElementControl control)) return true;
            mathElementControl = control;
            return false;
        }

        private static void FocusMathElement(MathElementControl element, LogicalDirection direction)
        {
            if (direction == LogicalDirection.Forward)
                element.FocusFirst();
            else
                element.FocusLast();
            //var caretPosition = direction == LogicalDirection.Forward ? BoxCaretPosition.Start : BoxCaretPosition.End;
            //var box = direction == LogicalDirection.Forward ? ElementBox.Main : ElementBox.Sup;//element.GetFirstBox()/GetLastBox()
            //element.FocusBox(box, caretPosition);
        }
    }
}