using System.Windows;
using System.Windows.Documents;
using Mathematica.Contracts;
using Mathematica.Controls;
using Mathematica.Extensions;

namespace Mathematica.Behaviors
{
    public static class FocusParentBehavior
    {
        public static readonly DependencyProperty EnabledProperty =
            DependencyProperty.RegisterAttached("Enabled", typeof(bool),
                typeof(FocusParentBehavior),
                new PropertyMetadata(false, OnEnabledChanged));

        private static void OnEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is MathBox mathBox)) return;

            if (e.NewValue is true)
            {
                mathBox.ChildFocusFailed += MathBox_ChildFocusFailed;
            }
            else
            {
                mathBox.ChildFocusFailed -= MathBox_ChildFocusFailed;
            }
        }

        private static void MathBox_ChildFocusFailed(NotationBase sender, Direction direction)
        {
            var uiContainer = sender.FindParent<InlineUIContainer>();
            var mathBox = uiContainer.FindParent<MathBox>();
            LogicalDirection logicalDirection = ConvertDirectionToLogicalDirection(direction);
            mathBox.MoveCaretToTextElementBoundary(uiContainer, logicalDirection);
            mathBox.Focus();
        }

        private static LogicalDirection  ConvertDirectionToLogicalDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return LogicalDirection.Backward;
                case Direction.Right:
                    return LogicalDirection.Forward;
                default:
                    return LogicalDirection.Forward;
            }
        }
    }
}