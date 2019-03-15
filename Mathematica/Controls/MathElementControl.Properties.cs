using System.Windows;
using Mathematica.Models;

namespace Mathematica.Controls
{
    public partial class MathElementControl
    {
        public MathElement Value
        {
            get { return (MathElement)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(MathElement),
                typeof(Controls.MathElementControl), new PropertyMetadata(null));
    }
}