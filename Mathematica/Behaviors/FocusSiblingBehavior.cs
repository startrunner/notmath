using Mathematica.Controls;
using Mathematica.Extensions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using Mathematica.Contracts;

namespace Mathematica.Behaviors
{
    public class FocusSiblingBehavior
    {
        public static readonly DependencyProperty EnabledProperty =
            DependencyProperty.RegisterAttached("Enabled", typeof(bool),
                typeof(FocusSiblingBehavior), new PropertyMetadata(false, OnEnabledChanged));

        private static void OnEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is MathBox box)) return;
            if (e.NewValue == e.OldValue) return;

            if (e.NewValue is true) box.PreviewKeyDown += HandleKeyDown;
            else box.PreviewKeyDown -= HandleKeyDown;
        }

        private static void HandleKeyDown(object sender, KeyEventArgs e)
        {
            var mathBox = sender as MathBox;
            if (e.Key != Key.Right && e.Key != Key.Left && e.Key != Key.Down && e.Key != Key.Up) return;
            if (mathBox == null || mathBox != e.OriginalSource) return;
            if (!ShouldNavigate(e.Key, mathBox, out var direction)) return;

            FocusSibling(mathBox, direction);
        }

        private static void FocusSibling(MathBox mathBox, Direction direction)
        {
            NotationBase parent = mathBox.FindParent<NotationBase>();
            parent?.FocusDirection(direction);
        }

        private static bool ShouldNavigate(
            Key key, 
            MathBox mathBox,
            out Direction logicalDirection)
        {
            logicalDirection = Direction.Right;

            if (key == Key.Up)
            {
                logicalDirection = Direction.Up;
                return true;
            }

            if (key == Key.Down)
            {
                logicalDirection = Direction.Down;
                return true;
            }

            if (key == Key.Right && mathBox.CaretPosition.IsAtDocumentEnd())
            {
                logicalDirection = Direction.Right;
                return true;
            }

            if (key == Key.Left && mathBox.CaretPosition.IsAtDocumentStart())
            {
                logicalDirection = Direction.Left;
                return true;
            }

            return false;
        }
    }
}