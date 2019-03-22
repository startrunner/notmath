using System.Windows;
using System.Windows.Documents;

namespace Mathematica.Behaviors
{
    public partial class FocusChildOnArrowBehavior
    {
        public static readonly DependencyProperty IsFocusChildOnArrowEnabledProperty =
            DependencyProperty.RegisterAttached("IsFocusChildOnArrowEnabled", typeof(bool),
                typeof(FocusChildOnArrowBehavior), new PropertyMetadata(false, OnFocusChildOnArrowChanged));

        public static readonly DependencyProperty ForwardUiElementProperty =
            DependencyProperty.RegisterAttached("ForwardUiElement", typeof(InlineUIContainer),
                typeof(FocusChildOnArrowBehavior), new PropertyMetadata(null));

        public static readonly DependencyProperty BackwardUiElementProperty =
            DependencyProperty.RegisterAttached("BackwardUiElement", typeof(InlineUIContainer),
                typeof(FocusChildOnArrowBehavior), new PropertyMetadata(null));

        public static bool GetIsFocusChildOnArrowEnabled(DependencyObject obj)
        {
            return (bool) obj.GetValue(IsFocusChildOnArrowEnabledProperty);
        }

        public static void SetIsFocusChildOnArrowEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsFocusChildOnArrowEnabledProperty, value);
        }

        public static InlineUIContainer GetForwardUiElement(DependencyObject obj)
        {
            return (InlineUIContainer) obj.GetValue(ForwardUiElementProperty);
        }

        public static void SetForwardUiElement(DependencyObject obj, InlineUIContainer value)
        {
            obj.SetValue(ForwardUiElementProperty, value);
        }

        public static InlineUIContainer GetBackwardUiElement(DependencyObject obj)
        {
            return (InlineUIContainer) obj.GetValue(BackwardUiElementProperty);
        }

        public static void SetBackwardUiElement(DependencyObject obj, InlineUIContainer value)
        {
            obj.SetValue(BackwardUiElementProperty, value);
        }
    }
}