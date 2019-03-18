using System.Windows;
using System.Windows.Annotations;
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
            if (e.NewValue == e.OldValue) return;

            if ((bool)e.NewValue)
            {
                box.PreviewKeyDown += HandleKeyDown;
                box.SelectionChanged += HandleSelectionChanged;
            }
            else
            {
                box.PreviewKeyDown -= HandleKeyDown;
                box.SelectionChanged -= HandleSelectionChanged;
            }
        }

        private static void HandleSelectionChanged(object sender, RoutedEventArgs e)
        {
            //throw new System.NotImplementedException();
        }

        private static void HandleKeyDown(object sender, KeyEventArgs e)
        {
            var mathBox = sender as MathBox;
            if (mathBox != e.OriginalSource || mathBox == null) return;
            if (e.Key != Key.Right && e.Key != Key.Left) return;
            if (!ShouldNavigate(e.Key, mathBox, out var navigationDirection, out var targetCaretPosition)) return;

            var traversalRequest = new TraversalRequest(navigationDirection);
            mathBox.MoveFocus(traversalRequest);
            if (Keyboard.FocusedElement is MathBox newFocus && newFocus.Parent == mathBox.Parent)
            {
                newFocus.SetCaretPosition(targetCaretPosition);
                e.Handled = true;
            }
        }

        private static bool ShouldNavigate(Key key, MathBox mathBox,
            out FocusNavigationDirection navigationDirection, out BoxCaretPosition targetCaretPosition)
        {
            navigationDirection = FocusNavigationDirection.Next;
            targetCaretPosition = BoxCaretPosition.Default;

            if (key == Key.Right && mathBox.CaretPosition.IsAtDocumentEnd())
            {
                navigationDirection = FocusNavigationDirection.Next;
                targetCaretPosition = BoxCaretPosition.Start;
                return true;
            }

            if (key == Key.Left && mathBox.CaretPosition.IsAtDocumentStart())
            {
                navigationDirection = FocusNavigationDirection.Previous;
                targetCaretPosition = BoxCaretPosition.End;
                return true;
            }

            return false;
        }
    }
}