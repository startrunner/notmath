using System.Windows;
using Mathematica.Controls;

namespace Mathematica.Behaviors
{
    public partial class MathBoxBehaviors
    {
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FocusChildOnArrowProperty =
            DependencyProperty.Register("FocusChildOnArrow", typeof(bool), 
                typeof(MathBox), new PropertyMetadata(false, OnFocusChildOnArrowChanged));

        private static void OnFocusChildOnArrowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }
    }
}