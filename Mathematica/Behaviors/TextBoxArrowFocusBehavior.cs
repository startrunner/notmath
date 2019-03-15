using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;

namespace Mathematica.Behaviors
{
    public class TextBoxArrowFocusBehavior
    {
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(TextBoxArrowFocusBehavior), new PropertyMetadata(false, OnIsEnabledChanged));

        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = d as TextBox;
            box.PreviewKeyDown += (sender, eventArgs) =>
            {
                if (eventArgs.Key == Key.Right && box.CaretIndex == box.Text.Length)
                {
                    var traversalRequest = new TraversalRequest(FocusNavigationDirection.Next);
                    box.MoveFocus(traversalRequest);
                    var scope = FocusManager.GetFocusScope(d);
                    if (FocusManager.GetFocusedElement(scope) is TextBox newFocus)
                    {
                        newFocus.CaretIndex = 0;
                        eventArgs.Handled = true;
                    }
                }
                if (eventArgs.Key == Key.Left && box.CaretIndex == 0)
                {
                    var traversalRequest = new TraversalRequest(FocusNavigationDirection.Previous);
                    box.MoveFocus(traversalRequest);
                    var scope = FocusManager.GetFocusScope(d);
                    var newFocus = FocusManager.GetFocusedElement(scope) as TextBox;
                    if (newFocus != null)
                    {
                        newFocus.CaretIndex = newFocus.Text.Length;
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
    }
}