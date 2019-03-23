using System.Windows;
using System.Windows.Documents;

namespace Mathematica.Behaviors
{
    public partial class FocusChildBehavior
    {
        public static readonly DependencyProperty EnabledProperty =
            DependencyProperty.RegisterAttached("Enabled", typeof(bool),
                typeof(FocusChildBehavior), new PropertyMetadata(false, OnEnabledChanged));

        public static readonly DependencyProperty ForwardUiElementProperty =
            DependencyProperty.RegisterAttached("ForwardUiElement", typeof(InlineUIContainer),
                typeof(FocusChildBehavior), new PropertyMetadata(null));

        public static readonly DependencyProperty BackwardUiElementProperty =
            DependencyProperty.RegisterAttached("BackwardUiElement", typeof(InlineUIContainer),
                typeof(FocusChildBehavior), new PropertyMetadata(null));
    }
}