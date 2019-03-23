using Mathematica.Controls;
using Mathematica.Extensions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace Mathematica.Behaviors
{
    public class FocusSiblingOnArrowBehavior
    {
        public static readonly DependencyProperty IsFocusSiblingEnabledProperty =
            DependencyProperty.RegisterAttached("IsFocusSiblingEnabled", typeof(bool),
                typeof(FocusSiblingOnArrowBehavior), new PropertyMetadata(false, OnIsEnabledChanged));

        public static bool GetIsFocusSiblingEnabled(DependencyObject obj) =>
            (bool)obj.GetValue(IsFocusSiblingEnabledProperty);

        public static void SetIsFocusSiblingEnabled(DependencyObject obj, bool value) =>
            obj.SetValue(IsFocusSiblingEnabledProperty, value);

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is MathBox box)) return;
            if (e.NewValue == e.OldValue) return;

            if (e.NewValue is true) box.PreviewKeyDown += HandleKeyDown;
            else box.PreviewKeyDown -= HandleKeyDown;

        }

        private static void HandleKeyDown(object sender, KeyEventArgs e)
        {
            var mathBox = sender as MathBox;
            if (e.Key != Key.Right && e.Key != Key.Left) return;
            if (mathBox == null || mathBox != e.OriginalSource) return;
            if (!ShouldNavigate(e.Key, mathBox, out LogicalDirection direction)) return;

            FocusSibling(mathBox, direction);
        }

        private static void FocusSibling(MathBox mathBox, LogicalDirection direction)
        {
            NotationBase parent = mathBox.FindParent<NotationBase>();
            if (parent == null) return;

            if (direction == LogicalDirection.Forward) parent.FocusNext();
            if (direction == LogicalDirection.Backward) parent.FocusPrevious();
        }

        private static bool ShouldNavigate(
            Key key, 
            MathBox mathBox,
            out LogicalDirection logicalDirection)
        {
            logicalDirection = LogicalDirection.Forward;

            if (key == Key.Right && mathBox.CaretPosition.IsAtDocumentEnd())
            {
                logicalDirection = LogicalDirection.Forward;
                return true;
            }

            if (key == Key.Left && mathBox.CaretPosition.IsAtDocumentStart())
            {
                logicalDirection = LogicalDirection.Backward;
                return true;
            }

            return false;
        }
    }
}